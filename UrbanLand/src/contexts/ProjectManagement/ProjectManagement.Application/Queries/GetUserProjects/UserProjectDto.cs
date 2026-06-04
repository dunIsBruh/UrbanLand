namespace ProjectManagement.Application.Queries.GetUserProjects;

public sealed record UserProjectDto(
    Guid Id,
    string Name,
    string Type,
    string Status,
    string AccessLevel,
    int ObserverCount,
    DateTime CreatedAt,
    DateTime? LastModifiedAt);
