using BankSimulation.Domain.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.AccountManagement;

public class AccountBeneficiaryConfiguration : IEntityTypeConfiguration<AccountBeneficiary>
{
    public void Configure(EntityTypeBuilder<AccountBeneficiary> builder)
    {
        builder.ToTable("account_beneficiaries");

        builder.HasKey(ab => ab.BeneficiaryId);
        builder.Property(ab => ab.BeneficiaryId)
            .HasColumnName("beneficiary_id");

        builder.Property(ab => ab.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.Property(ab => ab.BeneficiaryName)
            .HasColumnName("beneficiary_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ab => ab.BeneficiaryIban)
            .HasColumnName("beneficiary_iban")
            .HasMaxLength(34)
            .IsRequired();

        builder.Property(ab => ab.BeneficiaryBank)
            .HasColumnName("beneficiary_bank")
            .HasMaxLength(100);

        builder.Property(ab => ab.Nickname)
            .HasColumnName("nickname")
            .HasMaxLength(50);

        builder.Property(ab => ab.IsVerified)
            .HasColumnName("is_verified")
            .HasDefaultValue(false);

        builder.Property(ab => ab.AddedAt)
            .HasColumnName("added_at")
            .HasDefaultValueSql("GETUTCDATE()");

        // Account -> Beneficiaries ilişkisi
        builder.HasOne(ab => ab.Account)
            .WithMany(a => a.Beneficiaries)
            .HasForeignKey(ab => ab.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(ab => ab.AccountId)
            .HasDatabaseName("IX_account_beneficiaries_account");
    }
}