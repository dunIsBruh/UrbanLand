using ProjectManagement.Domain.Services;
using SharedKernel.Identity;

namespace ProjectManagement.Infrastructure.Adapters;

public class UserServiceAdapter(ICurrentUserService currentUserService) : IUserService
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    
    // В реальном проекте здесь был бы HttpClient или gRPC клиент
    // для связи с Identity сервисом
    private readonly Dictionary<Guid, UserInfo> _mockUsers = new()
    {
        { Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"), 
            new UserInfo(Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"), 
                "System Administrator", 
                "admin@urbanland.com") 
        },
        { Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"), 
            new UserInfo(Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
                "Regular User",
                "user@urbanland.com") 
        }
    };

    public async Task<bool> UserExistsAsync(UserId userId)
    {
        // TODO: Заменить на реальный запрос к Identity Service
        return await Task.FromResult(_mockUsers.ContainsKey(userId.Value));
    }

    public async Task<UserInfo?> GetUserInfoAsync(UserId userId)
    {
        // TODO: Заменить на реальный запрос
        return await Task.FromResult(
            _mockUsers.GetValueOrDefault(userId.Value));
    }

    public async Task<Dictionary<UserId, UserInfo>> GetUsersInfoAsync(IEnumerable<UserId> userIds)
    {
        // TODO: Заменить на batch запрос к Identity Service
        return await Task.FromResult(
            userIds
                .Where(id => _mockUsers.ContainsKey(id.Value))
                .ToDictionary(id => id, id => _mockUsers[id.Value]));
    }
}