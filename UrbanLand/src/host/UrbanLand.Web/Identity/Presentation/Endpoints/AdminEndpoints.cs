using Microsoft.AspNetCore.Mvc;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Application.Services;
using UrbanLand.Web.Identity.Presentation.Models.Requests;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Presentation.Endpoints;

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

    private static async Task<IResult> GetAllUsersAsync(IAdminService adminService)
    {
        // TODO: think about check
        var result = await adminService.GetUsersAsync();
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> SetUserRoleAsync(
        Guid userId,
        [FromBody] SetRoleRequest request,
        IAdminService adminService)
    {
        if (request.Role is not ("User" or "Admin"))
        {
            return TypedResults.BadRequest(new Error("INVALID_ROLE", "Role must be 'User' or 'Admin'"));
        }

        var result = await adminService.SetUserRoleAsync(userId, request.Role);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }
        
        return TypedResults.Ok(result.Value);
    }
}