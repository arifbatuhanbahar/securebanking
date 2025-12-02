using BankSimulation.Domain.Entities.TransactionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
{
    public void Configure(EntityTypeBuilder<GeneralLedger> builder)
    {
        builder.ToTable("general_ledger");

        builder.HasKey(l => l.LedgerId);
        builder.Property(l => l.LedgerId).HasColumnName("ledger_id");

        builder.Property(l => l.TransactionId).HasColumnName("transaction_id").IsRequired();
        builder.Property(l => l.AccountId).HasColumnName("account_id").IsRequired();
        
        builder.Property(l => l.DebitAmount).HasColumnName("debit_amount").HasPrecision(18, 2);
        builder.Property(l => l.CreditAmount).HasColumnName("credit_amount").HasPrecision(18, 2);
        
        builder.Property(l => l.EntryType).HasColumnName("entry_type").HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Property(l => l.EntryDate).HasColumnName("entry_date").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(l => l.Description).HasColumnName("description").HasMaxLength(255);

        // İlişkiler
        builder.HasOne(l => l.Transaction)
            .WithMany(t => t.GeneralLedgerEntries)
            .HasForeignKey(l => l.TransactionId)
            .OnDelete(DeleteBehavior.Restrict); // Muhasebe kaydı silinmemeli

        builder.HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}