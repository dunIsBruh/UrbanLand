using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.Services;
using SceneDesign.Infrastructure.Integration.Consumers;
using SceneDesign.Infrastructure.Persistence;
using SceneDesign.Infrastructure.Persistence.Repositories;
using SceneDesign.Infrastructure.Services;

namespace SceneDesign.Infrastructure;

public static class SceneDesignInfrastructureRegistration
{
    public static IServiceCollection AddSceneDesignInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SceneDesignDbContext>((_, options) =>
        {
            // var connectionString = configuration.GetConnectionString("SceneDesign");
            var connectionString = configuration["DatabaseConnection"];

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(SceneDesignDbContext).Assembly.FullName);
                npgsqlOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    "scene_design");
            });

            options.UseSnakeCaseNamingConvention();
            options.EnableSensitiveDataLogging(
                configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"));
        });

        services.AddScoped<ISceneRepository, SceneRepository>();
        services.AddScoped<ISceneValidationService, SceneValidationService>();
        services.AddScoped<IAssetProvider, AssetProviderService>();

        return services;
    }

    public static void AddSceneDesignConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(SceneCreatedDomainEventConsumer).Assembly);
    }
}
