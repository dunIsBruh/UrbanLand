using Identity.Application.Services;
using Identity.Infrastructure.Entities;
using Identity.Presentation.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Primitives;

namespace Identity.Infrastructure.Services;

public class IdentityAdminService(UserManager<ApplicationUser> userManager) : IAdminService
{
    public async Task<List<UserResponse>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken: ct);

        var userResponses = new List<UserResponse>();
        
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userResponses.Add(new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                Role = roles.FirstOrDefault() ?? "User",
                CreatedAt = user.CreatedAt
            });
        }
        
        return userResponses;
    }

    public async Task<Result<UserResponse>> SetUserRoleAsync(Guid userId, string role, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result<UserResponse>.Failure(new Error("USER_NOT_FOUND", "User not found"));
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            await userManager.RemoveFromRolesAsync(user, currentRoles);
        }
        await userManager.AddToRoleAsync(user, role);

        return Result<UserResponse>.Success(new UserResponse
        {
            Id = user.Id,
            Email = user.Email ?? "",
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = role,
            CreatedAt = user.CreatedAt
        });
    }
}