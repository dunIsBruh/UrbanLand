using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.ProjectSettings;

/// <summary>
/// Request model for updating project settings.
/// </summary>
public sealed record UpdateSettingsRequest
{
    /// <summary>
    /// Default grid size for the project editor (0.1-100.0).
    /// </summary>
    /// <example>1.0</example>
    [Range(0.1, 100.0)]
    public double DefaultGridSize { get; init; } = 1.0;

    /// <summary>
    /// Whether the grid is shown by default in the editor.
    /// </summary>
    public bool ShowGrid { get; init; } = true;

    /// <summary>
    /// Default terrain type for the project.
    /// </summary>
    /// <example>Flat</example>
    [StringLength(50)]
    public string DefaultTerrainType { get; init; } = "Flat";

    /// <summary>
    /// Maximum number of objects allowed in the project (1-100000).
    /// </summary>
    /// <example>10000</example>
    [Range(1, 100000)]
    public int MaxObjectsLimit { get; init; } = 10000;

    /// <summary>
    /// Validity period of invitations in hours (1-168).
    /// </summary>
    /// <example>24</example>
    [Range(1, 168)]
    public int InvitationValidityHours { get; init; } = 24;
}