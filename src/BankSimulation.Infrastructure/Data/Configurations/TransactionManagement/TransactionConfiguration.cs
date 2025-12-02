using BankSimulation.Domain.Entities.TransactionManagement;
using BankSimulation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.TransactionId);
        builder.Property(t => t.TransactionId).HasColumnName("transaction_id");

        builder.Property(t => t.FromAccountId).HasColumnName("from_account_id");
        builder.Property(t => t.ToAccountId).HasColumnName("to_account_id");

        builder.Property(t => t.Amount)
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Currency)
            .HasColumnName("currency")
            .HasConversion<string>()
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(t => t.TransactionType)
            .HasColumnName("transaction_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        // Reference Number - UNIQUE
        builder.Property(t => t.ReferenceNumber)
            .HasColumnName("reference_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(t => t.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("IX_transactions_reference_number");

        builder.Property(t => t.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(TransactionStatus.Pending);

        // Fraud & Security
        builder.Property(t => t.FraudScore).HasColumnName("fraud_score").HasDefaultValue(0);
        builder.Property(t => t.FraudFlags).HasColumnName("fraud_flags").HasColumnType("nvarchar(max)"); // JSON
        builder.Property(t => t.RequiresReview).HasColumnName("requires_review").HasDefaultValue(false);
        builder.Property(t => t.ReviewedBy).HasColumnName("reviewed_by");
        builder.Property(t => t.ReviewedAt).HasColumnName("reviewed_at");
        
        builder.Property(t => t.TransactionDate).HasColumnName("transaction_date").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(t => t.CompletedAt).HasColumnName("completed_at");
        builder.Property(t => t.CreatedBy).HasColumnName("created_by");

        builder.Property(t => t.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        builder.Property(t => t.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
        builder.Property(t => t.IsSuspicious).HasColumnName("is_suspicious").HasDefaultValue(false);
        builder.Property(t => t.SuspiciousReason).HasColumnName("suspicious_reason").HasMaxLength(255);
        builder.Property(t => t.ReportedToMasak).HasColumnName("reported_to_masak").HasDefaultValue(false);
        builder.Property(t => t.MasakReportDate).HasColumnName("masak_report_date");

        // Relationships
        // Gönderen Hesap (Restrict: Hesap silinirse işlem geçmişi bozulmasın)
        builder.HasOne(t => t.FromAccount)
            .WithMany()
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Alıcı Hesap
        builder.HasOne(t => t.ToAccount)
            .WithMany()
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // İnceleyen Kullanıcı
        builder.HasOne(t => t.ReviewedByUser)
            .WithMany()
            .HasForeignKey(t => t.ReviewedBy)
            .OnDelete(DeleteBehavior.NoAction);

        // Oluşturan Kullanıcı
        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
            
        // Indexler
        builder.HasIndex(t => t.TransactionDate).HasDatabaseName("IX_transactions_date");
        builder.HasIndex(t => new { t.FromAccountId, t.TransactionDate }).HasDatabaseName("IX_transactions_from_date");
        builder.HasIndex(t => new { t.ToAccountId, t.TransactionDate }).HasDatabaseName("IX_transactions_to_date");
    }
}