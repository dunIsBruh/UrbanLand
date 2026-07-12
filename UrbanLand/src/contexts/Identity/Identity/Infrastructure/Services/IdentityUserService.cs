using Core.Identity;
using Core.Primitives;
using Identity.Application.Services;
using Identity.Infrastructure.Entities;
using Identity.Presentation.Models.Requests;
using Identity.Presentation.Models.Responses;

namespace Identity.Infrastructure.Services;

public class IdentityUserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Result<UserResponse>> GetAsync(UserId userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user == null)
        {
            return Result<UserResponse>.Failure(new Error("UNAUTHORIZED", "User not found"));
        }
        
        var roles = await userManager.GetRolesAsync(user);
        
        return Result<UserResponse>.Success(new UserResponse
        {
        	Id = user.Id,
        	Email = user.Email ?? string.Empty,
        	DisplayName = user.DisplayName,
        	AvatarUrl = user.AvatarUrl,
        	Role = roles.FirstOrDefault() ?? "User",
        	CreatedAt = user.CreatedAt
        });
    }

    public async Task<Result<UserResponse>> UpdateProfileAsync(
	    UserId userId,
	    UpdateProfileRequest request,
	    CancellationToken ct = default)
    {
	    var user = await userManager.FindByIdAsync(userId.Value.ToString());
	    if (user is null)
	    {
		    return Result<UserResponse>.Failure(new Error("UNAUTHORIZED", "User not found"));
	    }

	    if (!string.IsNullOrWhiteSpace(request.DisplayName))
	    {
		    user.DisplayName = request.DisplayName;
	    }
	    if (request.AvatarUrl is not null)
	    {
		    user.AvatarUrl = request.AvatarUrl;
	    }

	    await userManager.UpdateAsync(user);
	    var roles = await userManager.GetRolesAsync(user);
	    return Result<UserResponse>.Success(new UserResponse
	    {
		    Id = user.Id,
		    Email = user.Email ?? string.Empty,
		    DisplayName = user.DisplayName,
		    AvatarUrl = user.AvatarUrl,
		    Role = roles.FirstOrDefault() ?? "User",
		    CreatedAt = user.CreatedAt
	    });
    }

    public async Task<Result> ChangePasswordAsync(
	    UserId userId, 
	    ChangePasswordRequest request, 
	    CancellationToken ct = default)
    {
	    var user = await userManager.FindByIdAsync(userId.Value.ToString());
	    if (user is null)
	    {
		    return Result.Failure(new Error("USER_NOT_FOUND", "User not found"));
	    }

	    var result = await userManager.ChangePasswordAsync(
		    user,
		    request.CurrentPassword,
		    request.NewPassword);

	    if (!result.Succeeded)
	    {
		    return Result.Failure(
			    new Error(
				    "PASSWORD_CHANGE_FAILED",
				    string.Join(", ", result.Errors.Select(e => e.Description))
			    ));
	    }

	    return Result.Success();
    }
}