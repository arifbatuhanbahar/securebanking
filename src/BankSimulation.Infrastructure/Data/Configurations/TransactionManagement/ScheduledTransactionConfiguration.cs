using BankSimulation.Domain.Entities.TransactionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.TransactionManagement;

public class ScheduledTransactionConfiguration : IEntityTypeConfiguration<ScheduledTransaction>
{
    public void Configure(EntityTypeBuilder<ScheduledTransaction> builder)
    {
        builder.ToTable("scheduled_transactions");

        builder.HasKey(s => s.ScheduleId);
        builder.Property(s => s.ScheduleId).HasColumnName("schedule_id");

        builder.Property(s => s.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(s => s.FromAccountId).HasColumnName("from_account_id").IsRequired();
        builder.Property(s => s.ToAccountId).HasColumnName("to_account_id").IsRequired();

        builder.Property(s => s.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();
        builder.Property(s => s.Frequency).HasColumnName("frequency").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(s => s.StartDate).HasColumnName("start_date").HasColumnType("date").IsRequired();
        builder.Property(s => s.EndDate).HasColumnName("end_date").HasColumnType("date");
        builder.Property(s => s.NextExecutionDate).HasColumnName("next_execution_date").HasColumnType("date");
        builder.Property(s => s.LastExecutionDate).HasColumnName("last_execution_date");
        
        builder.Property(s => s.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETUTCDATE()");

        // İlişkiler
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.FromAccount)
            .WithMany()
            .HasForeignKey(s => s.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.ToAccount)
            .WithMany()
            .HasForeignKey(s => s.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}