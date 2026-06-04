using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.ProjectSettings;

public sealed record UpdateSettingsRequest
{
    [Range(0.1, 100.0)]
    public double DefaultGridSize { get; init; } = 1.0;

    public bool ShowGrid { get; init; } = true;

    [StringLength(50)]
    public string DefaultTerrainType { get; init; } = "Flat";

    [Range(1, 100000)]
    public int MaxObjectsLimit { get; init; } = 10000;

    [Range(1, 168)]
    public int InvitationValidityHours { get; init; } = 24;
}