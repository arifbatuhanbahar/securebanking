using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class SuspiciousActivityReportConfiguration : IEntityTypeConfiguration<SuspiciousActivityReport>
{
    public void Configure(EntityTypeBuilder<SuspiciousActivityReport> builder)
    {
        builder.ToTable("suspicious_activity_reports");

        builder.HasKey(s => s.SarId);
        builder.Property(s => s.SarId).HasColumnName("sar_id");

        builder.Property(s => s.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(s => s.TransactionId).HasColumnName("transaction_id");
        
        builder.Property(s => s.ReportType).HasColumnName("report_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(1000).IsRequired();
        builder.Property(s => s.RiskScore).HasColumnName("risk_score").HasDefaultValue(0);
        
        builder.Property(s => s.CreatedBy).HasColumnName("created_by");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()"); // Yerel Saat
        
        builder.Property(s => s.ReportedToMasak).HasColumnName("reported_to_masak").HasDefaultValue(false);
        builder.Property(s => s.MasakReportDate).HasColumnName("masak_report_date");
        builder.Property(s => s.MasakReferenceNumber).HasColumnName("masak_reference_number").HasMaxLength(100);
        
        builder.Property(s => s.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

        // İlişkiler
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Transaction)
            .WithMany()
            .HasForeignKey(s => s.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.CreatedByUser)
            .WithMany()
            .HasForeignKey(s => s.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}