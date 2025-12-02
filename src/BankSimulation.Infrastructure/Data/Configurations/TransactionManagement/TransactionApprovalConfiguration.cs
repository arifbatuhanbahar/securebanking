using BankSimulation.Domain.Entities.TransactionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class TransactionApprovalConfiguration : IEntityTypeConfiguration<TransactionApproval>
{
    public void Configure(EntityTypeBuilder<TransactionApproval> builder)
    {
        builder.ToTable("transaction_approvals");

        builder.HasKey(a => a.ApprovalId);
        builder.Property(a => a.ApprovalId).HasColumnName("approval_id");

        builder.Property(a => a.TransactionId).HasColumnName("transaction_id").IsRequired();
        builder.Property(a => a.ApproverId).HasColumnName("approver_id").IsRequired();
        
        builder.Property(a => a.ApprovalLevel).HasColumnName("approval_level").HasDefaultValue(1);
        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.Comments).HasColumnName("comments").HasMaxLength(500);
        builder.Property(a => a.ApprovedAt).HasColumnName("approved_at");

        // İlişkiler
        builder.HasOne(a => a.Transaction)
            .WithMany(t => t.TransactionApprovals)
            .HasForeignKey(a => a.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Approver)
            .WithMany()
            .HasForeignKey(a => a.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}