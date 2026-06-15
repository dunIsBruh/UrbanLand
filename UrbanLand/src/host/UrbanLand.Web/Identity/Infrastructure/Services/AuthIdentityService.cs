using Microsoft.AspNetCore.Identity;
using SharedKernel.Primitives;
using UrbanLand.Web.Identity.Application.Entities;
using UrbanLand.Web.Identity.Application.Services;
using UrbanLand.Web.Identity.Infrastructure.Services.Tokens;
using UrbanLand.Web.Identity.Presentation.Models.Requests;
using UrbanLand.Web.Identity.Presentation.Models.Responses;

namespace UrbanLand.Web.Identity.Infrastructure.Services;

public class AuthIdentityService(
    UserManager<ApplicationUser> userManager,
    TokenService tokenService,
    ILogger<AuthIdentityService> logger)
    : IAuthService
{

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
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
            return Result<AuthResponse>.Failure(new Error("IDENTITY_ERROR", "User creation failed"));
        }

        await userManager.AddToRoleAsync(user, "User");

        logger.LogInformation("New user registered: {Email}", request.Email);

        var tokens = await tokenService.GenerateTokensAsync(user);
        
        return Result<AuthResponse>.Success(tokens);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<AuthResponse>.Failure(new Error("UNAUTHTORIZED", "User not found"));
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            await userManager.AccessFailedAsync(user);
            
            logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
            return Result<AuthResponse>.Failure(new Error("UNAUTHTORIZED", "Password is incorrect"));
        }

        await userManager.ResetAccessFailedCountAsync(user);

        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);
        
        logger.LogInformation("User logged in: {Email}", request.Email);

        var authResponse = await tokenService.GenerateTokensAsync(user);

        return Result<AuthResponse>.Success(authResponse);
    }
}