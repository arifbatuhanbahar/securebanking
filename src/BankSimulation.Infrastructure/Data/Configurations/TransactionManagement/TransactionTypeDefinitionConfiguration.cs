using BankSimulation.Domain.Entities.TransactionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class TransactionTypeDefinitionConfiguration : IEntityTypeConfiguration<TransactionTypeDefinition>
{
    public void Configure(EntityTypeBuilder<TransactionTypeDefinition> builder)
    {
        builder.ToTable("transaction_types");

        builder.HasKey(t => t.TypeId);
        builder.Property(t => t.TypeId).HasColumnName("type_id");

        builder.Property(t => t.TypeName).HasColumnName("type_name").HasMaxLength(50).IsRequired();
        builder.Property(t => t.TypeCode).HasColumnName("type_code").HasMaxLength(20).IsRequired();
        builder.Property(t => t.Description).HasColumnName("description").HasMaxLength(255);
        
        builder.Property(t => t.FeeFixed).HasColumnName("fee_fixed").HasPrecision(18, 2);
        builder.Property(t => t.FeePercentage).HasColumnName("fee_percentage").HasPrecision(5, 4); // %0.0001 hassasiyet
        
        builder.Property(t => t.IsActive).HasColumnName("is_active").HasDefaultValue(true);
    }
}