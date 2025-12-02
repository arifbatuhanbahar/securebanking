using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class KycVerificationConfiguration : IEntityTypeConfiguration<KycVerification>
{
    public void Configure(EntityTypeBuilder<KycVerification> builder)
    {
        builder.ToTable("kyc_verifications");

        builder.HasKey(v => v.VerificationId);
        builder.Property(v => v.VerificationId).HasColumnName("verification_id");

        builder.Property(v => v.UserId).HasColumnName("user_id").IsRequired();
        
        builder.Property(v => v.VerificationType).HasColumnName("verification_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(v => v.VerificationMethod).HasColumnName("verification_method").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(v => v.VerificationStatus).HasColumnName("verification_status").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(v => v.VerificationCode).HasColumnName("verification_code").HasMaxLength(10);
        builder.Property(v => v.VerifiedAt).HasColumnName("verified_at");
        builder.Property(v => v.Attempts).HasColumnName("attempts").HasDefaultValue(0);
        builder.Property(v => v.ExpiresAt).HasColumnName("expires_at").IsRequired();

        // İlişki
        builder.HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}