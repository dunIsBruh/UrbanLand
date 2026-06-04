using SharedKernel.Identity;

namespace ProjectManagement.Domain.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(UserId userId);
    Task<UserInfo?> GetUserInfoAsync(UserId userId);
    Task<Dictionary<UserId, UserInfo>> GetUsersInfoAsync(IEnumerable<UserId> userIds);
}