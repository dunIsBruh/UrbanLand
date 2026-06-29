using System.Reflection;
using MassTransit;
using SharedKernel.Infrastructure.Messaging;

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

// var mtEnabled = builder.Configuration.GetValue<bool>("MassTransit:Enabled");
// if (mtEnabled)
// {
// 	builder.Services.AddSharedMassTransit(
// 		builder.Configuration, 
// 		typeof(ProjectManagement.Infrastructure.Integration.Consumers.SceneStatisticsConsumer).Assembly,
// 		typeof(SceneCreatedDomainEventConsumer).Assembly,
// 		typeof(AssetCatalog.Infrastructure.Integration.Consumers.GetAssetInfoConsumer).Assembly
// 	);
// }
// else
// {
// 	builder.Services.AddMassTransit(x =>
// 	{
// 		var pmAssembly = typeof(ProjectManagement.Infrastructure.Integration.Consumers.SceneStatisticsConsumer).Assembly;
// 		var sdAssembly = typeof(SceneCreatedDomainEventConsumer).Assembly;
// 		var acAssembly = typeof(AssetCatalog.Infrastructure.Integration.Consumers.GetAssetInfoConsumer).Assembly;
// 		x.AddConsumers(pmAssembly);
// 		x.AddConsumers(sdAssembly);
// 		x.AddConsumers(acAssembly);
// 		x.UsingInMemory((context, cfg) =>
// 		{
// 			cfg.ConfigureEndpoints(context);
// 		});
// 	});
// }
