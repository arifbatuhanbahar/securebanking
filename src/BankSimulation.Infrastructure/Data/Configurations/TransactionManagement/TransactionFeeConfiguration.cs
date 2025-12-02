using BankSimulation.Domain.Entities.TransactionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class TransactionFeeConfiguration : IEntityTypeConfiguration<TransactionFee>
{
    public void Configure(EntityTypeBuilder<TransactionFee> builder)
    {
        builder.ToTable("transaction_fees");

        builder.HasKey(f => f.FeeId);
        builder.Property(f => f.FeeId).HasColumnName("fee_id");

        builder.Property(f => f.TransactionId).HasColumnName("transaction_id").IsRequired();
        builder.Property(f => f.FeeType).HasColumnName("fee_type").HasMaxLength(50).IsRequired();
        builder.Property(f => f.FeeAmount).HasColumnName("fee_amount").HasPrecision(18, 2).IsRequired();
        builder.Property(f => f.AppliedAt).HasColumnName("applied_at").HasDefaultValueSql("GETUTCDATE()");

        // İlişki: Transaction silinirse ücretler de silinsin (Cascade)
        builder.HasOne(f => f.Transaction)
            .WithMany(t => t.TransactionFees)
            .HasForeignKey(f => f.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}