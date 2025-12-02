using BankSimulation.Domain.Entities.TransactionManagement;
using BankSimulation.Domain.Entities.UserManagement;
using BankSimulation.Domain.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankSimulation.Infrastructure.Data;

/// <summary>
/// Banka Simülasyonu Ana DbContext
/// </summary>
public class BankSimulationDbContext : DbContext
{
    public BankSimulationDbContext(DbContextOptions<BankSimulationDbContext> options)
        : base(options)
    {
    }
    
    // User Management Tabloları
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();
 public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountTypeDefinition> AccountTypes => Set<AccountTypeDefinition>();
    public DbSet<AccountBeneficiary> AccountBeneficiaries => Set<AccountBeneficiary>();
    public DbSet<AccountLimit> AccountLimits => Set<AccountLimit>();
// Transaction Management Tabloları
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionTypeDefinition> TransactionTypes => Set<TransactionTypeDefinition>();
    public DbSet<TransactionFee> TransactionFees => Set<TransactionFee>();
    public DbSet<ScheduledTransaction> ScheduledTransactions => Set<ScheduledTransaction>();
    public DbSet<TransactionApproval> TransactionApprovals => Set<TransactionApproval>();
    public DbSet<GeneralLedger> GeneralLedgerEntries => Set<GeneralLedger>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Tüm Configuration sınıflarını otomatik uygula
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Decimal precision (para için)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var decimalProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));
            
            foreach (var property in decimalProperties)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
    
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            var updatedAtProperty = entry.Entity.GetType().GetProperty("UpdatedAt");
            if (updatedAtProperty != null && updatedAtProperty.PropertyType == typeof(DateTime?))
            {
                updatedAtProperty.SetValue(entry.Entity, DateTime.UtcNow);
            }
            
            if (entry.State == EntityState.Added)
            {
                var createdAtProperty = entry.Entity.GetType().GetProperty("CreatedAt");
                if (createdAtProperty != null && createdAtProperty.PropertyType == typeof(DateTime))
                {
                    var currentValue = (DateTime)createdAtProperty.GetValue(entry.Entity)!;
                    if (currentValue == default)
                    {
                        createdAtProperty.SetValue(entry.Entity, DateTime.UtcNow);
                    }
                }
            }
        }
    }
}