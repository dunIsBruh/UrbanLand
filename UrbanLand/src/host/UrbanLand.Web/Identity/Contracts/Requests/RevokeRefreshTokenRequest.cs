namespace UrbanLand.Web.Identity.Contracts.Requests;

public sealed record RevokeRefreshTokenRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}