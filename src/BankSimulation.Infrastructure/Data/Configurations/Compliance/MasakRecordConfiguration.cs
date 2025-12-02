using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class MasakRecordConfiguration : IEntityTypeConfiguration<MasakRecord>
{
    public void Configure(EntityTypeBuilder<MasakRecord> builder)
    {
        builder.ToTable("masak_records");

        builder.HasKey(m => m.RecordId);
        builder.Property(m => m.RecordId).HasColumnName("record_id");

        builder.Property(m => m.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(m => m.TransactionId).HasColumnName("transaction_id");
        
        builder.Property(m => m.RecordType).HasColumnName("record_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        
        builder.Property(m => m.Data).HasColumnName("data").HasColumnType("nvarchar(max)").IsRequired(); // JSON
        
        builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()"); // Yerel Saat
        builder.Property(m => m.RetentionUntil).HasColumnName("retention_until").HasColumnType("date").IsRequired();

        // İlişkiler
        builder.HasOne(m => m.Customer)
            .WithMany()
            .HasForeignKey(m => m.CustomerId)
            .OnDelete(DeleteBehavior.Restrict); // MASAK kayıtları silinemez, kullanıcı silinse bile durmalı

        builder.HasOne(m => m.Transaction)
            .WithMany()
            .HasForeignKey(m => m.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}