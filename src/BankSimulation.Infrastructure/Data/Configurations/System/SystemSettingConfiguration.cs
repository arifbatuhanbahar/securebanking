using BankSimulation.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.System;

public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("system_settings");

        builder.HasKey(s => s.SettingId);
        builder.Property(s => s.SettingId).HasColumnName("setting_id");

        // Ayar Anahtarı (UNIQUE) - Aynı ayardan iki tane olamaz
        builder.Property(s => s.SettingKey).HasColumnName("setting_key").HasMaxLength(100).IsRequired();
        builder.HasIndex(s => s.SettingKey).IsUnique();

        builder.Property(s => s.SettingValue).HasColumnName("setting_value").HasMaxLength(500).IsRequired();
        
        builder.Property(s => s.SettingType).HasColumnName("setting_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(255);
        
        // Yerel Saat
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
        builder.Property(s => s.UpdatedBy).HasColumnName("updated_by");

        // İlişki
        builder.HasOne(s => s.UpdatedByUser)
            .WithMany()
            .HasForeignKey(s => s.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}