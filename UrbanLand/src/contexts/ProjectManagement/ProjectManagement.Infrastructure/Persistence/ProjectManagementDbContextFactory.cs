using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectManagement.Infrastructure.Persistence;

public class ProjectManagementDbContextFactory : IDesignTimeDbContextFactory<ProjectManagementDbContext>
{
    public ProjectManagementDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectManagementDbContext>();

        var connectionString = args.FirstOrDefault()
            ?? "Host=localhost;Port=5433;Database=postgres;Username=admin;Password=20admin26";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(ProjectManagementDbContext).Assembly.FullName);
            npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "project_management");
        });
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new ProjectManagementDbContext(optionsBuilder.Options);
    }
}
