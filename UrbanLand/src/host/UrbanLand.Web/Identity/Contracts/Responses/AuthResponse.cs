using System.Text.Json.Serialization;

namespace UrbanLand.Web.Identity.Contracts.Responses;

public sealed record AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;

    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonIgnore]
    public DateTime RefreshTokenExpiresAt { get; init; }

    public DateTime ExpiresAt { get; init; }
    public string TokenType { get; init; } = "Bearer";
    public UserResponse User { get; init; } = null!;
}