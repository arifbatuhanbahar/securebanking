using BankSimulation.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Audit;

public class PciAuditLogConfiguration : IEntityTypeConfiguration<PciAuditLog>
{
    public void Configure(EntityTypeBuilder<PciAuditLog> builder)
    {
        builder.ToTable("pci_audit_log");

        builder.HasKey(p => p.LogId);
        builder.Property(p => p.LogId).HasColumnName("log_id");

        builder.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(p => p.Action).HasColumnName("action").HasConversion<string>().HasMaxLength(50).IsRequired();
        
        // Kart token'ı hassas veridir
        builder.Property(p => p.CardToken).HasColumnName("card_token").HasMaxLength(512).IsRequired();
        
        builder.Property(p => p.Timestamp).HasColumnName("timestamp").HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        builder.Property(p => p.Success).HasColumnName("success").IsRequired();
        builder.Property(p => p.Reason).HasColumnName("reason").HasMaxLength(255);

        // İlişki
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}