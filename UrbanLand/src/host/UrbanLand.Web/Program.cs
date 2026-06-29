using ProjectManagement.Presentation;
using SceneDesign.Presentation;
using AssetCatalog.Presentation;
using Core.Infrastructure.Caching;
using Identity;
using UrbanLand.Web.Extensions;
using Serilog;
using UrbanLand.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCustomSwagger();
builder.Services.AddRedisCaching(configuration);
builder.Services.AddCustomCors(configuration);
builder.Services.AddCustomMassTransit(configuration);

builder.Services.AddIdentityModule(configuration);
builder.Services.AddProjectManagementModule(configuration);
builder.Services.AddSceneDesignModule(configuration);
builder.Services.AddAssetCatalogModule(configuration);

var app = builder.Build();

await app.MigrateAsync();

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
