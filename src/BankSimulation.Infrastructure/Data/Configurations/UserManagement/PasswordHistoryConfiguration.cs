using BankSimulation.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.UserManagement;

public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.ToTable("password_history");
        
        builder.HasKey(ph => ph.HistoryId);
        builder.Property(ph => ph.HistoryId)
            .HasColumnName("history_id");
        
        builder.Property(ph => ph.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(ph => ph.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();
        
        builder.Property(ph => ph.ChangedAt)
            .HasColumnName("changed_at")
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(ph => ph.ChangedBy)
            .HasColumnName("changed_by");
        
        // User -> PasswordHistories ilişkisi (NO ACTION)
        builder.HasOne(ph => ph.User)
            .WithMany(u => u.PasswordHistories)
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // ChangedBy -> User ilişkisi (NO ACTION)
        builder.HasOne(ph => ph.ChangedByUser)
            .WithMany(u => u.ChangedPasswords)
            .HasForeignKey(ph => ph.ChangedBy)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasIndex(ph => new { ph.UserId, ph.ChangedAt })
            .HasDatabaseName("IX_password_history_user_date");
    }
}