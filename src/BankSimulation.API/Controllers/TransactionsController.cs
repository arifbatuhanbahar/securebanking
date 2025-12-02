using BankSimulation.Domain.Entities.TransactionManagement;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public TransactionsController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // GET: api/transactions/account/1
    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccount(int accountId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();

        return Ok(transactions);
    }

    // POST: api/transactions/transfer
    // Bu metod ACID transaction (Bütünsel işlem) kullanır
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferMoney([FromBody] TransferRequest request)
    {
        // 1. Transaction (Veritabanı İşlemi) Başlat
        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 2. Gönderen hesabı bul ve kilitle (Bakiyeyi korumak için)
            var fromAccount = await _context.Accounts.FindAsync(request.FromAccountId);
            if (fromAccount == null) return BadRequest("Gönderen hesap bulunamadı.");

            // 3. Alıcı hesabı bul
            var toAccount = await _context.Accounts.FindAsync(request.ToAccountId);
            if (toAccount == null) return BadRequest("Alıcı hesap bulunamadı.");

            // 4. Bakiye kontrolü
            if (fromAccount.AvailableBalance < request.Amount)
                return BadRequest("Yetersiz bakiye.");

            // 5. Para Transferi İşlemleri (Bakiyeleri Güncelle)
            fromAccount.Balance -= request.Amount;
            fromAccount.AvailableBalance -= request.Amount;
            
            toAccount.Balance += request.Amount;
            toAccount.AvailableBalance += request.Amount;

            // 6. Transaction Kaydı Oluştur
            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Currency = fromAccount.Currency, // Basitlik için aynı kur kabul ediyoruz
                TransactionType = TransactionType.Transfer,
                Description = request.Description,
                ReferenceNumber = Guid.NewGuid().ToString(), // Benzersiz numara
                Status = TransactionStatus.Completed,
                TransactionDate = DateTime.UtcNow,
                RequiresReview = false,
                FraudScore = 0
            };

            _context.Transactions.Add(transaction);

            // 7. Değişiklikleri Kaydet
            await _context.SaveChangesAsync();

            // 8. İşlemi Onayla (Commit) - Buraya kadar hata olmadıysa kalıcı yap
            await dbTransaction.CommitAsync();

            return Ok(new { Message = "Transfer başarılı", TransactionId = transaction.TransactionId, Reference = transaction.ReferenceNumber });
        }
        catch (Exception ex)
        {
            // Hata olursa tüm işlemleri geri al (Rollback)
            await dbTransaction.RollbackAsync();
            return StatusCode(500, $"Transfer sırasında hata oluştu: {ex.Message}");
        }
    }
}

// DTO (Veri Transfer Objesi)
public class TransferRequest
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}