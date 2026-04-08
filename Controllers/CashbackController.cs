using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosApi.DTOs;
using PosApi.Services;

namespace PosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CashbackController : ControllerBase
{
    private readonly ICashbackService _cashbackService;
    private readonly ILogger<CashbackController> _logger;

    public CashbackController(ICashbackService cashbackService, ILogger<CashbackController> logger)
    {
        _cashbackService = cashbackService;
        _logger = logger;
    }

    [HttpPost("request")]
    public async Task<ActionResult<CashbackResponseDto>> RequestCashback([FromBody] CashbackRequestDto request)
    {
        _logger.LogInformation("Cashback request - TransactionId: {TransactionId}, Amount: {Amount}", 
            request.TransactionId, request.Amount);

        if (string.IsNullOrWhiteSpace(request.TransactionId) || request.Amount <= 0)
        {
            return BadRequest(new { message = "Valid TransactionId and Amount are required" });
        }

        var response = await _cashbackService.RequestCashbackAsync(request);
        return Ok(response);
    }
}
