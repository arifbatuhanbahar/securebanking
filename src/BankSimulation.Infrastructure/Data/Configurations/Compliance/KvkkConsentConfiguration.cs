using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class KvkkConsentConfiguration : IEntityTypeConfiguration<KvkkConsent>
{
    public void Configure(EntityTypeBuilder<KvkkConsent> builder)
    {
        builder.ToTable("kvkk_consents");

        builder.HasKey(c => c.ConsentId);
        builder.Property(c => c.ConsentId).HasColumnName("consent_id");

        builder.Property(c => c.UserId).HasColumnName("user_id").IsRequired();
        
        builder.Property(c => c.ConsentType).HasColumnName("consent_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(c => c.ConsentGiven).HasColumnName("consent_given").IsRequired();
        
        // Metin uzun olabilir
        builder.Property(c => c.ConsentText).HasColumnName("consent_text").HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(c => c.ConsentVersion).HasColumnName("consent_version").HasMaxLength(20).IsRequired();
        
        builder.Property(c => c.GrantedAt).HasColumnName("granted_at").HasDefaultValueSql("GETDATE()"); // Yerel Saat
        builder.Property(c => c.RevokedAt).HasColumnName("revoked_at");
        builder.Property(c => c.IpAddress).HasColumnName("ip_address").HasMaxLength(45);

        // İlişki
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}