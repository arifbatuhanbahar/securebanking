using BankSimulation.Domain.Entities.System;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public SystemController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // GET: api/system/settings
    [HttpGet("settings")]
    public async Task<ActionResult<IEnumerable<SystemSetting>>> GetSettings()
    {
        return await _context.SystemSettings.ToListAsync();
    }

    // POST: api/system/settings
    // Ayar güncelle veya yoksa oluştur
    [HttpPost("settings")]
    public async Task<ActionResult<SystemSetting>> UpdateSetting(string key, string value, string? description)
    {
        var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == key);

        if (setting == null)
        {
            setting = new SystemSetting
            {
                SettingKey = key,
                SettingValue = value,
                SettingType = SettingType.String,
                Description = description,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1 // Admin
            };
            _context.SystemSettings.Add(setting);
        }
        else
        {
            setting.SettingValue = value;
            setting.UpdatedAt = DateTime.Now;
            if (description != null) setting.Description = description;
        }

        await _context.SaveChangesAsync();
        return Ok(setting);
    }

    // POST: api/system/templates
    [HttpPost("templates")]
    public async Task<ActionResult<NotificationTemplate>> CreateTemplate(TemplateRequest request)
    {
        var template = new NotificationTemplate
        {
            TemplateName = request.TemplateName,
            TemplateType = request.TemplateType,
            Subject = request.Subject,
            Body = request.Body,
            Variables = request.Variables, // Örn: "['Name', 'Amount']"
            Language = "TR",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        _context.NotificationTemplates.Add(template);
        await _context.SaveChangesAsync();

        return Ok(template);
    }
}

// DTO
public class TemplateRequest
{
    public string TemplateName { get; set; } = null!;
    public TemplateType TemplateType { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; } = null!;
    public string? Variables { get; set; }
}