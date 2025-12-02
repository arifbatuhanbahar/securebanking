using BankSimulation.Domain.Entities.PaymentAndCards;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public PaymentsController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // GET: api/payments/cards/user/1
    [HttpGet("cards/user/{userId}")]
    public async Task<ActionResult<IEnumerable<CreditCard>>> GetCardsByUser(int userId)
    {
        var cards = await _context.CreditCards
            .Where(c => c.UserId == userId)
            .ToListAsync();
        return Ok(cards);
    }

    // POST: api/payments/cards
    [HttpPost("cards")]
    public async Task<ActionResult<CreditCard>> CreateCard(CreateCardRequest request)
    {
        var card = new CreditCard
        {
            UserId = request.UserId,
            // Gerçek hayatta şifrelenmeli, şimdilik simülasyon
            CardNumberEncrypted = request.CardNumber, 
            CardLastFour = request.CardNumber.Substring(request.CardNumber.Length - 4),
            CvvEncrypted = "123", // Örnek
            CardType = CardType.Physical,
            CardBrand = CardBrand.MasterCard,
            CreditLimit = request.Limit,
            AvailableLimit = request.Limit,
            CurrentBalance = 0,
            MinimumPayment = 0,
            InterestRate = 3.5m,
            ExpiryMonth = 12,
            ExpiryYear = 2030,
            PaymentDueDate = DateTime.Now.AddDays(30),
            Status = CardStatus.Active,
            IssuedAt = DateTime.Now
        };

        _context.CreditCards.Add(card);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCardsByUser), new { userId = card.UserId }, card);
    }

    // POST: api/payments/transaction
    [HttpPost("transaction")]
    public async Task<IActionResult> MakeTransaction(CardTransactionRequest request)
    {
        var card = await _context.CreditCards.FindAsync(request.CardId);
        if (card == null) return NotFound("Kart bulunamadı.");

        if (card.AvailableLimit < request.Amount)
            return BadRequest("Yetersiz limit.");

        // Limiti düşür, borcu artır
        card.AvailableLimit -= request.Amount;
        card.CurrentBalance += request.Amount;

        var transaction = new CardTransaction
        {
            CardId = request.CardId,
            MerchantName = request.MerchantName,
            MerchantCategory = "Shopping",
            Amount = request.Amount,
            Currency = Currency.TRY,
            TransactionDate = DateTime.Now,
            Status = CardTransactionStatus.Approved,
            AuthorizationCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
        };

        _context.CardTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "İşlem Başarılı", AuthCode = transaction.AuthorizationCode });
    }
}

// DTOs
public class CreateCardRequest
{
    public int UserId { get; set; }
    public string CardNumber { get; set; } = null!;
    public decimal Limit { get; set; }
}

public class CardTransactionRequest
{
    public int CardId { get; set; }
    public decimal Amount { get; set; }
    public string MerchantName { get; set; } = null!;
}