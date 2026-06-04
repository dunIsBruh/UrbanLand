using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Persistence.Configurations;
using ProjectManagement.Domain.Entities;
using SharedKernel.Abstractions;
using SharedKernel.Identity;

namespace ProjectManagement.Infrastructure.Persistence;

public class ProjectManagementDbContext(
    DbContextOptions<ProjectManagementDbContext> options,
    ICurrentUserService? currentUserService = null)
    : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> Members => Set<ProjectMember>();
    public DbSet<ProjectInvitation> Invitations => Set<ProjectInvitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("project_management");

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectInvitationConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        
        var result = await base.SaveChangesAsync(cancellationToken);
        
        return result;
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        var now = DateTime.UtcNow;
        var userId = currentUserService?.UserId ?? Guid.Empty;

        foreach (var entry in entries)
        {
            if (entry.Entity is Project project)
            {
                if (entry.State == EntityState.Added)
                {
                    // CreatedAt уже установлен в домене
                }
            }
        }
    }
}