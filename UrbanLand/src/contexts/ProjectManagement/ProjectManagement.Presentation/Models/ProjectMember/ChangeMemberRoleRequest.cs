using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.ProjectMember;

/// <summary>
/// Request model for changing a member's role in the project.
/// </summary>
public sealed record ChangeMemberRoleRequest
{
    /// <summary>
    /// New role to assign (Manager, Editor, Visitor).
    /// </summary>
    /// <example>Editor</example>
    [Required]
    [RegularExpression(@"^(Manager|Editor|Visitor)$")]
    public string NewRole { get; init; } = string.Empty;
}