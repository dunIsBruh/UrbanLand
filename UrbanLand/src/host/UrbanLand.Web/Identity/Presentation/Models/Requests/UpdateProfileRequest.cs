using System.ComponentModel.DataAnnotations;

namespace UrbanLand.Web.Identity.Presentation.Models.Requests;

/// <summary>
/// Request model for updating user profile information.
/// </summary>
public sealed record UpdateProfileRequest
{
    /// <summary>
    /// New display name for the user.
    /// </summary>
    /// <example>John Doe</example>
    [StringLength(100, MinimumLength = 1)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// URL to the new avatar image.
    /// </summary>
    /// <example>https://example.com/avatars/user.png</example>
    [Url]
    [StringLength(500)]
    public string? AvatarUrl { get; init; }
}