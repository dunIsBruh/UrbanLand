using Identity.Infrastructure;

namespace Identity.Application.Entities;
 
public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public bool IsValid => !IsUsed && !IsRevoked && DateTime.UtcNow < ExpiresAt;
}
