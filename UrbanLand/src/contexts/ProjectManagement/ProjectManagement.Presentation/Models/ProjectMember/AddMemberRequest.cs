using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.ProjectMember;

public sealed record AddMemberRequest
{
    [Required]
    public Guid UserId { get; init; }

    [Required]
    [RegularExpression("^(Manager|Builder|Commentator|Visitor)$")]
    public string Role { get; init; } = "Visitor";
}