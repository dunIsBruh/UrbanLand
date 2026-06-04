using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UrbanLand.Web.Identity.Data;

public class IdentityDbContext : IdentityDbContext<
    ApplicationUser,
    IdentityRole<Guid>,
    Guid,
    IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
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


        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");

            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            entity.Property(e => e.LastLoginAt);
        });

        // RefreshToken
        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.JwtId)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.ExpiresAt)
                .IsRequired();

            entity.HasIndex(e => e.Token)
                .IsUnique();

            entity.HasIndex(e => e.JwtId);

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Identity Roles
        builder.Entity<IdentityRole<Guid>>(entity => { entity.ToTable("Roles"); });

        builder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });

        builder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });

        builder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });

        builder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });

        builder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });

        // Seed ролей вынесен в IdentityDataSeeder (запускается при старте приложения)
    }
}