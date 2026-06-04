namespace UrbanLand.Web.Identity.Contracts.Requests;

public sealed record UpdateProfileRequest
{
    public string? DisplayName { get; init; }
    public string? AvatarUrl { get; init; }
}