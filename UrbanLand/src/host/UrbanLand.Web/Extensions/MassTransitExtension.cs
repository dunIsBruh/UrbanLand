using Core.Infrastructure.Messaging;

namespace UrbanLand.Web.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
		var isMassTransitEnabled = configuration.GetValue<bool>("MassTransit:Enabled");
		
		Assembly[] assemblies = [
			typeof(ProjectManagement.Infrastructure.Integration.Consumers.SceneStatisticsConsumer).Assembly,
			typeof(SceneDesign.Infrastructure.Integration.Consumers.SceneCreatedDomainEventConsumer).Assembly,
			typeof(AssetCatalog.Infrastructure.Integration.Consumers.GetAssetInfoConsumer).Assembly
		];
		
		if (isMassTransitEnabled)
		{
			services.AddSharedMassTransit(configuration, assemblies);
		}	
		else
		{	
			services.AddMassTransit(busConf =>
			{
				busConf.AddConsumers(assemblies);
				
				busConf.UsingInMemory((context, cfg) =>
				{
					cfg.ConfigureEndpoints(context);
				});
			});
		}
		
        return services;
    }
}
