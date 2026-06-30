namespace Identity.Presentation.Models.Requests;

public sealed record SetRoleRequest
{
    [Required]
    [RegularExpression("^(User|Admin)$")]
    public string Role { get; init; } = string.Empty;
}