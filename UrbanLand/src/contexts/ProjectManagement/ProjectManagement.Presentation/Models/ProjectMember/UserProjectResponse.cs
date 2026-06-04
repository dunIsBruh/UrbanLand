namespace ProjectManagement.Presentation.Models.ProjectMember;

/// <summary>
/// Summary information about a project for the current user.
/// </summary>
public sealed record UserProjectResponse
{
    /// <summary>
    /// Unique identifier of the project.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Name of the project.
    /// </summary>
    /// <example>Central Park</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Type of the project.
    /// </summary>
    /// <example>Park</example>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Current status of the project.
    /// </summary>
    /// <example>Active</example>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Role of the current user in this project.
    /// </summary>
    /// <example>Manager</example>
    public string UserRole { get; init; } = string.Empty;

    /// <summary>
    /// Number of members in the project.
    /// </summary>
    public int MemberCount { get; init; }

    /// <summary>
    /// Date and time when the project was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Date and time when the project was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; init; }
}