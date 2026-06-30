namespace ProjectManagement.Presentation.Models.Project;

/// <summary>
/// Request model for creating a new project.
/// </summary>
public sealed record CreateProjectRequest
{
    /// <summary>
    /// Name of the project.
    /// </summary>
    /// <example>Central Park</example>
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Optional description of the project.
    /// </summary>
    /// <example>A modern urban park with recreational zones</example>
    [StringLength(2000)]
    public string? Description { get; init; }

    /// <summary>
    /// Type of the project (Park, ResidentialComplex, CottageVillage, UrbanSquare, IndustrialZone).
    /// </summary>
    /// <example>Park</example>
    [Required]
    [RegularExpression(@"^(Park|ResidentialComplex|CottageVillage|UrbanSquare|IndustrialZone)$")]
    public string Type { get; init; } = string.Empty;
}