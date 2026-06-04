using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectId.From(value));
        
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
            
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
            
        // Owned entity для Value Object
        builder.OwnsOne(p => p.Settings, settings =>
        {
            settings.Property(s => s.IsPublic)
                .HasColumnName("Is_Public");
            settings.Property(s => s.AllowPublicComments)
                .HasColumnName("Allow_Public_Comments");
            settings.Property(s => s.DefaultAccessLevel)
                .HasColumnName("Default_Access_Level")
                .HasConversion<int>();
            settings.Property(s => s.InvitationValidityPeriod)
                .HasColumnName("Invitation_Validity_Period")
                .HasConversion(
                    ts => ts.Ticks,
                    value => TimeSpan.FromTicks(value));
        });
        
        builder.Property(p => p.OwnerId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value));
        
        // Связи
        builder.HasMany(p => p.Members)
            .WithOne()
            .HasForeignKey(m => m.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(p => p.Invitations)
            .WithOne()
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Status);
    }
}
