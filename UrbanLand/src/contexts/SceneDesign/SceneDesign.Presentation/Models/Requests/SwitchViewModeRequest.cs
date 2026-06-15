using System.ComponentModel.DataAnnotations;

namespace SceneDesign.Presentation.Models.Requests;

/// <summary>
/// Request model for switching the scene view mode.
/// </summary>
public sealed record SwitchViewModeRequest
{
    /// <summary>
    /// View mode to switch to (2D or 3D).
    /// </summary>
    /// <example>2D</example>
    [Required]
    public string ViewMode { get; init; } = string.Empty;
}
