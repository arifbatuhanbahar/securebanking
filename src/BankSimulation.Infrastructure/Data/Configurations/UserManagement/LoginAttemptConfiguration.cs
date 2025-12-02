using BankSimulation.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.UserManagement;

public class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.ToTable("login_attempts");
        
        builder.HasKey(la => la.AttemptId);
        builder.Property(la => la.AttemptId)
            .HasColumnName("attempt_id");
        
        builder.Property(la => la.UserId)
            .HasColumnName("user_id");
        
        builder.Property(la => la.EmailAttempted)
            .HasColumnName("email_attempted")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(la => la.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(45);
        
        builder.Property(la => la.Success)
            .HasColumnName("success")
            .IsRequired();
        
        builder.Property(la => la.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(255);
        
        builder.Property(la => la.AttemptedAt)
            .HasColumnName("attempted_at")
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(la => la.UserAgent)
            .HasColumnName("user_agent")
            .HasMaxLength(500);
        
        // User -> LoginAttempts ilişkisi (NO ACTION)
        builder.HasOne(la => la.User)
            .WithMany(u => u.LoginAttempts)
            .HasForeignKey(la => la.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Brute-force tespiti için index
        builder.HasIndex(la => new { la.IpAddress, la.AttemptedAt })
            .HasDatabaseName("IX_login_attempts_ip_time");
        
        builder.HasIndex(la => new { la.EmailAttempted, la.AttemptedAt })
            .HasDatabaseName("IX_login_attempts_email_time");
    }
}