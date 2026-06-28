using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetCatalog.Infrastructure.Persistence;

public class AssetCatalogDbContextFactory : IDesignTimeDbContextFactory<AssetCatalogDbContext>
{
    public AssetCatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AssetCatalogDbContext>();
        
        var connectionString = args.FirstOrDefault()
            ?? "Host=localhost;Port=5433;Database=postgres;Username=admin;Password=20admin26";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(AssetCatalogDbContext).Assembly.FullName);
            npgsqlOptions.MigrationsHistoryTable(
                "__EFMigrationsHistory",
                "asset_catalog");
        });

        optionsBuilder.UseSnakeCaseNamingConvention();

        return new AssetCatalogDbContext(optionsBuilder.Options);
    }
}
