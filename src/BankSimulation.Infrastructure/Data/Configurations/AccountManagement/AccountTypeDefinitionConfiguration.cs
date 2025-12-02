using BankSimulation.Domain.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.AccountManagement;

public class AccountTypeDefinitionConfiguration : IEntityTypeConfiguration<AccountTypeDefinition>
{
    public void Configure(EntityTypeBuilder<AccountTypeDefinition> builder)
    {
        builder.ToTable("account_types");

        builder.HasKey(at => at.TypeId);
        builder.Property(at => at.TypeId)
            .HasColumnName("type_id");

        builder.Property(at => at.TypeName)
            .HasColumnName("type_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(at => at.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(at => at.MinBalance)
            .HasColumnName("min_balance")
            .HasPrecision(18, 2);

        builder.Property(at => at.InterestRate)
            .HasColumnName("interest_rate")
            .HasPrecision(5, 2);

        builder.Property(at => at.Features)
            .HasColumnName("features")
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);
    }
}