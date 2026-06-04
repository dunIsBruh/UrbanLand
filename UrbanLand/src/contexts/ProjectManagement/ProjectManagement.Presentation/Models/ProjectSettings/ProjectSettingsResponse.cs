namespace ProjectManagement.Presentation.Models.ProjectSettings;

/// <summary>
/// Project configuration settings.
/// </summary>
public sealed record ProjectSettingsResponse
{
    /// <summary>
    /// Default grid size for the project editor.
    /// </summary>
    /// <example>1.0</example>
    public double DefaultGridSize { get; init; }

    /// <summary>
    /// Whether the grid is shown by default in the editor.
    /// </summary>
    public bool ShowGrid { get; init; }

    /// <summary>
    /// Default terrain type for the project.
    /// </summary>
    /// <example>Flat</example>
    public string DefaultTerrainType { get; init; } = string.Empty;

    /// <summary>
    /// Maximum number of objects allowed in the project.
    /// </summary>
    /// <example>10000</example>
    public int MaxObjectsLimit { get; init; }

    /// <summary>
    /// Validity period of invitations in hours.
    /// </summary>
    /// <example>24</example>
    public int InvitationValidityHours { get; init; }
}