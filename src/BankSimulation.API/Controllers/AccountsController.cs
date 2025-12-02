using BankSimulation.Infrastructure.Data;
using BankSimulation.Domain.Entities.AccountManagement;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly BankSimulationDbContext _context;

    public AccountsController(BankSimulationDbContext context)
    {
        _context = context;
    }

    // GET: api/accounts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
    {
        var accounts = await _context.Accounts.ToListAsync();
        return Ok(accounts);
    }

    // GET: api/accounts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    // GET: api/accounts/user/5
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByUser(int userId)
    {
        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();

        return Ok(accounts);
    }

    // POST: api/accounts
    [HttpPost]
    public async Task<ActionResult<Account>> CreateAccount(CreateAccountRequest request)
    {
        // Kullanıcı var mı kontrol et
        var userExists = await _context.Users.AnyAsync(u => u.UserId == request.UserId);
        if (!userExists)
        {
            return BadRequest("Kullanıcı bulunamadı.");
        }

        var account = new Account
        {
            UserId = request.UserId,
            AccountNumber = request.AccountNumber,
            AccountType = request.AccountType,
            Currency = request.Currency,
            Balance = request.Balance,
            AvailableBalance = request.Balance,
            DailyTransferLimit = request.DailyTransferLimit,
            DailyWithdrawalLimit = request.DailyWithdrawalLimit,
            InterestRate = request.InterestRate,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
    }

    // PUT: api/accounts/5/balance
    [HttpPut("{id}/balance")]
    public async Task<IActionResult> UpdateBalance(int id, [FromBody] decimal newBalance)
    {
        var account = await _context.Accounts.FindAsync(id);
        
        if (account == null)
        {
            return NotFound();
        }

        account.Balance = newBalance;
        account.AvailableBalance = newBalance;
        await _context.SaveChangesAsync();

        return Ok(account);
    }
}

// DTO - Data Transfer Object
public class CreateAccountRequest
{
    public int UserId { get; set; }
    public string AccountNumber { get; set; } = null!;
    public AccountType AccountType { get; set; }
    public Currency Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal DailyTransferLimit { get; set; }
    public decimal DailyWithdrawalLimit { get; set; }
    public decimal InterestRate { get; set; }
}