using Core.Identity;

namespace Identity.Infrastructure.Services;

public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Guid UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return userIdClaim != null && Guid.TryParse(userIdClaim, out var userId) 
                ? userId 
                : Guid.Empty;
        }
    }

    public string Email => httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    public string DisplayName => httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    
    public string Role => httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    
    public bool IsAdmin => Role == "Admin";
}