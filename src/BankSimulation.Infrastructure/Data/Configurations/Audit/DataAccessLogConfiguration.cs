using BankSimulation.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Audit;

public class DataAccessLogConfiguration : IEntityTypeConfiguration<DataAccessLog>
{
    public void Configure(EntityTypeBuilder<DataAccessLog> builder)
    {
        builder.ToTable("data_access_log");

        builder.HasKey(d => d.LogId);
        builder.Property(d => d.LogId).HasColumnName("log_id");

        builder.Property(d => d.AccessedByUserId).HasColumnName("accessed_by_user_id").IsRequired();
        builder.Property(d => d.TargetUserId).HasColumnName("target_user_id").IsRequired();
        
        builder.Property(d => d.DataType).HasColumnName("data_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(d => d.AccessReason).HasColumnName("access_reason").HasMaxLength(255);
        
        builder.Property(d => d.AccessTimestamp).HasColumnName("access_timestamp").HasDefaultValueSql("GETDATE()");
        builder.Property(d => d.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        builder.Property(d => d.IsSensitive).HasColumnName("is_sensitive").HasDefaultValue(false);

        // İlişkiler
        builder.HasOne(d => d.AccessedByUser)
            .WithMany()
            .HasForeignKey(d => d.AccessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TargetUser)
            .WithMany()
            .HasForeignKey(d => d.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}