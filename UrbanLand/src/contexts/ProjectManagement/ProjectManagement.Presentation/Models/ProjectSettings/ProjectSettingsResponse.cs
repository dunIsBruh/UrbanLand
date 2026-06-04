namespace ProjectManagement.Presentation.Models.ProjectSettings;

public sealed record ProjectSettingsResponse
{
    public double DefaultGridSize { get; init; }
    public bool ShowGrid { get; init; }
    public string DefaultTerrainType { get; init; } = string.Empty;
    public int MaxObjectsLimit { get; init; }
    public int InvitationValidityHours { get; init; }
}