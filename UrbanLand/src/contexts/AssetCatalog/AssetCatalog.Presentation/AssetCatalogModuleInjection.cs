using AssetCatalog.Application;
using AssetCatalog.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetCatalog.Presentation;

public static class AssetCatalogModuleInjection
{
    public static IServiceCollection AddAssetCatalogModule(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAssetCatalogApplication()
            .AddAssetCatalogInfrastructure(configuration)
            .AddAssetCatalogPresentation();
        
        return services;
    }
}