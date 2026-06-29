using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Messaging;

public static class CoreInfrastructureRegistration
{
    // public static IServiceCollection AddCoreInfrastructure(
    //     this IServiceCollection services,
    //     IConfiguration configuration)
    // {
    //     services.AddDbContext<BaseDbContext>();
    // }
    
    public static IServiceCollection AddSharedMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] consumerAssemblies)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqConfiguration>() 
                             ?? throw new InvalidOperationException("Cookies settings are not configured.");

        services.AddMassTransit(x =>
        {
            // Регистрируем всех consumers из переданных сборок
            foreach (var assembly in consumerAssemblies)
            {
                x.AddConsumers(assembly);
            }

            // Transactional Outbox (пока не работает)
            // x.AddEntityFrameworkOutbox<BaseDbContext>(o =>
            // {
            //     o.QueryDelay = TimeSpan.FromSeconds(1);
            //     o.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);
            //     
            //     if (rabbitMqConfig.UsePostgres)
            //     {
            //         o.UsePostgres();
            //     }
            //     else
            //     {
            //         o.UseSqlServer();
            //     }
            // });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig.Host, rabbitMqConfig.Port, "/", h =>
                {
                    h.Username(rabbitMqConfig.Username);
                    h.Password(rabbitMqConfig.Password);
                });

                // Настройки повторных попыток
                cfg.UseMessageRetry(r => 
                    r.Exponential(
                        retryLimit: 5,
                        minInterval: TimeSpan.FromSeconds(1),
                        maxInterval: TimeSpan.FromMinutes(5),
                        intervalDelta: TimeSpan.FromSeconds(2)));

                // Circuit Breaker
                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.TripThreshold = 15;
                    cb.ActiveThreshold = 10;
                    cb.ResetInterval = TimeSpan.FromMinutes(5);
                });

                // Rate Limiting
                cfg.UseRateLimit(100, TimeSpan.FromSeconds(1));

                // Корреляция
                cfg.ConfigureEndpoints(context);
                
                // Добавляем CorrelationId в заголовки
                // cfg.ConfigureSend(sendConfigurator =>
                // {
                //     sendConfigurator.UseCorrelationId(context => CorrelationContext.CorrelationId);
                // });
            });
        });

        // Health Checks для RabbitMQ
        // services.AddHealthChecks()
        //     .AddRabbitMQ(
        //         $"amqp://{rabbitMqConfig.Username}:{rabbitMqConfig.Password}@{rabbitMqConfig.Host}:{rabbitMqConfig.Port}",
        //         name: "rabbitmq",
        //         tags: new[] { "messaging" });
        
        return services;
    }
}