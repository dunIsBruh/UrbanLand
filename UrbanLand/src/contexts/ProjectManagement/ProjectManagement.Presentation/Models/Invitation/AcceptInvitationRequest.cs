using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.Invitation;

public sealed record AcceptInvitationRequest
{
    [Required]
    public string InviteCode { get; init; } = string.Empty;
}