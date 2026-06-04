using System.ComponentModel.DataAnnotations;

namespace UrbanLand.Web.Identity.Contracts.Requests;

/// <summary>
/// Request model for refreshing an access token.
/// </summary>
public sealed record RefreshTokenRequest
{
    /// <summary>
    /// Current (expired or about to expire) JWT access token.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIs...</example>
    [Required]
    public string AccessToken { get; init; } = string.Empty;
}