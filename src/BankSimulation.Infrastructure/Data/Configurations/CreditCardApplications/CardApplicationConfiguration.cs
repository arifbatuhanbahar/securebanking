using BankSimulation.Domain.Entities.CreditCardApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.CreditCardApplications;

public class CardApplicationConfiguration : IEntityTypeConfiguration<CardApplication>
{
    public void Configure(EntityTypeBuilder<CardApplication> builder)
    {
        builder.ToTable("card_applications");

        builder.HasKey(a => a.ApplicationId);
        builder.Property(a => a.ApplicationId).HasColumnName("application_id");

        builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();
        
        builder.Property(a => a.CardTypeRequested).HasColumnName("card_type_requested").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(a => a.MonthlyIncome).HasColumnName("monthly_income").HasPrecision(18, 2).IsRequired();
        builder.Property(a => a.EmploymentStatus).HasColumnName("employment_status").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(a => a.EmployerName).HasColumnName("employer_name").HasMaxLength(100).IsRequired();
        
        // Yerel Saat
        builder.Property(a => a.ApplicationDate).HasColumnName("application_date").HasDefaultValueSql("GETDATE()");
        
        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(a => a.RejectionReason).HasColumnName("rejection_reason").HasMaxLength(255);
        
        builder.Property(a => a.ApprovedBy).HasColumnName("approved_by");
        builder.Property(a => a.ApprovedAt).HasColumnName("approved_at");
        builder.Property(a => a.CreditLimitApproved).HasColumnName("credit_limit_approved").HasPrecision(18, 2);

        // İlişkiler
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.ApprovedByUser)
            .WithMany()
            .HasForeignKey(a => a.ApprovedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}