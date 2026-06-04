using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models.Project;

public sealed record CreateProjectRequest
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; init; }

    [Required]
    [RegularExpression(@"^(Park|ResidentialComplex|CottageVillage|UrbanSquare|IndustrialZone)$")]
    public string Type { get; init; } = string.Empty;
}