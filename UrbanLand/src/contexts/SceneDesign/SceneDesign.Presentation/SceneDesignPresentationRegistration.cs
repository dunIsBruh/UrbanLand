using SceneDesign.Presentation.Endpoints;

namespace SceneDesign.Presentation;

public static class SceneDesignPresentationRegistration
{
    public static IServiceCollection AddSceneDesignPresentation(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapSceneDesignEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSceneEndpoints();
        return app;
    }
}
