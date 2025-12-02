using BankSimulation.Domain.Entities.PaymentAndCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.PaymentAndCards;

public class CardTransactionConfiguration : IEntityTypeConfiguration<CardTransaction>
{
    public void Configure(EntityTypeBuilder<CardTransaction> builder)
    {
        builder.ToTable("card_transactions");

        builder.HasKey(ct => ct.CardTransactionId);
        builder.Property(ct => ct.CardTransactionId).HasColumnName("card_transaction_id");

        builder.Property(ct => ct.CardId).HasColumnName("card_id").IsRequired();
        
        builder.Property(ct => ct.MerchantName).HasColumnName("merchant_name").HasMaxLength(100).IsRequired();
        builder.Property(ct => ct.MerchantCategory).HasColumnName("merchant_category").HasMaxLength(50);
        
        builder.Property(ct => ct.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();
        builder.Property(ct => ct.Currency).HasColumnName("currency").HasConversion<string>().HasMaxLength(3).IsRequired();
        
        // Yerel Saat
        builder.Property(ct => ct.TransactionDate).HasColumnName("transaction_date").HasDefaultValueSql("GETDATE()");
        
        builder.Property(ct => ct.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(ct => ct.AuthorizationCode).HasColumnName("authorization_code").HasMaxLength(50);
        
        builder.Property(ct => ct.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        builder.Property(ct => ct.Location).HasColumnName("location").HasMaxLength(100);

        // İlişkiler
        builder.HasOne(ct => ct.CreditCard)
            .WithMany(c => c.CardTransactions)
            .HasForeignKey(ct => ct.CardId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}