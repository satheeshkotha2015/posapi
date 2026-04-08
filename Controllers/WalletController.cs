using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosApi.DTOs;
using PosApi.Services;

namespace PosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ILogger<WalletController> _logger;

    public WalletController(IWalletService walletService, ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _logger = logger;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<WalletResponseDto>> GetWallet(string customerId)
    {
        _logger.LogInformation("Get wallet request for customer: {CustomerId}", customerId);

        var wallet = await _walletService.GetWalletAsync(customerId);
        if (wallet == null)
        {
            return NotFound(new { message = "Wallet not found" });
        }

        return Ok(wallet);
    }

    [HttpPost("credit")]
    public async Task<ActionResult<WalletResponseDto>> Credit([FromBody] WalletOperationDto request)
    {
        _logger.LogInformation("Wallet credit request for customer: {CustomerId}, Amount: {Amount}", 
            request.CustomerId, request.Amount);

        if (request.Amount <= 0)
        {
            return BadRequest(new { message = "Amount must be greater than 0" });
        }

        var wallet = await _walletService.CreditAsync(request.CustomerId, request.Amount);
        return Ok(wallet);
    }

    [HttpPost("debit")]
    public async Task<ActionResult<WalletResponseDto>> Debit([FromBody] WalletOperationDto request)
    {
        _logger.LogInformation("Wallet debit request for customer: {CustomerId}, Amount: {Amount}", 
            request.CustomerId, request.Amount);

        if (request.Amount <= 0)
        {
            return BadRequest(new { message = "Amount must be greater than 0" });
        }

        var wallet = await _walletService.DebitAsync(request.CustomerId, request.Amount);
        if (wallet == null)
        {
            return BadRequest(new { message = "Insufficient balance or wallet not found" });
        }

        return Ok(wallet);
    }
}
