using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SceneDesign.Infrastructure.Persistence;

public class SceneDesignDbContextFactory : IDesignTimeDbContextFactory<SceneDesignDbContext>
{
    public SceneDesignDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SceneDesignDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("SCENEDESIGN_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=urbanland_scenedesign;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(
                typeof(SceneDesignDbContext).Assembly.FullName);
            npgsqlOptions.MigrationsHistoryTable(
                "__EFMigrationsHistory",
                "scene_design");
        });

        optionsBuilder.UseSnakeCaseNamingConvention();

        return new SceneDesignDbContext(optionsBuilder.Options, publishEndpoint: null!, currentUserService: null!);
    }
}
