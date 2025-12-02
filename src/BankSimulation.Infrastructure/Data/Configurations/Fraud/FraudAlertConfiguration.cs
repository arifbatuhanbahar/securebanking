using BankSimulation.Domain.Entities.Fraud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Fraud;

public class FraudAlertConfiguration : IEntityTypeConfiguration<FraudAlert>
{
    public void Configure(EntityTypeBuilder<FraudAlert> builder)
    {
        builder.ToTable("fraud_alerts");

        builder.HasKey(a => a.AlertId);
        builder.Property(a => a.AlertId).HasColumnName("alert_id");

        builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(a => a.TransactionId).HasColumnName("transaction_id").IsRequired();
        
        builder.Property(a => a.FraudScore).HasColumnName("fraud_score").IsRequired();
        builder.Property(a => a.TriggeredRules).HasColumnName("triggered_rules").HasColumnType("nvarchar(max)"); // Array/JSON
        
        builder.Property(a => a.AlertSeverity).HasColumnName("alert_severity").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        // Yerel Saat
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        
        builder.Property(a => a.ReviewedAt).HasColumnName("reviewed_at");
        builder.Property(a => a.ReviewedBy).HasColumnName("reviewed_by");
        builder.Property(a => a.ResolutionNotes).HasColumnName("resolution_notes").HasMaxLength(500);

        // İlişkiler
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse de alarmı kalsın

        builder.HasOne(a => a.Transaction)
            .WithMany()
            .HasForeignKey(a => a.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.ReviewedByUser)
            .WithMany()
            .HasForeignKey(a => a.ReviewedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}