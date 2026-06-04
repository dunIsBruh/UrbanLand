namespace ProjectManagement.Presentation.Models.Project;

/// <summary>
/// Response returned after a project is created.
/// </summary>
public sealed record CreateProjectResponse
{
    /// <summary>
    /// Unique identifier of the created project.
    /// </summary>
    public Guid ProjectId { get; init; }

    /// <summary>
    /// Name of the created project.
    /// </summary>
    /// <example>Central Park</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Type of the created project.
    /// </summary>
    /// <example>Park</example>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the project was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}