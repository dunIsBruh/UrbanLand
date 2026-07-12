using Identity.Infrastructure.Entities;

namespace Identity.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(e => e.AvatarUrl)
            .HasMaxLength(500);
        
        builder.Property(e => e.LastLoginAt);
    }
}