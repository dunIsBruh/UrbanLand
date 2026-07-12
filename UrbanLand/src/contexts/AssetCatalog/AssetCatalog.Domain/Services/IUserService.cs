using Core.Identity;

namespace AssetCatalog.Domain.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(UserId userId);
    Task<UserInfo?> GetUserInfoAsync(UserId userId);
}