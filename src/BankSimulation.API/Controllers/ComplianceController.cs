using BankSimulation.Domain.Entities.Compliance;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplianceController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public ComplianceController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // POST: api/compliance/upload-document
    [HttpPost("upload-document")]
    public async Task<ActionResult<KycDocument>> UploadDocument(KycDocumentRequest request)
    {
        var document = new KycDocument
        {
            UserId = request.UserId,
            DocumentType = request.DocumentType,
            DocumentNumber = request.DocumentNumber,
            // Gerçek senaryoda dosya sunucuya yüklenir, burada yolunu simüle ediyoruz
            DocumentFilePath = $"/uploads/users/{request.UserId}/{Guid.NewGuid()}.pdf",
            DocumentHash = "A1B2C3D4...", // Dosya imzasını simüle ediyoruz
            UploadDate = DateTime.Now,
            VerificationStatus = VerificationStatus.Pending
        };

        _context.KycDocuments.Add(document);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Belge yüklendi, onay bekleniyor.", DocumentId = document.DocumentId });
    }

    // POST: api/compliance/verify-document
    [HttpPost("verify-document")]
    public async Task<IActionResult> VerifyDocument(int documentId, bool isApproved, string? rejectionReason)
    {
        var document = await _context.KycDocuments.FindAsync(documentId);
        if (document == null) return NotFound("Belge bulunamadı.");

        document.VerificationStatus = isApproved ? VerificationStatus.Verified : VerificationStatus.Rejected;
        document.VerifiedAt = DateTime.Now;
        document.RejectionReason = rejectionReason;
        document.VerifiedBy = 1; // Sistem Admin (Simüle)

        // Eğer onaylandıysa kullanıcının KYC statüsünü güncelle
        if (isApproved)
        {
            var user = await _context.Users.FindAsync(document.UserId);
            if (user != null) user.KycStatus = KycStatus.Verified;
        }

        await _context.SaveChangesAsync();
        return Ok($"Belge durumu güncellendi: {document.VerificationStatus}");
    }

    // POST: api/compliance/report-suspicious-activity
    [HttpPost("report-suspicious-activity")]
    public async Task<ActionResult<SuspiciousActivityReport>> ReportActivity(SarRequest request)
    {
        var report = new SuspiciousActivityReport
        {
            UserId = request.UserId,
            TransactionId = request.TransactionId,
            ReportType = request.ReportType,
            Description = request.Description,
            RiskScore = 85, // Yüksek risk simülasyonu
            Status = SarStatus.Draft,
            CreatedAt = DateTime.Now,
            CreatedBy = 1 // Sistem/Personel ID
        };

        _context.SuspiciousActivityReports.Add(report);

        // Otomatik MASAK kaydı oluştur (Yüksek riskli ise)
        if (report.RiskScore > 80)
        {
            var masakRecord = new MasakRecord
            {
                CustomerId = request.UserId,
                TransactionId = request.TransactionId,
                RecordType = MasakRecordType.SuspiciousReport,
                Data = $"{{ 'reason': '{request.Description}', 'risk_score': 85 }}",
                CreatedAt = DateTime.Now,
                RetentionUntil = DateTime.Now.AddYears(10) // 10 yıl saklama zorunluluğu
            };
            _context.MasakRecords.Add(masakRecord);
            report.ReportedToMasak = true;
            report.MasakReportDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Şüpheli işlem bildirildi ve MASAK kaydı oluşturuldu.", ReportId = report.SarId });
    }
}

// DTOs
public class KycDocumentRequest
{
    public int UserId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = null!;
}

public class SarRequest
{
    public int UserId { get; set; }
    public int? TransactionId { get; set; }
    public SarReportType ReportType { get; set; }
    public string Description { get; set; } = null!;
}