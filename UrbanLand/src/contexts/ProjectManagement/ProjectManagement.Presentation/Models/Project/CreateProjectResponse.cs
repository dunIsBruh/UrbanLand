namespace ProjectManagement.Presentation.Models.Project;

public sealed record CreateProjectResponse
{
    public Guid ProjectId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}