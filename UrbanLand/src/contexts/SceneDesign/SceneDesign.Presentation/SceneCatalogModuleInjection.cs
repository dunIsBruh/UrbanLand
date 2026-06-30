using SceneDesign.Application;
using SceneDesign.Infrastructure;

namespace SceneDesign.Presentation;

public static class SceneDesignModuleInjection
{
    public static IServiceCollection AddSceneDesignModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSceneDesignApplication()
            .AddSceneDesignInfrastructure(configuration)
            .AddSceneDesignPresentation();
        
        return services;
    }
}