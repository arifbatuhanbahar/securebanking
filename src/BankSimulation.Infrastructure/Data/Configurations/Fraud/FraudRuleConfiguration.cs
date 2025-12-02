using BankSimulation.Domain.Entities.Fraud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Fraud;

public class FraudRuleConfiguration : IEntityTypeConfiguration<FraudRule>
{
    public void Configure(EntityTypeBuilder<FraudRule> builder)
    {
        builder.ToTable("fraud_rules");

        builder.HasKey(r => r.RuleId);
        builder.Property(r => r.RuleId).HasColumnName("rule_id");

        builder.Property(r => r.RuleName).HasColumnName("rule_name").HasMaxLength(100).IsRequired();
        builder.Property(r => r.RuleType).HasColumnName("rule_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(r => r.RuleDescription).HasColumnName("rule_description").HasMaxLength(500);
        
        // JSON verisi
        builder.Property(r => r.RuleConditions).HasColumnName("rule_conditions").HasColumnType("nvarchar(max)").IsRequired();
        
        builder.Property(r => r.RiskScoreWeight).HasColumnName("risk_score_weight").HasDefaultValue(0);
        builder.Property(r => r.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        
        // Yerel Saat
        builder.Property(r => r.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(r => r.UpdatedAt).HasColumnName("updated_at");
    }
}