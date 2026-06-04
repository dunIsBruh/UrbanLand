using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.Invitation;

public sealed record CreateInvitationRequest
{
    [Required]
    [RegularExpression(@"^(Manager|Editor|Visitor)$")]
    public string SuggestedRole { get; init; } = "Visitor";
}