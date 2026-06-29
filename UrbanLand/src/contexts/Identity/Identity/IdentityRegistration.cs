using System.Text;
using Core.Identity;
using Identity.Application.Services;
using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Options;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.Services.Tokens;
using Identity.Presentation.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity;

public static class IdentityRegistration
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings not found");

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<IdentityConfiguration>(configuration.GetSection(IdentityConfiguration.SectionName));
        
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
        
        services.AddAuthorizationBuilder()
                    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
                    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        
        services.AddDbContext<IdentityDbContext>(options =>
        {
            // var connectionString = configuration.GetConnectionString("Identity");
            var connectionString = configuration["DatabaseConnection"];
            
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", 
                        "identity");
                });
            
            options.UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<TokenService>();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        services.AddScoped<IUserService, IdentityUserService>();
        services.AddScoped<IAdminService, IdentityAdminService>();
        services.AddScoped<IAuthService, AuthIdentityService>();

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapProfileEndpoints();
        app.MapAdminEndpoints();
        return app;
    }
}
