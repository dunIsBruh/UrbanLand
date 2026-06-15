using Microsoft.AspNetCore.Identity;
using UrbanLand.Web.Identity.Application.Entities;

namespace UrbanLand.Web.Identity.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedIdentityAsync(
        IdentityDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
                Name = "User",
                NormalizedName = "USER"
            });
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }

        // Создание администратора, если нет пользователей
        if (!dbContext.Users.Any())
        {
            var adminUser = new ApplicationUser
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000100"),
                UserName = "admin@urbanland.com",
                Email = "admin@urbanland.com",
                DisplayName = "System Administrator",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = null
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "User");
            }

            var regularUser = new ApplicationUser
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000101"),
                UserName = "user@urbanland.com",
                Email = "user@urbanland.com",
                DisplayName = "Regular User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = null
            };

            createResult = await userManager.CreateAsync(regularUser, "User123!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }

    public static async Task SeedAllAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        // Identity seeding
        var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await SeedIdentityAsync(identityDb, userManager, roleManager);
    }
}
