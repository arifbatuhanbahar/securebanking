using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class KvkkDataRequestConfiguration : IEntityTypeConfiguration<KvkkDataRequest>
{
    public void Configure(EntityTypeBuilder<KvkkDataRequest> builder)
    {
        builder.ToTable("kvkk_data_requests");

        builder.HasKey(r => r.RequestId);
        builder.Property(r => r.RequestId).HasColumnName("request_id");

        builder.Property(r => r.UserId).HasColumnName("user_id").IsRequired();
        
        builder.Property(r => r.RequestType).HasColumnName("request_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(r => r.RequestDate).HasColumnName("request_date").HasDefaultValueSql("GETDATE()"); // Yerel Saat
        
        builder.Property(r => r.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(r => r.CompletedAt).HasColumnName("completed_at");
        builder.Property(r => r.CompletedBy).HasColumnName("completed_by");
        
        builder.Property(r => r.ResponseData).HasColumnName("response_data").HasColumnType("nvarchar(max)"); // JSON
        builder.Property(r => r.ResponseFilePath).HasColumnName("response_file_path").HasMaxLength(255);

        // İlişkiler
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.CompletedByUser)
            .WithMany()
            .HasForeignKey(r => r.CompletedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}