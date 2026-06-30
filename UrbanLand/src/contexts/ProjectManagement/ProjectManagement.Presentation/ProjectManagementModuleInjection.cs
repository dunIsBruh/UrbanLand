using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Presentation;

public static class ProjectManagementModuleInjection
{
    public static IServiceCollection AddProjectManagementModule(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddProjectManagementApplication()
            .AddProjectManagementInfrastructure(configuration)
            .AddProjectManagementPresentation();
        
        return services;
    }
}