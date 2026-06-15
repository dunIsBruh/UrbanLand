using Identity.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.JwtId)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ExpiresAt)
            .IsRequired();

        builder.HasIndex(e => e.Token)
            .IsUnique();

        builder.HasIndex(e => e.JwtId);

        builder.HasOne(e => e.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}