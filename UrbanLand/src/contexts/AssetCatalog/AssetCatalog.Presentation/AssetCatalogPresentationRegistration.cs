using AssetCatalog.Presentation.Endpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AssetCatalog.Presentation;

public static class AssetCatalogPresentationRegistration
{
    public static IServiceCollection AddAssetCatalogPresentation(
        this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapAssetCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAssetEndpoints();
        return app;
    }
}
