using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Persistence;
using ProjectManagement.Presentation;
using SceneDesign.Infrastructure.Persistence;
using SceneDesign.Presentation;
using AssetCatalog.Infrastructure.Persistence;
using AssetCatalog.Presentation;
using Identity;
using Identity.Infrastructure.Persistence;
using SharedKernel.Infrastructure.Caching;
using UrbanLand.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCustomSwagger();
builder.Services.AddRedisCaching(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddCustomMassTransit(builder.Configuration);

builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddProjectManagementModule(builder.Configuration);
builder.Services.AddSceneDesignModule(builder.Configuration);
builder.Services.AddAssetCatalogModule(builder.Configuration);

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
