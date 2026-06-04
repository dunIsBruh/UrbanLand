namespace SharedKernel.Identity;

public record UserInfo(Guid UserId, string DisplayName, string Email, string? AvatarUrl = null);
