using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.Invitation;

/// <summary>
/// Request model for creating a project invitation.
/// </summary>
public sealed record CreateInvitationRequest
{
    /// <summary>
    /// Suggested role for the invited user (Manager, Editor, Visitor).
    /// </summary>
    /// <example>Editor</example>
    [Required]
    [RegularExpression(@"^(Manager|Editor|Visitor)$")]
    public string SuggestedRole { get; init; } = "Visitor";
}