using Core.Infrastructure.Cors;

namespace UrbanLand.Web.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var corsConf = configuration.GetSection(CorsConfiguration.SectionName).Get<CorsConfiguration>();

            if (corsConf?.AllowedOrigins != null)
            {
                options.AddDefaultPolicy(policy => 
                    policy.WithOrigins(corsConf.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                    );
            }
        });
        
        return services;
    }
}