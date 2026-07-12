using System.Text.Json.Serialization;

namespace Identity.Presentation.Models.Responses;

/// <summary>
/// Response returned after successful authentication.
/// </summary>
public sealed record AuthResponse
{
    /// <summary>
    /// JWT access token for API authorization.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIs...</example>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// Refresh token used to obtain a new access token.
    /// </summary>
    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>
    /// Expiration date and time of the refresh token.
    /// </summary>
    [JsonIgnore]
    public DateTime RefreshTokenExpiresAt { get; init; }

    /// <summary>
    /// Expiration date and time of the access token.
    /// </summary>
    public DateTime ExpiresAt { get; init; }

    /// <summary>
    /// Type of the token (always "Bearer").
    /// </summary>
    /// <example>Bearer</example>
    public string TokenType { get; init; } = "Bearer";

    /// <summary>
    /// Authenticated user information.
    /// </summary>
    public UserResponse User { get; init; } = null!;
}