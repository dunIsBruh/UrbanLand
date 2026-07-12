using Core.Primitives;
using Identity.Application.Services;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.Services.Tokens;
using Identity.Presentation.Models.Requests;
using Identity.Presentation.Models.Responses;

namespace Identity.Presentation.Endpoints;

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
            .Produces<AuthResponse>()
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Login via token")
            .WithDescription("Authenticate user and get tokens")
            .Produces<AuthResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();

        group.MapPost("/refresh", RefreshTokenAsync)
            .WithName("Refresh")
            .WithSummary("Refresh Token")
            .WithDescription("Refresh access token using refresh token")
            .Produces<AuthResponse>()
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("/logout", LogoutAsync)
            .WithName("Logout")
            .WithSummary("Logout")
            .WithDescription("Revoke refresh token from user, witch invoke this handler")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        group.MapPost("/logout-all", LogoutAllAsync)
            .WithName("LogoutAll")
            .WithSummary("Logout All refresh tokens")
            .WithDescription("Revoke all refresh tokens from user, witch invoke this handler")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        HttpContext httpContext,
        IAuthService authService)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
        {
            return TypedResults.BadRequest(new Error("VALIDATION_ERROR", "Invalid email address"));
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            return TypedResults.BadRequest(new Error(
                "VALIDATION_ERROR", 
                "Password must be at least 8 characters"
                )
            );
        }

        var result = await authService.RegisterAsync(request);
        if (!result.IsSuccess)
        {
            return TypedResults.BadRequest(result.Error);
        }

        var authResponse = result.Value;
        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);

        return TypedResults.Ok(authResponse);
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        HttpContext httpContext,
        IAuthService authService)
    {
        var result = await authService.LoginAsync(request);
        if (!result.IsSuccess)
        {
            return TypedResults.Unauthorized();
        }
        
        var authResponse = result.Value;
        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);

        return TypedResults.Ok(authResponse);
    }

    private static async Task<IResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        HttpContext httpContext,
        TokenService tokenService)
    {
        var refreshToken = httpContext.Request.GetRefreshTokenCookie();
        if (string.IsNullOrWhiteSpace(request.AccessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return TypedResults.BadRequest(new Error(
                "VALIDATION_ERROR", 
                "Access token and refresh token are required"
                ));
        }

        var authResponse = await tokenService.RefreshTokensAsync(request.AccessToken, refreshToken);
        if (authResponse == null)
        {
            httpContext.Response.ClearRefreshTokenCookie();
            return TypedResults.BadRequest(new Error(
                "INVALID_TOKEN", 
                "Invalid or expired refresh token"
                ));
        }

        httpContext.Response.SetRefreshTokenCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiresAt);
        return TypedResults.Ok(authResponse);
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
        CurrentUserAccessor userAccessor,
        TokenService tokenService)
    {
        var userId = userAccessor.UserId;
        if (userId.Equals(Guid.Empty))
        {
            return TypedResults.Unauthorized();
        }

        await tokenService.RevokeAllUserRefreshTokensAsync(userId);
        
        return TypedResults.Ok(new { Message = "Logged out from all devices" });
    }
}
