using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Infrastructure.Persistence.Configurations;

public class ObjectLayerConfiguration : IEntityTypeConfiguration<ObjectLayer>
{
    public void Configure(EntityTypeBuilder<ObjectLayer> builder)
    {
        builder.ToTable("ObjectLayers");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasConversion(
                id => id.Value,
                value => ObjectLayerId.From(value));
        
        builder.Property(l => l.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.Order);

        builder.Property(l => l.IsVisible);
        builder.Property(l => l.IsLocked);
    }
}
