using Microsoft.AspNetCore.Mvc;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Application.Services;
using UrbanLand.Web.Identity.Presentation.Models.Requests;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Presentation.Endpoints;

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
		ICurrentUserAccessor userAccessor,
		IUserService userService)
	{
		var userId = userAccessor.UserId;
		
		var result = await userService.GetAsync(UserId.From(userId));
		if (result.IsFailure)
		{
			return TypedResults.Unauthorized();
		}

		return TypedResults.Ok(result.Value);
	}

	private static async Task<IResult> UpdateProfileAsync(
		[FromBody] UpdateProfileRequest request,
		ICurrentUserAccessor userAccessor,
		IUserService userService)
	{
		if (string.IsNullOrWhiteSpace(request.DisplayName) || request.AvatarUrl is null)
		{
			return TypedResults.BadRequest(new Error("USER_NOT_FOUND", "User not found"));
		}
		
		var userId = userAccessor.UserId;
		var result = await userService.UpdateProfileAsync(UserId.From(userId), request);

		if (result.IsFailure)
		{
			return TypedResults.Unauthorized();
		}
		
		return TypedResults.Ok(result.Value);
	}
	
	private static async Task<IResult> ChangePasswordAsync(
		[FromBody] ChangePasswordRequest request,
		ICurrentUserAccessor userAccessor,
		IUserService userService)
	{
		var userId = userAccessor.UserId;
		var result = await userService.ChangePasswordAsync(UserId.From(userId), request);

		if (result.IsFailure)
		{
			return TypedResults.BadRequest(result.Error);
		}
		
		return TypedResults.Ok(new { Message = "Password changed successfully" });
	}
}
