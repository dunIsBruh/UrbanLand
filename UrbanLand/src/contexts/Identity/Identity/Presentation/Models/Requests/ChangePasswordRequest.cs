using System.ComponentModel.DataAnnotations;

namespace Identity.Presentation.Models.Requests;

/// <summary>
/// Request model for changing the user's password.
/// </summary>
public sealed record ChangePasswordRequest
{
    /// <summary>
    /// Current password for verification.
    /// </summary>
    /// <example>OldP@ss1</example>
    [Required]
    public string CurrentPassword { get; init; } = string.Empty;

    /// <summary>
    /// New password (min 8 characters).
    /// </summary>
    /// <example>NewSecureP@ss1</example>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string NewPassword { get; init; } = string.Empty;
}
