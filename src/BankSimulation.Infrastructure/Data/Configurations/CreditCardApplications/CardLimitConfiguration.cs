using BankSimulation.Domain.Entities.CreditCardApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.CreditCardApplications;

public class CardLimitConfiguration : IEntityTypeConfiguration<CardLimit>
{
    public void Configure(EntityTypeBuilder<CardLimit> builder)
    {
        builder.ToTable("card_limits");

        builder.HasKey(l => l.LimitId);
        builder.Property(l => l.LimitId).HasColumnName("limit_id");

        builder.Property(l => l.CardId).HasColumnName("card_id").IsRequired();
        
        builder.Property(l => l.LimitType).HasColumnName("limit_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(l => l.LimitAmount).HasColumnName("limit_amount").HasPrecision(18, 2).IsRequired();
        builder.Property(l => l.UsedAmount).HasColumnName("used_amount").HasPrecision(18, 2).IsRequired();
        
        builder.Property(l => l.ResetDate).HasColumnName("reset_date");

        // İlişki
      builder.HasOne(l => l.CreditCard)
            .WithMany(c => c.CardLimits) // Artık bu listeyi tanıyor!
            .HasForeignKey(l => l.CardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}