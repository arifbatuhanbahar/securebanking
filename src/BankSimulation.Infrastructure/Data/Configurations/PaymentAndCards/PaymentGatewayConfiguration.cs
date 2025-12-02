using BankSimulation.Domain.Entities.PaymentAndCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.PaymentAndCards;

public class PaymentGatewayConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
        builder.ToTable("payment_gateways");

        builder.HasKey(pg => pg.GatewayId);
        builder.Property(pg => pg.GatewayId).HasColumnName("gateway_id");

        builder.Property(pg => pg.GatewayName).HasColumnName("gateway_name").HasMaxLength(100).IsRequired();
        builder.Property(pg => pg.GatewayType).HasColumnName("gateway_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        
        builder.Property(pg => pg.ApiEndpoint).HasColumnName("api_endpoint").HasMaxLength(255).IsRequired();
        builder.Property(pg => pg.ApiKeyEncrypted).HasColumnName("api_key_encrypted").HasMaxLength(512).IsRequired();
        
        builder.Property(pg => pg.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        
        // Yerel Saat
        builder.Property(pg => pg.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
    }
}