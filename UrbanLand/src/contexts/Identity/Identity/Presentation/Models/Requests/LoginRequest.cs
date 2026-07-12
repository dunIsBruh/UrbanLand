namespace Identity.Presentation.Models.Requests;

/// <summary>
/// Request model for user authentication.
/// </summary>
public sealed record LoginRequest
{
    /// <summary>
    /// Email address of the user.
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Password for authentication.
    /// </summary>
    /// <example>MySecureP@ss1</example>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}