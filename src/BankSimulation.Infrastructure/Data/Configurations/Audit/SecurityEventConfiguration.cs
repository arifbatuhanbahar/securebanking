using BankSimulation.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Audit;

public class SecurityEventConfiguration : IEntityTypeConfiguration<SecurityEvent>
{
    public void Configure(EntityTypeBuilder<SecurityEvent> builder)
    {
        builder.ToTable("security_events");

        builder.HasKey(s => s.EventId);
        builder.Property(s => s.EventId).HasColumnName("event_id");

        builder.Property(s => s.EventType).HasColumnName("event_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.Property(s => s.Severity).HasColumnName("severity").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
        builder.Property(s => s.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
        builder.Property(s => s.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
        
        builder.Property(s => s.EventDate).HasColumnName("event_date").HasDefaultValueSql("GETDATE()");
        
        builder.Property(s => s.Resolved).HasColumnName("resolved").HasDefaultValue(false);
        builder.Property(s => s.ResolvedAt).HasColumnName("resolved_at");
        builder.Property(s => s.ResolvedBy).HasColumnName("resolved_by");

        // İlişkiler
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.ResolvedByUser)
            .WithMany()
            .HasForeignKey(s => s.ResolvedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}