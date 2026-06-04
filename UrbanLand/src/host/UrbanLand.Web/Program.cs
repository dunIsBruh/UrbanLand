using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Persistence;
using ProjectManagement.Presentation;
using SceneDesign.Application;
using SceneDesign.Infrastructure;
using SceneDesign.Infrastructure.Persistence;
using SceneDesign.Presentation;
using AssetCatalog.Application;
using AssetCatalog.Infrastructure;
using AssetCatalog.Infrastructure.Persistence;
using AssetCatalog.Presentation;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Cors;
using SharedKernel.Infrastructure.Messaging;
using UrbanLand.Web.Identity;
using UrbanLand.Web.Extensions;
using UrbanLand.Web.Identity.Data;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCustomSwagger();

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.AddCors(options =>
{
    var corsConfig = builder.Configuration.GetSection("Cors").Get<CorsConfiguration>();
    
    if (corsConfig?.AllowedOrigins != null)
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(corsConfig.AllowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    }
});

// identity subdomain
builder.Services.AddIdentityModule(builder.Configuration);

// project management
builder.Services
    .AddProjectManagementApplication()
    .AddProjectManagementInfrastructure(builder.Configuration)
    .AddProjectManagementPresentation();

// scene design  
builder.Services
    .AddSceneDesignApplication()
    .AddSceneDesignInfrastructure(builder.Configuration)
    .AddSceneDesignPresentation();

// asset catalog
builder.Services
    .AddAssetCatalogApplication()
    .AddAssetCatalogInfrastructure(builder.Configuration)
    .AddAssetCatalogPresentation();

    var mtEnabled = builder.Configuration.GetValue<bool>("MassTransit:Enabled");
    if (mtEnabled)
    {
        builder.Services.AddSharedMassTransit(
            builder.Configuration, 
            typeof(ProjectManagement.Infrastructure.Integration.Consumers.SceneStatisticsConsumer).Assembly,
            typeof(SceneDesign.Infrastructure.Integration.Consumers.DomainEvents.SceneCreatedDomainEventConsumer).Assembly,
            typeof(AssetCatalog.Infrastructure.Integration.Consumers.GetAssetInfoConsumer).Assembly
        );
    }
    else
    {
        builder.Services.AddMassTransit(x =>
        {
            var pmAssembly = typeof(ProjectManagement.Infrastructure.Integration.Consumers.SceneStatisticsConsumer).Assembly;
            var sdAssembly = typeof(SceneDesign.Infrastructure.Integration.Consumers.DomainEvents.SceneCreatedDomainEventConsumer).Assembly;
            var acAssembly = typeof(AssetCatalog.Infrastructure.Integration.Consumers.GetAssetInfoConsumer).Assembly;
            x.AddConsumers(pmAssembly);
            x.AddConsumers(sdAssembly);
            x.AddConsumers(acAssembly);
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await identityDb.Database.MigrateAsync();
    
    var pmDb = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();
    await pmDb.Database.MigrateAsync();

    var sdDb = scope.ServiceProvider.GetRequiredService<SceneDesignDbContext>();
    await sdDb.Database.MigrateAsync();

    var acDb = scope.ServiceProvider.GetRequiredService<AssetCatalogDbContext>();
    await acDb.Database.MigrateAsync();

    await DbSeeder.SeedAllAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityEndpoints();
app.MapProjectManagementEndpoints();
app.MapSceneDesignEndpoints();
app.MapAssetCatalogEndpoints();

try
{
    Log.Information("Application starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
