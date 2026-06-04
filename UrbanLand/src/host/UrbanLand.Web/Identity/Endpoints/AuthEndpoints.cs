using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Contracts.Requests;
using UrbanLand.Web.Identity.Contracts.Responses;
using UrbanLand.Web.Identity.Data;
using UrbanLand.Web.Identity.Services;

namespace UrbanLand.Web.Identity.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication");
        
        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Register")
            .WithDescription("Register a new user")
            .AllowAnonymous();

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Login")
            .WithDescription("Authenticate user and get tokens")
            .AllowAnonymous();

        group.MapPost("/refresh", RefreshTokenAsync)
            .WithName("Refresh")
            .WithSummary("Refresh Token")
            .WithDescription("Refresh access token using refresh token")
            .AllowAnonymous();
        

        group.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .WithSummary("Get Current User")
            .WithDescription("Get current user information")
            .RequireAuthorization();

        group.MapPut("/profile", UpdateProfileAsync)
            .WithName("UpdateProfile")
            .WithSummary("Update Profile")
            .WithDescription("Update user profile")
            .RequireAuthorization();

        group.MapPost("/change-password", ChangePasswordAsync)
            .WithName("ChangePassword")
            .WithSummary("Change Password")
            .WithDescription("Change user password")
            .RequireAuthorization();

        group.MapPost("/logout", LogoutAsync)
            .WithName("Logout")
            .WithSummary("Logout")
            .WithDescription("Revoke refresh token")
            .RequireAuthorization();

        group.MapPost("/logout-all", LogoutAllAsync)
            .WithName("LogoutAll")
            .WithSummary("Logout All")
            .WithDescription("Revoke all refresh tokens")
            .RequireAuthorization();
        

        group.MapGet("/users", GetAllUsersAsync)
            .WithName("GetAllUsers")
            .WithSummary("Get All Users")
            .WithDescription("Get all users (Admin only)")
            .RequireAuthorization("AdminPolicy");

        group.MapPost("/users/{userId:guid}/role", SetUserRoleAsync)
            .WithName("SetUserRole")
            .WithSummary("Set User Role")
            .WithDescription("Set user role (Admin only)")
            .RequireAuthorization("AdminPolicy");

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        ILogger<AuthEndpointsMarker> logger
        )
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
        {
            return TypedResults.BadRequest(new Error("VALIDATION_ERROR", "Invalid email address"));
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            return TypedResults.BadRequest(new Error("VALIDATION_ERROR", "Password must be at least 8 characters"));
        }
        
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(new Error("IDENTITY_ERROR", "User creation failed"));
        }

        await userManager.AddToRoleAsync(user, "User");

        logger.LogInformation("New user registered: {Email}", request.Email);

        var authResponse = await tokenService.GenerateTokensAsync(user);

        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);

        return TypedResults.Ok(authResponse);
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        TokenService tokenService,
        ILogger<AuthEndpointsMarker> logger)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return TypedResults.Unauthorized();
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        
        if (!isPasswordValid)
        {
            await userManager.AccessFailedAsync(user);
            
            logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
            return TypedResults.Unauthorized();
        }

        await userManager.ResetAccessFailedCountAsync(user);

        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        logger.LogInformation("User logged in: {Email}", request.Email);

        var authResponse = await tokenService.GenerateTokensAsync(user);

        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);

        return TypedResults.Ok(authResponse);
    }

    private static async Task<IResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        HttpContext httpContext,
        TokenService tokenService)
    {
        var refreshToken = httpContext.Request.GetRefreshTokenCookie();

        if (string.IsNullOrWhiteSpace(request.AccessToken) || 
            string.IsNullOrWhiteSpace(refreshToken))
        {
            return TypedResults.BadRequest(new Error("VALIDATION_ERROR", "Access token and refresh token are required"));
        }

        var authResponse = await tokenService.RefreshTokensAsync(
            request.AccessToken, 
            refreshToken);

        if (authResponse == null)
        {
            httpContext.Response.ClearRefreshTokenCookie();
            return TypedResults.BadRequest(new Error("INVALID_TOKEN", "Invalid or expired refresh token"));
        }

        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);

        return TypedResults.Ok(authResponse);
    }

    private static async Task<IResult> GetCurrentUserAsync(
        HttpContext httpContext,
        UserManager<ApplicationUser> userManager)
    {
        var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await userManager.FindByIdAsync(userId!);
        var roles = await userManager.GetRolesAsync(user!);

        return TypedResults.Ok(new UserResponse
        {
            Id = user!.Id,
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
            return TypedResults.BadRequest(new Error("PASSWORD_CHANGE_FAILED", "Failed to change password"));
        }

        return TypedResults.Ok(new { Message = "Password changed successfully" });
    }

    private static async Task<IResult> LogoutAsync(
        HttpContext httpContext,
        TokenService tokenService)
    {
        var refreshToken = httpContext.Request.GetRefreshTokenCookie();

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            await tokenService.RevokeRefreshTokenAsync(refreshToken);
        }

        httpContext.Response.ClearRefreshTokenCookie();

        return TypedResults.Ok(new { Message = "Logged out successfully" });
    }

    private static async Task<IResult> LogoutAllAsync(
        HttpContext httpContext,
        TokenService tokenService)
    {
        var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (Guid.TryParse(userId, out var guid))
        {
            await tokenService.RevokeAllUserRefreshTokensAsync(guid);
        }

        return TypedResults.Ok(new { Message = "Logged out from all devices" });
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

public sealed record SetRoleRequest
{
    [Required]
    [RegularExpression("^(User|Admin)$")]
    public string Role { get; init; } = string.Empty;
}

public abstract class AuthEndpointsMarker;