using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetCatalog.Infrastructure.Persistence;

public class AssetCatalogDbContextFactory : IDesignTimeDbContextFactory<AssetCatalogDbContext>
{
    public AssetCatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AssetCatalogDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ASSETCATALOG_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=urbanland_assetcatalog;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(
                typeof(AssetCatalogDbContext).Assembly.FullName);
            npgsqlOptions.MigrationsHistoryTable(
                "__EFMigrationsHistory",
                "asset_catalog");
        });

        optionsBuilder.UseSnakeCaseNamingConvention();

        return new AssetCatalogDbContext(optionsBuilder.Options);
    }
}
