using AssetCatalog.Domain.Repositories;
using AssetCatalog.Infrastructure.Integration.Consumers;
using AssetCatalog.Infrastructure.Persistence;
using AssetCatalog.Infrastructure.Persistence.Repositories;
using AssetCatalog.Infrastructure.Services;
using AssetCatalog.Application.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetCatalog.Infrastructure;

public static class AssetCatalogInfrastructureRegistration
{
    public static IServiceCollection AddAssetCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AssetCatalogDbContext>((_, options) =>
        {
            var connectionString = configuration.GetConnectionString("AssetCatalog");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(
                    typeof(AssetCatalogDbContext).Assembly.FullName);
                npgsqlOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    "asset_catalog");
            });

            options.UseSnakeCaseNamingConvention();
            options.EnableSensitiveDataLogging(
                configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"));
        });

        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ISketchfabService, SketchfabService>();
        services.AddHttpClient<SketchfabService>();

        return services;
    }

    public static void AddAssetCatalogConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<GetAssetInfoConsumer>();
    }
}
