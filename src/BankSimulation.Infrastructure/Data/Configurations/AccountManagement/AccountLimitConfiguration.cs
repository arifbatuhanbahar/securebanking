using BankSimulation.Domain.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.AccountManagement;

public class AccountLimitConfiguration : IEntityTypeConfiguration<AccountLimit>
{
    public void Configure(EntityTypeBuilder<AccountLimit> builder)
    {
        builder.ToTable("account_limits");

        builder.HasKey(al => al.LimitId);
        builder.Property(al => al.LimitId)
            .HasColumnName("limit_id");

        builder.Property(al => al.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.Property(al => al.LimitType)
            .HasColumnName("limit_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(al => al.LimitAmount)
            .HasColumnName("limit_amount")
            .HasPrecision(18, 2);

        builder.Property(al => al.UsedAmount)
            .HasColumnName("used_amount")
            .HasPrecision(18, 2);

        builder.Property(al => al.ResetDate)
            .HasColumnName("reset_date");

        builder.Property(al => al.LastUpdated)
            .HasColumnName("last_updated");

        // Account -> Limits ilişkisi
        builder.HasOne(al => al.Account)
            .WithMany(a => a.Limits)
            .HasForeignKey(al => al.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Aynı hesapta aynı limit türü bir kez olabilir
        builder.HasIndex(al => new { al.AccountId, al.LimitType })
            .IsUnique()
            .HasDatabaseName("IX_account_limits_account_type_unique");
    }
}