using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.ProjectMember;

public sealed record ChangeMemberRoleRequest
{
    [Required]
    [RegularExpression(@"^(Manager|Editor|Visitor)$")]
    public string NewRole { get; init; } = string.Empty;
}