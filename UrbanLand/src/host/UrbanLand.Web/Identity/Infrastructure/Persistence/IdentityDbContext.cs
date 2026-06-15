using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanLand.Web.Identity.Application.Entities;
using UrbanLand.Web.Identity.Infrastructure.Persistence.Configurations;

namespace UrbanLand.Web.Identity.Infrastructure.Persistence;

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