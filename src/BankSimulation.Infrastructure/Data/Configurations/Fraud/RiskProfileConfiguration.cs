using BankSimulation.Domain.Entities.Fraud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Fraud;

public class RiskProfileConfiguration : IEntityTypeConfiguration<RiskProfile>
{
    public void Configure(EntityTypeBuilder<RiskProfile> builder)
    {
        builder.ToTable("risk_profiles");

        builder.HasKey(p => p.ProfileId);
        builder.Property(p => p.ProfileId).HasColumnName("profile_id");

        builder.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
        
        builder.Property(p => p.RiskLevel).HasColumnName("risk_level").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(p => p.TransactionVelocityScore).HasColumnName("transaction_velocity_score").HasDefaultValue(0);
        builder.Property(p => p.AmountAnomalyScore).HasColumnName("amount_anomaly_score").HasDefaultValue(0);
        builder.Property(p => p.GeographicRiskScore).HasColumnName("geographic_risk_score").HasDefaultValue(0);
        builder.Property(p => p.BehavioralScore).HasColumnName("behavioral_score").HasDefaultValue(0);
        
        // Yerel Saat
        builder.Property(p => p.LastCalculatedAt).HasColumnName("last_calculated_at").HasDefaultValueSql("GETDATE()");
        
        builder.Property(p => p.Factors).HasColumnName("factors").HasColumnType("nvarchar(max)"); // JSON

        // İlişki
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}