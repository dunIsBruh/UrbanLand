using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Infrastructure.Persistence.Configurations;

public class SceneObjectConfiguration : IEntityTypeConfiguration<SceneObject>
{
    public void Configure(EntityTypeBuilder<SceneObject> builder)
    {
        builder.ToTable("SceneObjects");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => SceneObjectId.From(value));

        builder.Property(o => o.AssetId)
            .HasConversion(
                id => id.Value,
                value => new AssetId(value));

        builder.Property(o => o.LayerId)
            .HasConversion(
                id => id.Value,
                value => new ObjectLayerId(value));

        builder.OwnsOne(o => o.Position, pos =>
        {
            pos.Property(p => p.X).HasColumnName("Position_X");
            pos.Property(p => p.Y).HasColumnName("Position_Y");
            pos.Property(p => p.Z).HasColumnName("Position_Z");
        });

        builder.OwnsOne(o => o.Rotation, rot =>
        {
            rot.Property(r => r.Yaw).HasColumnName("Rotation_Yaw");
            rot.Property(r => r.Pitch).HasColumnName("Rotation_Pitch");
            rot.Property(r => r.Roll).HasColumnName("Rotation_Roll");
        });

        builder.OwnsOne(o => o.Scale, scale =>
        {
            scale.Property(s => s.X).HasColumnName("Scale_X");
            scale.Property(s => s.Y).HasColumnName("Scale_Y");
            scale.Property(s => s.Z).HasColumnName("Scale_Z");
        });

        builder.OwnsOne(o => o.BoundingBox, bb =>
        {
            bb.OwnsOne(b => b.Min, min =>
            {
                min.Property(p => p.X).HasColumnName("BoundingBox_Min_X");
                min.Property(p => p.Y).HasColumnName("BoundingBox_Min_Y");
                min.Property(p => p.Z).HasColumnName("BoundingBox_Min_Z");
            });
            bb.OwnsOne(b => b.Max, max =>
            {
                max.Property(p => p.X).HasColumnName("BoundingBox_Max_X");
                max.Property(p => p.Y).HasColumnName("BoundingBox_Max_Y");
                max.Property(p => p.Z).HasColumnName("BoundingBox_Max_Z");
            });
        });
    }
}
