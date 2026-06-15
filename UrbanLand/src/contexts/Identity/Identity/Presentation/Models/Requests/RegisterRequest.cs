using System.ComponentModel.DataAnnotations;

namespace Identity.Presentation.Models.Requests;

/// <summary>
/// Request model for user registration.
/// </summary>
public sealed record RegisterRequest
{
    /// <summary>
    /// Email address for the new account.
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Password for the new account (min 8 characters).
    /// </summary>
    /// <example>MySecureP@ss1</example>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}