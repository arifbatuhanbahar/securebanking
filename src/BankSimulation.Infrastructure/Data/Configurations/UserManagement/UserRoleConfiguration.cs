using BankSimulation.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.UserManagement;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        
        builder.HasKey(ur => ur.RoleId);
        builder.Property(ur => ur.RoleId)
            .HasColumnName("role_id");
        
        builder.Property(ur => ur.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(ur => ur.RoleName)
            .HasColumnName("role_name")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(ur => ur.AssignedAt)
            .HasColumnName("assigned_at")
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(ur => ur.AssignedBy)
            .HasColumnName("assigned_by");
        
        builder.Property(ur => ur.ExpiresAt)
            .HasColumnName("expires_at");
        
        // User -> UserRoles ilişkisi (NO ACTION)
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // AssignedBy -> User ilişkisi (NO ACTION)
        builder.HasOne(ur => ur.AssignedByUser)
            .WithMany(u => u.AssignedRoles)
            .HasForeignKey(ur => ur.AssignedBy)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Aynı kullanıcıya aynı rol iki kez atanamaz
        builder.HasIndex(ur => new { ur.UserId, ur.RoleName })
            .IsUnique()
            .HasDatabaseName("IX_user_roles_user_role_unique");
    }
}