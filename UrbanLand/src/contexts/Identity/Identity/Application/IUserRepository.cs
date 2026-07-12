using Identity.Infrastructure.Entities;

namespace Identity.Application;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(ApplicationUser user, string password, CancellationToken ct = default);
    Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task UpdateUserAsync(ApplicationUser user, CancellationToken ct = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default);
}