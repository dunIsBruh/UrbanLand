namespace ProjectManagement.Presentation.Models.Invitation;

public sealed record InvitationResponse
{
    public Guid Id { get; init; }
    public string InviteCode { get; init; } = string.Empty;
    public string SuggestedRole { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
}