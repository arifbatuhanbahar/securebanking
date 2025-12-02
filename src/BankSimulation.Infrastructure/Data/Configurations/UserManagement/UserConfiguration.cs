using BankSimulation.Domain.Entities.UserManagement;
using BankSimulation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Data.Configurations.UserManagement;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Tablo Adı
        builder.ToTable("users");
        
        // Primary Key
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId)
            .HasColumnName("user_id");
        
        // TC Kimlik - UNIQUE
        builder.Property(u => u.TcKimlikNo)
            .HasColumnName("tc_kimlik_no")
            .HasMaxLength(11)
            .IsFixedLength()
            .IsRequired();
        
        builder.HasIndex(u => u.TcKimlikNo)
            .IsUnique()
            .HasDatabaseName("IX_users_tc_kimlik_no");
        
        // Kişisel Bilgiler
        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth")
            .HasColumnType("date")
            .IsRequired();
        
        // Email - UNIQUE
        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_users_email");
        
        builder.Property(u => u.EmailVerified)
            .HasColumnName("email_verified")
            .HasDefaultValue(false);
        
        // Telefon
        builder.Property(u => u.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20);
        
        builder.Property(u => u.PhoneVerified)
            .HasColumnName("phone_verified")
            .HasDefaultValue(false);
        
        // Adres
        builder.Property(u => u.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(255);
        
        builder.Property(u => u.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(255);
        
        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasMaxLength(100);
        
        builder.Property(u => u.PostalCode)
            .HasColumnName("postal_code")
            .HasMaxLength(10);
        
        builder.Property(u => u.Country)
            .HasColumnName("country")
            .HasMaxLength(2)
            .HasDefaultValue("TR");
        
        // Güvenlik
        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();
        
        builder.Property(u => u.PasswordSalt)
            .HasColumnName("password_salt")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(u => u.PasswordChangedAt)
            .HasColumnName("password_changed_at");
        
        // Status - Enum string olarak
        builder.Property(u => u.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(UserStatus.Active);
        
        builder.Property(u => u.FailedLoginAttempts)
            .HasColumnName("failed_login_attempts")
            .HasDefaultValue(0);
        
        builder.Property(u => u.LockedUntil)
            .HasColumnName("locked_until");
        
        // KYC & Risk
        builder.Property(u => u.KycStatus)
            .HasColumnName("kyc_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(KycStatus.Pending);
        
        builder.Property(u => u.RiskLevel)
            .HasColumnName("risk_level")
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasDefaultValue(RiskLevel.Low);
        
        builder.Property(u => u.IsPep)
            .HasColumnName("is_pep")
            .HasDefaultValue(false);
        
        // Audit
        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.Property(u => u.LastLoginAt)
            .HasColumnName("last_login_at");
        
        // Soft Delete
        builder.Property(u => u.DeletedAt)
            .HasColumnName("deleted_at");
        
        builder.Property(u => u.GdprAnonymized)
            .HasColumnName("gdpr_anonymized")
            .HasDefaultValue(false);
        
        // Query Filter - Silinmiş kayıtları otomatik filtrele
        builder.HasQueryFilter(u => u.DeletedAt == null);
        
        // Composite Index
        builder.HasIndex(u => new { u.Status, u.KycStatus })
            .HasDatabaseName("IX_users_status_kyc");
    }
}