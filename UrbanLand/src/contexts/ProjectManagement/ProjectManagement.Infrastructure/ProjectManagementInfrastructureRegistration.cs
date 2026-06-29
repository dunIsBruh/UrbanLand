using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Adapters;
using ProjectManagement.Infrastructure.Integration.Consumers;
using ProjectManagement.Infrastructure.Persistence;
using ProjectManagement.Infrastructure.Persistence.Repositories;
using ProjectManagement.Infrastructure.Services;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.Services;

namespace ProjectManagement.Infrastructure;

public static class ProjectManagementInfrastructureRegistration
{
    public static IServiceCollection AddProjectManagementInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProjectManagementDbContext>((_, options) =>
        {
            // var connectionString = configuration.GetConnectionString("ProjectManagement");
            var connectionString = configuration["DatabaseConnection"];
            
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ProjectManagementDbContext).Assembly.FullName);
                npgsqlOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory", 
                    "project_management"
                    );
            });
            
            options.UseSnakeCaseNamingConvention();
            options.EnableSensitiveDataLogging(
                configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"));
        });
        
        services.AddScoped<IProjectRepository, ProjectRepository>();
        
        services.AddScoped<IProjectAccessService, ProjectAccessService>();
        services.AddScoped<IUserService, UserServiceAdapter>();

        return services;
    }
    
    // Регистрация MassTransit Consumer-ов (вызывается из Shared Infra)
    public static void AddProjectManagementConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<SceneStatisticsConsumer>();
    }
}