using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosApi.DTOs;
using PosApi.Services;

namespace PosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    private readonly ILogger<PurchaseController> _logger;

    public PurchaseController(IPurchaseService purchaseService, ILogger<PurchaseController> logger)
    {
        _purchaseService = purchaseService;
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<ActionResult<PurchaseResponseDto>> ProcessPurchase([FromBody] PurchaseRequestDto request)
    {
        _logger.LogInformation("Purchase request received - Customer: {CustomerId}, Amount: {Amount}", 
            request.CustomerId, request.Amount);

        if (request.Amount <= 0)
        {
            return BadRequest(new { message = "Amount must be greater than 0" });
        }

        var response = await _purchaseService.ProcessPurchaseAsync(request);
        if (!response.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        return Ok(response);
    }

    [HttpPost("testpurchase")]
    public async Task<ActionResult<PurchaseResponseDto>> TestPurchase([FromBody] PurchaseRequestDto request)
    {
        _logger.LogInformation("Purchase request received - Customer: {CustomerId}, Amount: {Amount}", 
            request.CustomerId, request.Amount);

        if (request.Amount <= 0)
        {
            return BadRequest(new { message = "Amount must be greater than 0" });
        }

        var response = await _purchaseService.ProcessPurchaseAsync(request);
        if (!response.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        return Ok(response);
    }
}
