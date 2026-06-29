using AssetCatalog.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Persistence;
using SceneDesign.Infrastructure.Persistence;

namespace UrbanLand.Web;

public static class DatabaseMigrator
{
    public static async Task MigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        await identityDb.Database.MigrateAsync();
    
        var pmDb = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();
        await pmDb.Database.MigrateAsync();

        var sdDb = scope.ServiceProvider.GetRequiredService<SceneDesignDbContext>();
        await sdDb.Database.MigrateAsync();

        var acDb = scope.ServiceProvider.GetRequiredService<AssetCatalogDbContext>();
        await acDb.Database.MigrateAsync();

        await DbSeeder.SeedAllAsync(scope.ServiceProvider);
    }
}