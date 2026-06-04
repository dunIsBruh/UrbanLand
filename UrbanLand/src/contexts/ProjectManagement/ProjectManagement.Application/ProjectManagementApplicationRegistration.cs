using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Application;

public static class ProjectManagementApplicationRegistration
{
    public static IServiceCollection AddProjectManagementApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddValidatorsFromAssembly(
            typeof(ProjectManagementApplicationRegistration).Assembly,
            includeInternalTypes: true);
        
        return services;
    }
}
