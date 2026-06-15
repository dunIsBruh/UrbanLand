using Identity.Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
