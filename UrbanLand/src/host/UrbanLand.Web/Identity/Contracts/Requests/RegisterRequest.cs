namespace UrbanLand.Web.Identity.Contracts.Requests;

public sealed record RegisterRequest
{
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}