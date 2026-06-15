using Microsoft.AspNetCore.Identity;
using UrbanLand.Web.Identity.Application;
using UrbanLand.Web.Identity.Application.Entities;
using UrbanLand.Web.Identity.Application.Services;

namespace UrbanLand.Web.Identity.Infrastructure.Persistence.Repositories;

public class IdentityUserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateUserAsync(ApplicationUser user, string password, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(ApplicationUser user, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}