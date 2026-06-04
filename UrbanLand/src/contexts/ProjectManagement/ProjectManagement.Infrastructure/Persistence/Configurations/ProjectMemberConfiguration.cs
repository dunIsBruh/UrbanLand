using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Persistence.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");
        
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectMemberId.From(value));
        
        builder.Property(m => m.MemberId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .IsRequired();
        
        builder.Property(m => m.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.From(value))
            .IsRequired();
        
        builder.Property(m => m.InvitedBy)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? UserId.From(value.Value) : null)
            .IsRequired(false);
        
        builder.Property(m => m.JoinedAt)
            .IsRequired();
            
        // // LeftAt (для soft delete)
        // builder.Property(m => m.LeftAt)
        //     .IsRequired(false);
        //     
        // // Status
        // builder.Property(m => m.Status)
        //     .HasConversion<string>()
        //     .HasMaxLength(20)
        //     .IsRequired();
        
        builder.OwnsOne(m => m.Role, role =>
        {
            role.Property(r => r.Level)
                .HasColumnName("Role_Level")
                .HasConversion<int>()
                .IsRequired();
                
            role.Property(r => r.Name)
                .HasColumnName("Role_Name")
                .HasMaxLength(50)
                .IsRequired();
            
            role.HasIndex(r => r.Level)
                .HasDatabaseName("IX_ProjectMembers_RoleLevel");
        });
        
        builder.HasIndex(m => new { m.ProjectId, m.MemberId }).IsUnique();
        
        builder.HasIndex(m => m.MemberId);
        builder.HasIndex(m => m.JoinedAt);
        
        // builder.HasQueryFilter(m => m.LeftAt == null);
    }
}