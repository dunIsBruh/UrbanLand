namespace ProjectManagement.Application.Queries.GetProjectDetails;

public sealed record ProjectDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    string Status,
    Guid OwnerId,
    string OwnerName,
    DateTime CreatedAt,
    int ObserverCount,
    string CurrentUserAccessLevel,
    ProjectDetailsSettingsDto Settings,
    List<ProjectDetailsAccessDto> AccessList);

public sealed record ProjectDetailsSettingsDto(
    double DefaultGridSize,
    bool ShowGrid,
    string DefaultTerrainType,
    int MaxObjectsLimit);

public sealed record ProjectDetailsAccessDto(
    Guid UserId,
    string UserName,
    string AccessLevel,
    DateTime GrantedAt);
