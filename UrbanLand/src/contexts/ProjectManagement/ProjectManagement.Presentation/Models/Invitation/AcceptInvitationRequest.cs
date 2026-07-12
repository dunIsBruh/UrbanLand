namespace ProjectManagement.Presentation.Models.Invitation;

/// <summary>
/// Request model for accepting a project invitation.
/// </summary>
public sealed record AcceptInvitationRequest
{
    /// <summary>
    /// Invitation code received by the user.
    /// </summary>
    /// <example>ABC123XYZ</example>
    [Required]
    public string InviteCode { get; init; } = string.Empty;
}