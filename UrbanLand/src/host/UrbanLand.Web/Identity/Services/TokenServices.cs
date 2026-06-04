using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrbanLand.Web.Identity.Configurations;
using UrbanLand.Web.Identity.Contracts.Responses;
using UrbanLand.Web.Identity.Data;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

namespace UrbanLand.Web.Identity.Services;

public class TokenService(
    IOptions<JwtSettings> jwtSettings,
    IdentityDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    ILogger<TokenService> logger)
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> GenerateTokensAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        
        // Создаем claims
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.DisplayName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.DisplayName),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = GenerateAccessToken(claims);
        var jwtId = claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        var refreshToken = await GenerateRefreshTokenAsync(user.Id, jwtId);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            User = MapToUserResponse(user, roles.FirstOrDefault() ?? "User")
        };
    }

    public async Task<AuthResponse?> RefreshTokensAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            logger.LogWarning("Invalid access token during refresh");
            return null;
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var jwtId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(jwtId))
        {
            return null;
        }

        var storedRefreshToken = await dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedRefreshToken is not { IsValid: true } || storedRefreshToken.JwtId != jwtId)
        {
            logger.LogWarning("Invalid or expired refresh token");
            
            if (storedRefreshToken != null)
            {
                await RevokeAllUserRefreshTokensAsync(Guid.Parse(userId));
            }
            
            return null;
        }

        storedRefreshToken.IsUsed = true;
        storedRefreshToken.UsedAt = DateTime.UtcNow;
        dbContext.RefreshTokens.Update(storedRefreshToken);
        await dbContext.SaveChangesAsync();

        return await GenerateTokensAsync(storedRefreshToken.User);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var storedToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken is { IsValid: true })
        {
            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            dbContext.RefreshTokens.Update(storedToken);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task RevokeAllUserRefreshTokensAsync(Guid userId)
    {
        var tokens = await dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.IsValid)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }

        dbContext.RefreshTokens.UpdateRange(tokens);
        await dbContext.SaveChangesAsync();
        
        logger.LogWarning("All refresh tokens revoked for user {UserId}", userId);
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string jwtId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = GenerateSecureToken(),
            JwtId = jwtId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            IsUsed = false,
            IsRevoked = false
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();

        return refreshToken;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256, 
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static UserResponse MapToUserResponse(ApplicationUser user, string role)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = role,
            CreatedAt = user.CreatedAt
        };
    }
}