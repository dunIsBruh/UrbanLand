using System.Reflection;
using AssetCatalog.Presentation;
using Microsoft.OpenApi;
using ProjectManagement.Presentation;
using SceneDesign.Presentation;

namespace UrbanLand.Web.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "UrbanLand API",
                Version = "v1"
            });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

            options.IncludeXmlComments(xmlPath);

            var presentationAssemblies = new[]
            {
                typeof(ProjectManagementPresentationRegistration).Assembly,
                typeof(SceneDesignPresentationRegistration).Assembly,
                typeof(AssetCatalogPresentationRegistration).Assembly
            };

            foreach (var asm in presentationAssemblies)
            {
                var path = Path.Combine(AppContext.BaseDirectory, $"{asm.GetName().Name}.xml");
                if (File.Exists(path))
                {
                    options.IncludeXmlComments(path);
                }
            }
    
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name =  "Authorization",
                Description = "Enter JWT Bearer token",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
            });
    
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        return services;
    }
}