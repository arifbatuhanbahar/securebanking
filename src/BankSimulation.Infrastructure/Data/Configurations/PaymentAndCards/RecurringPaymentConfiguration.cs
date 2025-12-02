using BankSimulation.Domain.Entities.PaymentAndCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.PaymentAndCards;

public class RecurringPaymentConfiguration : IEntityTypeConfiguration<RecurringPayment>
{
    public void Configure(EntityTypeBuilder<RecurringPayment> builder)
    {
        builder.ToTable("recurring_payments");

        builder.HasKey(rp => rp.RecurringId);
        builder.Property(rp => rp.RecurringId).HasColumnName("recurring_id");

        builder.Property(rp => rp.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(rp => rp.PaymentMethodId).HasColumnName("payment_method_id").IsRequired();
        
        builder.Property(rp => rp.MerchantName).HasColumnName("merchant_name").HasMaxLength(100).IsRequired();
        builder.Property(rp => rp.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();
        
        builder.Property(rp => rp.Frequency).HasColumnName("frequency").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(rp => rp.NextPaymentDate).HasColumnName("next_payment_date").HasColumnType("date");
        builder.Property(rp => rp.LastPaymentDate).HasColumnName("last_payment_date").HasColumnType("date");
        
        builder.Property(rp => rp.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

        // İlişkiler
        builder.HasOne(rp => rp.User)
            .WithMany()
            .HasForeignKey(rp => rp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rp => rp.PaymentMethod)
            .WithMany(pm => pm.RecurringPayments)
            .HasForeignKey(rp => rp.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}