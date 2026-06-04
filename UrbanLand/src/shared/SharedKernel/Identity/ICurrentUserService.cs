namespace SharedKernel.Identity;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    string DisplayName { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
}