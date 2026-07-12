namespace ProjectManagement.Presentation.Models.ProjectMember;

/// <summary>
/// Request model for adding a member to a project.
/// </summary>
public sealed record AddMemberRequest
{
    /// <summary>
    /// Identifier of the user to add as a member.
    /// </summary>
    [Required]
    public Guid UserId { get; init; }

    /// <summary>
    /// Role to assign (Manager, Builder, Commentator, Visitor).
    /// </summary>
    /// <example>Builder</example>
    [Required]
    [RegularExpression("^(Manager|Builder|Commentator|Visitor)$")]
    public string Role { get; init; } = "Visitor";
}