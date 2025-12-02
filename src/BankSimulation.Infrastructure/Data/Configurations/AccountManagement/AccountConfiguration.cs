using BankSimulation.Domain.Entities.AccountManagement;
using BankSimulation.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.AccountManagement;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.AccountId);
        builder.Property(a => a.AccountId)
            .HasColumnName("account_id");

        builder.Property(a => a.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        // IBAN - UNIQUE
        builder.Property(a => a.AccountNumber)
            .HasColumnName("account_number")
            .HasMaxLength(34)
            .IsRequired();

        builder.HasIndex(a => a.AccountNumber)
            .IsUnique()
            .HasDatabaseName("IX_accounts_account_number");

        builder.Property(a => a.AccountType)
            .HasColumnName("account_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Currency)
            .HasColumnName("currency")
            .HasConversion<string>()
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(a => a.Balance)
            .HasColumnName("balance")
            .HasPrecision(18, 2);

        builder.Property(a => a.AvailableBalance)
            .HasColumnName("available_balance")
            .HasPrecision(18, 2);

        builder.Property(a => a.DailyTransferLimit)
            .HasColumnName("daily_transfer_limit")
            .HasPrecision(18, 2);

        builder.Property(a => a.DailyWithdrawalLimit)
            .HasColumnName("daily_withdrawal_limit")
            .HasPrecision(18, 2);

        builder.Property(a => a.InterestRate)
            .HasColumnName("interest_rate")
            .HasPrecision(5, 2);

        builder.Property(a => a.InterestCalculationDate)
            .HasColumnName("interest_calculation_date");

        builder.Property(a => a.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(Domain.Enums.AccountStatus.Active);

        builder.Property(a => a.OpenedDate)
            .HasColumnName("opened_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.ClosedDate)
            .HasColumnName("closed_date");

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at");

        // User -> Accounts ilişkisi
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Index
        builder.HasIndex(a => new { a.UserId, a.Status })
            .HasDatabaseName("IX_accounts_user_status");
    }
}