using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Caching;

public static class RedisConfigurationExtensions
{
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConfig = configuration
                              .GetSection(RedisConfiguration.SectionName)
                              .Get<RedisConfiguration>() 
                          ?? new RedisConfiguration();

        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = redisConfig.ConnectionString;
        //     options.InstanceName = redisConfig.InstanceName;
        // });

        return services;
    }
}