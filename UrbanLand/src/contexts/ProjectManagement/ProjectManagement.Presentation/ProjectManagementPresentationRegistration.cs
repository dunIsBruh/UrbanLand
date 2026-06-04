using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Presentation.Endpoints;

namespace ProjectManagement.Presentation;

public static class ProjectManagementPresentationRegistration
{
    public static IServiceCollection AddProjectManagementPresentation(
        this IServiceCollection services)
    {
        // Здесь могут быть специфичные для Presentation сервисы
        // Например, Mapster конфигурации

        return services;
    }

    public static IEndpointRouteBuilder MapProjectManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProjectEndpoints();
        app.MapProjectMemberEndpoints();
        app.MapInvitationEndpoints();
        
        return app;
    }
}
