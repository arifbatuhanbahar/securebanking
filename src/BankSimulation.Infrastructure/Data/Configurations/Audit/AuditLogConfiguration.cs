using BankSimulation.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Audit;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(a => a.LogId);
        builder.Property(a => a.LogId).HasColumnName("log_id");

        builder.Property(a => a.TableName).HasColumnName("table_name").HasMaxLength(100).IsRequired();
        builder.Property(a => a.RecordId).HasColumnName("record_id").HasMaxLength(50).IsRequired();
        
        builder.Property(a => a.Action).HasColumnName("action").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(a => a.OldValue).HasColumnName("old_value").HasColumnType("nvarchar(max)"); // JSON
        builder.Property(a => a.NewValue).HasColumnName("new_value").HasColumnType("nvarchar(max)"); // JSON
        builder.Property(a => a.ChangedFields).HasColumnName("changed_fields").HasColumnType("nvarchar(max)");
        
        builder.Property(a => a.ChangedBy).HasColumnName("changed_by");
        builder.Property(a => a.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        
        // Yerel Saat
        builder.Property(a => a.Timestamp).HasColumnName("timestamp").HasDefaultValueSql("GETDATE()");
    }
}