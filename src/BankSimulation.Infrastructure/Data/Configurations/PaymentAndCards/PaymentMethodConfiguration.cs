using BankSimulation.Domain.Entities.PaymentAndCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.PaymentAndCards;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(pm => pm.PaymentMethodId);
        builder.Property(pm => pm.PaymentMethodId).HasColumnName("payment_method_id");

        builder.Property(pm => pm.UserId).HasColumnName("user_id").IsRequired();

        builder.Property(pm => pm.MethodType).HasColumnName("method_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(pm => pm.CardToken).HasColumnName("card_token").HasMaxLength(512).IsRequired();
        builder.Property(pm => pm.CardLastFour).HasColumnName("card_last_four").HasMaxLength(4).IsRequired();
        builder.Property(pm => pm.CardBrand).HasColumnName("card_brand").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(pm => pm.ExpiryMonth).HasColumnName("expiry_month");
        builder.Property(pm => pm.ExpiryYear).HasColumnName("expiry_year");
        builder.Property(pm => pm.CardHolderName).HasColumnName("cardholder_name").HasMaxLength(100).IsRequired();
        
        builder.Property(pm => pm.IsDefault).HasColumnName("is_default").HasDefaultValue(false);
        builder.Property(pm => pm.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        // Yerel Saat
        builder.Property(pm => pm.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(pm => pm.LastUsedAt).HasColumnName("last_used_at");

        // İlişkiler
        builder.HasOne(pm => pm.User)
            .WithMany()
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}