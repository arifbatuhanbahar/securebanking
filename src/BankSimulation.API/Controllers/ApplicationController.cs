using BankSimulation.Domain.Entities.CreditCardApplications;
using BankSimulation.Domain.Entities.PaymentAndCards;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public ApplicationController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // POST: api/application/apply
    // Müşteri kart başvurusu yapar
    [HttpPost("apply")]
    public async Task<ActionResult<CardApplication>> ApplyForCard(CardApplicationRequest request)
    {
        var application = new CardApplication
        {
            UserId = request.UserId,
            CardTypeRequested = request.CardTypeRequested,
            MonthlyIncome = request.MonthlyIncome,
            EmploymentStatus = request.EmploymentStatus,
            EmployerName = request.EmployerName,
            ApplicationDate = DateTime.Now,
            Status = ApplicationStatus.Pending
        };

        _context.CardApplications.Add(application);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Başvurunuz alındı.", ApplicationId = application.ApplicationId });
    }

    // POST: api/application/approve/{id}
    // Personel başvuruyu onaylar ve limitleri belirler
    [HttpPost("approve/{applicationId}")]
    public async Task<IActionResult> ApproveApplication(int applicationId, decimal approvedLimit)
    {
        var app = await _context.CardApplications.FindAsync(applicationId);
        if (app == null) return NotFound("Başvuru bulunamadı.");

        // 1. Başvuru durumunu güncelle
        app.Status = ApplicationStatus.Approved;
        app.ApprovedBy = 1; // Admin
        app.ApprovedAt = DateTime.Now;
        app.CreditLimitApproved = approvedLimit;

        // 2. Yeni Kredi Kartı Oluştur (Otomatik)
        var newCard = new CreditCard
        {
            UserId = app.UserId,
            CardNumberEncrypted = "5555" + new Random().NextInt64(100000000000, 999999999999).ToString(),
            CardLastFour = "9999",
            CvvEncrypted = "123",
            CardType = CardType.Physical,
            CardBrand = CardBrand.Visa,
            CreditLimit = approvedLimit,
            AvailableLimit = approvedLimit,
            Status = CardStatus.Active,
            IssuedAt = DateTime.Now,
            ExpiryMonth = 12,
            ExpiryYear = 2030
        };
        
        _context.CreditCards.Add(newCard);
        await _context.SaveChangesAsync(); // Kart ID oluşsun diye önce kaydediyoruz

        // 3. Kart Limitlerini Tanımla (CardLimits Tablosu)
        var limitSettings = new List<CardLimit>
        {
            new CardLimit { CardId = newCard.CardId, LimitType = CardLimitType.TotalLimit, LimitAmount = approvedLimit, UsedAmount = 0 },
            new CardLimit { CardId = newCard.CardId, LimitType = CardLimitType.OnlineShopping, LimitAmount = approvedLimit, UsedAmount = 0 },
            new CardLimit { CardId = newCard.CardId, LimitType = CardLimitType.Contactless, LimitAmount = 750, UsedAmount = 0 }
        };

        _context.CardLimits.AddRange(limitSettings);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Başvuru onaylandı ve kart oluşturuldu.", CardId = newCard.CardId });
    }
}

// DTO
public class CardApplicationRequest
{
    public int UserId { get; set; }
    public CardPrestigeLevel CardTypeRequested { get; set; }
    public decimal MonthlyIncome { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public string EmployerName { get; set; } = null!;
}