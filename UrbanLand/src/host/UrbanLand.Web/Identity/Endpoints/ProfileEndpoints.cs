using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Contracts.Requests;
using UrbanLand.Web.Identity.Contracts.Responses;
using UrbanLand.Web.Identity.Data;

namespace UrbanLand.Web.Identity.Endpoints;

public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/profile").WithTags("Profile");
		
		group.MapGet("/me", GetCurrentUserAsync)
			.WithName("GetCurrentUser")
			.WithSummary("Get Current User")
			.WithDescription("Get current user information")
			.Produces<UserResponse>()
			.Produces(StatusCodes.Status401Unauthorized)
			.RequireAuthorization();
		
		group.MapPut("/profile", UpdateProfileAsync)
			.WithName("UpdateProfile")
			.WithSummary("Update Profile")
			.WithDescription("Update user profile")
			.Produces<UserResponse>()
			.Produces<Error>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.RequireAuthorization();
		
		group.MapPost("/change-password", ChangePasswordAsync)
			.WithName("ChangePassword")
			.WithSummary("Change Password")
			.WithDescription("Change user password")
			.Produces(StatusCodes.Status200OK)
			.Produces<Error>(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.RequireAuthorization();
		
		return app;
	}
		
	private static async Task<IResult> GetCurrentUserAsync(
		HttpContext httpContext,
		UserManager<ApplicationUser> userManager)
	{
		var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
		var user = await userManager.FindByIdAsync(userId!);

		if (user == null)
		{
			return TypedResults.Unauthorized();
		}
		
		var roles = await userManager.GetRolesAsync(user);

		return TypedResults.Ok(new UserResponse
		{
			Id = user.Id,
			Email = user.Email ?? string.Empty,
			DisplayName = user.DisplayName,
			AvatarUrl = user.AvatarUrl,
			Role = roles.FirstOrDefault() ?? "User",
			CreatedAt = user.CreatedAt
		});
	}

	private static async Task<IResult> UpdateProfileAsync(
		[FromBody] UpdateProfileRequest request,
		HttpContext httpContext,
		UserManager<ApplicationUser> userManager)
	{
		var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
		var user = await userManager.FindByIdAsync(userId!);

		if (user == null)
		{
			return TypedResults.BadRequest(new Error("USER_NOT_FOUND", "User not found"));
		}

		if (!string.IsNullOrWhiteSpace(request.DisplayName))
		{
			user.DisplayName = request.DisplayName;
		}

		if (request.AvatarUrl != null)
		{
			user.AvatarUrl = request.AvatarUrl;
		}

		await userManager.UpdateAsync(user);

		var roles = await userManager.GetRolesAsync(user);

		return TypedResults.Ok(new UserResponse
		{
			Id = user.Id,
			Email = user.Email ?? string.Empty,
			DisplayName = user.DisplayName,
			AvatarUrl = user.AvatarUrl,
			Role = roles.FirstOrDefault() ?? "User",
			CreatedAt = user.CreatedAt
		});
	}
	
	private static async Task<IResult> ChangePasswordAsync(
		[FromBody] ChangePasswordRequest request,
		HttpContext httpContext,
		UserManager<ApplicationUser> userManager)
	{
		var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
		var user = await userManager.FindByIdAsync(userId!);

		if (user == null)
		{
			return TypedResults.BadRequest(new Error("USER_NOT_FOUND", "User not found"));
		}

		var result = await userManager.ChangePasswordAsync(
			user, 
			request.CurrentPassword, 
			request.NewPassword);

		if (!result.Succeeded)
		{
			return TypedResults.BadRequest(new Error(
				"PASSWORD_CHANGE_FAILED", 
				"Failed to change password")
			);
		}

		return TypedResults.Ok(new { Message = "Password changed successfully" });
	}
}
