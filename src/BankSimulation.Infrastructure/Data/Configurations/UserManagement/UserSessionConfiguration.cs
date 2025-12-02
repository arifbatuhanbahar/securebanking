using BankSimulation.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.UserManagement;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("user_sessions");
        
        builder.HasKey(us => us.SessionId);
        builder.Property(us => us.SessionId)
            .HasColumnName("session_id");
        
        builder.Property(us => us.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(us => us.SessionToken)
            .HasColumnName("session_token")
            .HasMaxLength(512)
            .IsRequired();
        
        builder.HasIndex(us => us.SessionToken)
            .IsUnique()
            .HasDatabaseName("IX_user_sessions_token");
        
        builder.Property(us => us.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(45);
        
        builder.Property(us => us.UserAgent)
            .HasColumnName("user_agent")
            .HasMaxLength(500);
        
        builder.Property(us => us.DeviceInfo)
            .HasColumnName("device_info")
            .HasMaxLength(500);
        
        builder.Property(us => us.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(us => us.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();
        
        builder.Property(us => us.LastActivityAt)
            .HasColumnName("last_activity_at");
        
        builder.Property(us => us.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);
        
        // User -> Sessions ilişkisi (NO ACTION)
        builder.HasOne(us => us.User)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasIndex(us => new { us.UserId, us.IsActive })
            .HasDatabaseName("IX_user_sessions_user_active");
    }
}