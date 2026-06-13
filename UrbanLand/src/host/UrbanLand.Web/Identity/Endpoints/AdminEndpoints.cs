using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Contracts.Responses;
using UrbanLand.Web.Identity.Data;

namespace UrbanLand.Web.Identity.Endpoints;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Authentication");

        group.MapGet("/users", GetAllUsersAsync)
            .WithName("GetAllUsers")
            .WithSummary("Get All Users")
            .WithDescription("Get all users (Admin only)")
            .Produces<List<UserResponse>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminPolicy");

        group.MapPost("/users/{userId:guid}/role", SetUserRoleAsync)
            .WithName("SetUserRole")
            .WithSummary("Set User Role")
            .WithDescription("Set user role (Admin only)")
            .Produces<UserResponse>()
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminPolicy");

        return app;
    }

    private static async Task<IResult> GetAllUsersAsync(
        UserManager<ApplicationUser> userManager)
    {
        var users = await userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(100)
            .ToListAsync();

        var userResponses = new List<UserResponse>();
        
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userResponses.Add(new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                Role = roles.FirstOrDefault() ?? "User",
                CreatedAt = user.CreatedAt
            });
        }

        return TypedResults.Ok(userResponses);
    }

    private static async Task<IResult> SetUserRoleAsync(
        Guid userId,
        [FromBody] SetRoleRequest request,
        UserManager<ApplicationUser> userManager)
    {
        if (request.Role is not ("User" or "Admin"))
        {
            return TypedResults.BadRequest(new Error("INVALID_ROLE", "Role must be 'User' or 'Admin'"));
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            return TypedResults.BadRequest(new Error("USER_NOT_FOUND", "User not found"));
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        
        if (currentRoles.Any())
        {
            await userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        await userManager.AddToRoleAsync(user, request.Role);

        return TypedResults.Ok(new UserResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = request.Role,
            CreatedAt = user.CreatedAt
        });
    }
}