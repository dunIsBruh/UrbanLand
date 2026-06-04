using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SceneDesign.Application;

public static class SceneDesignApplicationRegistration
{
    public static IServiceCollection AddSceneDesignApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(
            typeof(SceneDesignApplicationRegistration).Assembly,
            includeInternalTypes: true);

        return services;
    }
}
