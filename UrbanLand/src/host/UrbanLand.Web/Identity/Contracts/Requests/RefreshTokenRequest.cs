namespace UrbanLand.Web.Identity.Contracts.Requests;

public sealed record RefreshTokenRequest
{
    public string AccessToken { get; init; } = string.Empty;
}