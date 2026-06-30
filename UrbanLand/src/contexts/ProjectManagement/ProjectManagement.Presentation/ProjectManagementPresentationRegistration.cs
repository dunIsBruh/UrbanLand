using ProjectManagement.Presentation.Endpoints;

namespace ProjectManagement.Presentation;

public static class ProjectManagementPresentationRegistration
{
    public static IServiceCollection AddProjectManagementPresentation(this IServiceCollection services)
    {
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
