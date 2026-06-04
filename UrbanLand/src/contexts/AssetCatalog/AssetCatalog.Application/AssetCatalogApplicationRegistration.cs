using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AssetCatalog.Application;

public static class AssetCatalogApplicationRegistration
{
    public static IServiceCollection AddAssetCatalogApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(
            typeof(AssetCatalogApplicationRegistration).Assembly,
            includeInternalTypes: true);

        return services;
    }
}
