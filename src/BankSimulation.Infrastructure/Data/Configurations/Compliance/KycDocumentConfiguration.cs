using BankSimulation.Domain.Entities.Compliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.Compliance;

public class KycDocumentConfiguration : IEntityTypeConfiguration<KycDocument>
{
    public void Configure(EntityTypeBuilder<KycDocument> builder)
    {
        builder.ToTable("kyc_documents");

        builder.HasKey(d => d.DocumentId);
        builder.Property(d => d.DocumentId).HasColumnName("document_id");

        builder.Property(d => d.UserId).HasColumnName("user_id").IsRequired();

        builder.Property(d => d.DocumentType).HasColumnName("document_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(d => d.DocumentNumber).HasColumnName("document_number").HasMaxLength(50).IsRequired();
        builder.Property(d => d.DocumentFilePath).HasColumnName("document_file_path").HasMaxLength(255).IsRequired();
        builder.Property(d => d.DocumentHash).HasColumnName("document_hash").HasMaxLength(255);
        
        builder.Property(d => d.ExpiryDate).HasColumnName("expiry_date").HasColumnType("date");
        builder.Property(d => d.UploadDate).HasColumnName("upload_date").HasDefaultValueSql("GETDATE()"); // Yerel Saat
        
        builder.Property(d => d.VerifiedAt).HasColumnName("verified_at");
        builder.Property(d => d.VerifiedBy).HasColumnName("verified_by");
        builder.Property(d => d.VerificationStatus).HasColumnName("verification_status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(d => d.RejectionReason).HasColumnName("rejection_reason").HasMaxLength(255);

        // İlişkiler
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.VerifiedByUser)
            .WithMany()
            .HasForeignKey(d => d.VerifiedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}