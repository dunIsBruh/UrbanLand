using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.ValueObjects;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Persistence.Configurations;

public class ProjectInvitationConfiguration : IEntityTypeConfiguration<ProjectInvitation>
{
    public void Configure(EntityTypeBuilder<ProjectInvitation> builder)
    {
        builder.ToTable("ProjectInvitations");
        
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectInvitationId.From(value));
            
        builder.Property(i => i.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.From(value))
            .IsRequired();
        
        builder.Property(i => i.InviteCode)
            .HasMaxLength(8)
            .IsRequired();
        
        builder.OwnsOne(i => i.SuggestedRole, role =>
        {
            role.Property(r => r.Level)
                .HasColumnName("Suggested_Role_Level")
                .HasConversion<int>()
                .IsRequired();
                
            role.Property(r => r.Name)
                .HasColumnName("Suggested_Role_Name")
                .HasMaxLength(50);
        });
            
        builder.Property(i => i.ExpiresAt)
            .IsRequired();
            
        builder.Property(i => i.IsUsed)
            .IsRequired();
            
        builder.HasIndex(i => i.InviteCode).IsUnique();
        
        builder.HasIndex(i => i.ExpiresAt);
        builder.HasIndex(i => i.IsUsed);
    }
}