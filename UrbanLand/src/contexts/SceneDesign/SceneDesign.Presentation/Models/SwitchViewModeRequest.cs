using System.ComponentModel.DataAnnotations;

namespace SceneDesign.Presentation.Models;

public sealed record SwitchViewModeRequest
{
    [Required]
    public string ViewMode { get; init; } = string.Empty;
}
