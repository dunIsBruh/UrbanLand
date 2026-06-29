using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Identity;

namespace AssetCatalog.Infrastructure.Persistence.Configurations;

public class AssetTemplateConfiguration : IEntityTypeConfiguration<AssetTemplate>
{
    public void Configure(EntityTypeBuilder<AssetTemplate> builder)
    {
        builder.ToTable("AssetTemplates");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AssetId.From(value));

        builder.Property(a => a.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.Category)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.IsCustom);

        builder.Property(a => a.UploaderId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value));

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.CreatedAt);

        builder.OwnsOne(a => a.TopographicSymbol, symbol =>
        {
            symbol.Property(s => s.SymbolType)
                .HasColumnName("Topographic_Symbol_Type")
                .HasMaxLength(50);
            symbol.Property(s => s.Color)
                .HasColumnName("Topographic_Symbol_Color")
                .HasMaxLength(20);
            symbol.Property(s => s.Size)
                .HasColumnName("Topographic_Symbol_Size");
        });

        builder.HasMany(a => a.Versions)
            .WithOne()
            .HasForeignKey("AssetTemplateId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.Name);
        builder.HasIndex(a => a.Category);
    }
}
