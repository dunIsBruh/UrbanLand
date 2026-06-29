using AssetCatalog.Application;
using AssetCatalog.Infrastructure;
using AssetCatalog.Presentation;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;
using ProjectManagement.Presentation;
using SceneDesign.Application;
using SceneDesign.Infrastructure;
using SceneDesign.Presentation;

namespace UrbanLand.Web.Extensions;

public static class BoundedContextRegistration
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSceneDesignModule(IConfiguration configuration)
        {
            services.AddSceneDesignApplication()
                .AddSceneDesignInfrastructure(configuration)
                .AddSceneDesignPresentation();
        
            return services;
        }

        public IServiceCollection AddProjectManagementModule(IConfiguration configuration)
        {
            services.AddProjectManagementApplication()
                .AddProjectManagementInfrastructure(configuration)
                .AddProjectManagementPresentation();
        
            return services;
        }

        public IServiceCollection AddAssetCatalogModule(IConfiguration configuration)
        {
            services.AddAssetCatalogApplication()
                .AddAssetCatalogInfrastructure(configuration)
                .AddAssetCatalogPresentation();
        
            return services;
        }
    }
}