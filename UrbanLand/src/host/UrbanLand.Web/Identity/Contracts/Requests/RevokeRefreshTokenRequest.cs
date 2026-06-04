using System.ComponentModel.DataAnnotations;

namespace UrbanLand.Web.Identity.Contracts.Requests;

/// <summary>
/// Request model for revoking a specific refresh token.
/// </summary>
public sealed record RevokeRefreshTokenRequest
{
    /// <summary>
    /// Refresh token to revoke.
    /// </summary>
    /// <example>dGhpcyBpcyBhIHJlZnJl... (base64)</example>
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}