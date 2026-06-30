using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Persistence.Configurations;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<
    ApplicationUser,
    IdentityRole<Guid>,
    Guid>
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");
        
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}