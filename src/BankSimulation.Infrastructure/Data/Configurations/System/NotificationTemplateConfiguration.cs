using BankSimulation.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.System;

public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("notification_templates");

        builder.HasKey(t => t.TemplateId);
        builder.Property(t => t.TemplateId).HasColumnName("template_id");

        builder.Property(t => t.TemplateName).HasColumnName("template_name").HasMaxLength(100).IsRequired();
        builder.Property(t => t.TemplateType).HasColumnName("template_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(t => t.Subject).HasColumnName("subject").HasMaxLength(255);
        builder.Property(t => t.Body).HasColumnName("body").HasColumnType("nvarchar(max)").IsRequired();
        
        builder.Property(t => t.Variables).HasColumnName("variables").HasColumnType("nvarchar(max)"); // JSON
        builder.Property(t => t.Language).HasColumnName("language").HasMaxLength(5).HasDefaultValue("TR");
        
        builder.Property(t => t.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        
        // Yerel Saat
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
    }
}