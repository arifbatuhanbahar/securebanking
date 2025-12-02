using BankSimulation.Domain.Entities.PaymentAndCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.PaymentAndCards;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.ToTable("credit_cards");

        builder.HasKey(c => c.CardId);
        builder.Property(c => c.CardId).HasColumnName("card_id");

        builder.Property(c => c.UserId).HasColumnName("user_id").IsRequired();

        // Şifreli Kart Bilgileri
        builder.Property(c => c.CardNumberEncrypted).HasColumnName("card_number_encrypted").HasMaxLength(512).IsRequired();
        builder.Property(c => c.CardLastFour).HasColumnName("card_last_four").HasMaxLength(4).IsRequired();
        builder.Property(c => c.CvvEncrypted).HasColumnName("cvv_encrypted").HasMaxLength(512).IsRequired();

        // Enumlar (String olarak saklanır)
        builder.Property(c => c.CardType).HasColumnName("card_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(c => c.CardBrand).HasColumnName("card_brand").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(c => c.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

        // Parasal Alanlar (Hassasiyet ayarı)
        builder.Property(c => c.CreditLimit).HasColumnName("credit_limit").HasPrecision(18, 2);
        builder.Property(c => c.AvailableLimit).HasColumnName("available_limit").HasPrecision(18, 2);
        builder.Property(c => c.CurrentBalance).HasColumnName("current_balance").HasPrecision(18, 2);
        builder.Property(c => c.MinimumPayment).HasColumnName("minimum_payment").HasPrecision(18, 2);
        builder.Property(c => c.InterestRate).HasColumnName("interest_rate").HasPrecision(5, 2);

        // Tarihler
        builder.Property(c => c.PaymentDueDate).HasColumnName("payment_due_date").HasColumnType("date");
        builder.Property(c => c.ExpiryMonth).HasColumnName("expiry_month");
        builder.Property(c => c.ExpiryYear).HasColumnName("expiry_year");
        
        // Yerel Saat (Türkiye Saati)
        builder.Property(c => c.IssuedAt).HasColumnName("issued_at").HasDefaultValueSql("GETDATE()");
        builder.Property(c => c.ActivatedAt).HasColumnName("activated_at");

        // İlişkiler
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}