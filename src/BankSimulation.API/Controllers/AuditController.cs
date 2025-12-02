using BankSimulation.Domain.Entities.Audit;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public AuditController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // POST: api/audit/security-event
    // Örnek: Hatalı şifre girişi simülasyonu
    [HttpPost("security-event")]
    public async Task<ActionResult<SecurityEvent>> LogSecurityEvent(SecurityEventRequest request)
    {
        var securityEvent = new SecurityEvent
        {
            EventType = request.EventType,
            UserId = request.UserId,
            Severity = request.Severity,
            Description = request.Description,
            IpAddress = "192.168.1.10", // Simüle IP
            UserAgent = "Mozilla/5.0 (Windows NT 10.0)",
            EventDate = DateTime.Now,
            Resolved = false
        };

        _context.SecurityEvents.Add(securityEvent);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Güvenlik olayı loglandı.", EventId = securityEvent.EventId });
    }

    // GET: api/audit/security-events
    [HttpGet("security-events")]
    public async Task<ActionResult<IEnumerable<SecurityEvent>>> GetSecurityEvents()
    {
        var events = await _context.SecurityEvents
            .OrderByDescending(e => e.EventDate)
            .Take(10)
            .ToListAsync();

        return Ok(events);
    }
    
    // POST: api/audit/access-log
    // Örnek: Bir personelin müşterinin maaş bilgisine bakması
    [HttpPost("access-log")]
    public async Task<IActionResult> LogDataAccess(int staffUserId, int customerUserId, string reason)
    {
        var accessLog = new DataAccessLog
        {
            AccessedByUserId = staffUserId,
            TargetUserId = customerUserId,
            DataType = DataType.Financial,
            AccessReason = reason,
            AccessTimestamp = DateTime.Now,
            IpAddress = "10.0.0.5",
            IsSensitive = true
        };

        _context.DataAccessLogs.Add(accessLog);
        await _context.SaveChangesAsync();

        return Ok("Veri erişimi kayıt altına alındı.");
    }
}

// DTOs
public class SecurityEventRequest
{
    public SecurityEventType EventType { get; set; }
    public int? UserId { get; set; }
    public Severity Severity { get; set; }
    public string Description { get; set; } = null!;
}