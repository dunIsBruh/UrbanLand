using AssetCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetCatalog.Infrastructure.Persistence.Configurations;

public class AssetVersionConfiguration : IEntityTypeConfiguration<AssetVersion>
{
    public void Configure(EntityTypeBuilder<AssetVersion> builder)
    {
        builder.ToTable("AssetVersions");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.VersionNumber);
        builder.Property(v => v.CreatedAt);

        builder.OwnsOne(v => v.Model, model =>
        {
            model.Property(m => m.FileUrl)
                .HasColumnName("Model_File_Url")
                .HasMaxLength(500)
                .IsRequired();

            model.Property(m => m.FileName)
                .HasColumnName("Model_File_Name")
                .HasMaxLength(200)
                .IsRequired();

            model.Property(m => m.FileSize)
                .HasColumnName("Model_File_Size");

            model.Property(m => m.Format)
                .HasColumnName("Model_Format")
                .HasMaxLength(20);

            model.Property(m => m.PolygonCount)
                .HasColumnName("Model_Polygon_Count");

            model.OwnsOne(m => m.Dimensions, dim =>
            {
                dim.Property(d => d.Width).HasColumnName("Model_Width");
                dim.Property(d => d.Height).HasColumnName("Model_Height");
                dim.Property(d => d.Depth).HasColumnName("Model_Depth");
            });
        });
    }
}
