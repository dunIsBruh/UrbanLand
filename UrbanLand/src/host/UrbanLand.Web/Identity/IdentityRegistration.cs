using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Identity;
using UrbanLand.Web.Identity.Configurations;
using UrbanLand.Web.Identity.Data;
using UrbanLand.Web.Identity.Endpoints;
using UrbanLand.Web.Identity.Services;

namespace UrbanLand.Web.Identity;

public static class IdentityRegistration
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings not found");

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<IdentityConfiguration>(
            configuration.GetSection(IdentityConfiguration.SectionName));
        
        var identityConfig = configuration.GetSection(IdentityConfiguration.SectionName)
            .Get<IdentityConfiguration>();

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = identityConfig?.PasswordMinLength ?? 8;
            options.Password.RequireDigit = identityConfig?.PasswordRequireDigit ?? true;
            options.Password.RequireNonAlphanumeric = identityConfig?.PasswordRequireNonAlphanumeric ?? false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            options.SignIn.RequireConfirmedEmail = identityConfig?.RequireConfirmedEmail ?? false;

            options.Lockout.MaxFailedAccessAttempts = identityConfig?.MaxFailedLoginAttempts ?? 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && 
                        path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
        
        services.AddAuthorizationBuilder()
                    .AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"))
                    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Identity"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", 
                        "identity");
                });
            
            options.UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        // Регистрируем UserService для ВСЕХ доменных интерфейсов
        services.AddScoped<UserService>();
        services.AddScoped<ProjectManagement.Domain.Services.IUserService>(
            sp => sp.GetRequiredService<UserService>());
        services.AddScoped<SceneDesign.Domain.Services.IUserService>(
            sp => sp.GetRequiredService<UserService>());
        services.AddScoped<AssetCatalog.Domain.Services.IUserService>(
            sp => sp.GetRequiredService<UserService>());

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        return app;
    }
}