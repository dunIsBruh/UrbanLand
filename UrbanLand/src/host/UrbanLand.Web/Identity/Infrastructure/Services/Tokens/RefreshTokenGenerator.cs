using System.Security.Cryptography;
using UrbanLand.Web.Identity.Application.Entities;
using UrbanLand.Web.Identity.Infrastructure.Options;

namespace UrbanLand.Web.Identity.Infrastructure.Services.Tokens;

public static class RefreshTokenGenerator
{
    public static RefreshToken Generate(Guid userId, string jwtId, JwtSettings jwtSettings)
    {
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = GenerateSecureToken(),
            JwtId = jwtId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays),
            IsUsed = false,
            IsRevoked = false,
        };
        
        return token;
    }

    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}