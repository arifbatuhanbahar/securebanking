using BankSimulation.Domain.Entities.Fraud;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FraudController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public FraudController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // POST: api/fraud/rules
    // Yeni bir dolandırıcılık kuralı ekle
    [HttpPost("rules")]
    public async Task<ActionResult<FraudRule>> CreateRule(FraudRuleRequest request)
    {
        var rule = new FraudRule
        {
            RuleName = request.RuleName,
            RuleType = request.RuleType,
            RuleDescription = request.Description,
            RuleConditions = request.Conditions, // Örn: "{ 'max_amount': 50000 }"
            RiskScoreWeight = request.RiskWeight,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        _context.FraudRules.Add(rule);
        await _context.SaveChangesAsync();

        return Ok(rule);
    }

    // POST: api/fraud/check-transaction
    // Bir işlemi simüle et ve risk kontrolü yap
    [HttpPost("check-transaction")]
    public async Task<ActionResult<FraudAlert>> CheckTransaction(int transactionId, int userId, decimal amount)
    {
        // 1. Aktif kuralları getir
        var rules = await _context.FraudRules.Where(r => r.IsActive).ToListAsync();
        
        int totalRiskScore = 0;
        List<string> triggeredRules = new List<string>();

        // 2. Basit Kural Motoru (Simülasyon)
        foreach (var rule in rules)
        {
            // Örnek: Eğer kural "AmountAnomaly" ise ve tutar 50.000'den büyükse
            if (rule.RuleType == RuleType.AmountAnomaly && amount > 50000)
            {
                totalRiskScore += rule.RiskScoreWeight;
                triggeredRules.Add(rule.RuleName);
            }
            // Örnek: Eğer kural "Velocity" ise (Burada veritabanından son işlemleri saymamız gerekir, şimdilik simüle ediyoruz)
            // ...
        }

        // 3. Eğer risk skoru 0'dan büyükse Alarm Oluştur
        if (totalRiskScore > 0)
        {
            var alert = new FraudAlert
            {
                UserId = userId,
                TransactionId = transactionId,
                FraudScore = totalRiskScore,
                TriggeredRules = string.Join(", ", triggeredRules),
                AlertSeverity = totalRiskScore > 80 ? AlertSeverity.Critical : AlertSeverity.High,
                Status = AlertStatus.Open,
                CreatedAt = DateTime.Now
            };

            _context.FraudAlerts.Add(alert);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "RİSKLİ İŞLEM TESPİT EDİLDİ!", Alert = alert });
        }

        return Ok(new { Message = "İşlem güvenli görünüyor.", RiskScore = 0 });
    }
}

// DTOs
public class FraudRuleRequest
{
    public string RuleName { get; set; } = null!;
    public RuleType RuleType { get; set; }
    public string Description { get; set; } = null!;
    public string Conditions { get; set; } = null!;
    public int RiskWeight { get; set; }
}