namespace UrbanLand.Web.Identity.Presentation.Models.Responses;

/// <summary>
/// Public user profile information.
/// </summary>
public sealed record UserResponse
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Email address of the user.
    /// </summary>
    /// <example>user@example.com</example>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    /// <example>John Doe</example>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// URL to the user's avatar image.
    /// </summary>
    /// <example>https://example.com/avatars/user.png</example>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Role assigned to the user (User or Admin).
    /// </summary>
    /// <example>User</example>
    public string Role { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}