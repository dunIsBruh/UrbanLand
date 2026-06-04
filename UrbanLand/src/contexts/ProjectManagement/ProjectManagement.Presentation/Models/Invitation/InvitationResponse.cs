namespace ProjectManagement.Presentation.Models.Invitation;

/// <summary>
/// Information about a project invitation.
/// </summary>
public sealed record InvitationResponse
{
    /// <summary>
    /// Unique identifier of the invitation.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Invitation code that can be shared with users.
    /// </summary>
    /// <example>ABC123XYZ</example>
    public string InviteCode { get; init; } = string.Empty;

    /// <summary>
    /// Suggested role for the invited user.
    /// </summary>
    /// <example>Editor</example>
    public string SuggestedRole { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the invitation was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Date and time when the invitation expires.
    /// </summary>
    public DateTime ExpiresAt { get; init; }
}