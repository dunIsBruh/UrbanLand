namespace ProjectManagement.Application.Commands.CreateInvitation;

public sealed record CreateInvitationResult(
    Guid Id,
    string InviteCode,
    string SuggestedRole,
    DateTime CreatedAt,
    DateTime ExpiresAt);
