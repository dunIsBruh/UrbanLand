using Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Infrastructure.Persistence.Configurations;

public class SceneConfiguration : IEntityTypeConfiguration<Scene>
{
    public void Configure(EntityTypeBuilder<Scene> builder)
    {
        builder.ToTable("Scenes");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SceneId.From(value))
            .Metadata.SetValueComparer(new ValueComparer<SceneId>(
                (a, b) => 
                    a != null && b != null && a.Value == b.Value,
                v => v.Value.GetHashCode(),
                v => SceneId.From(v.Value)));

        builder.Property(s => s.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.From(value));

        builder.Property(s => s.CurrentViewMode)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(s => s.Version)
            .IsConcurrencyToken();
        
        builder.Property(s => s.CreatedAt);
        builder.Property(s => s.IsLocked);

        builder.OwnsOne(s => s.Settings, settings =>
        {
            settings.Property(ss => ss.MaxObjectLimit)
                .HasColumnName("Max_Object_Limit");
            settings.Property(ss => ss.DetectCollisions)
                .HasColumnName("Detect_Collisions");
        });

        builder.HasMany(s => s.Objects)
            .WithOne()
            .HasForeignKey("SceneId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Layers)
            .WithOne()
            .HasForeignKey("SceneId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProjectId);
    }
}
