using BankSimulation.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Audit;

public class EncryptionKeyConfiguration : IEntityTypeConfiguration<EncryptionKey>
{
    public void Configure(EntityTypeBuilder<EncryptionKey> builder)
    {
        builder.ToTable("encryption_keys");

        builder.HasKey(k => k.KeyId);
        builder.Property(k => k.KeyId).HasColumnName("key_id");

        builder.Property(k => k.KeyName).HasColumnName("key_name").HasMaxLength(100).IsRequired();
        builder.Property(k => k.KeyType).HasColumnName("key_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(k => k.KeyValueEncrypted).HasColumnName("key_value_encrypted").HasMaxLength(1024).IsRequired();
        
        builder.Property(k => k.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
        builder.Property(k => k.ExpiresAt).HasColumnName("expires_at").HasColumnType("date").IsRequired();
        builder.Property(k => k.RotationDate).HasColumnName("rotation_date");
        
        builder.Property(k => k.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
    }
}