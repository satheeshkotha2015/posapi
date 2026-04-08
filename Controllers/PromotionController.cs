using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosApi.DTOs;
using PosApi.Services;

namespace PosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PromotionController : ControllerBase
{
    private readonly IPromotionService _promotionService;
    private readonly ILogger<PromotionController> _logger;

    public PromotionController(IPromotionService promotionService, ILogger<PromotionController> logger)
    {
        _promotionService = promotionService;
        _logger = logger;
    }

    [HttpPost("apply")]
    public async Task<ActionResult<PromotionResponseDto>> ApplyPromotion([FromBody] PromotionRequestDto request)
    {
        _logger.LogInformation("Promotion apply request - TransactionId: {TransactionId}, PromotionId: {PromotionId}", 
            request.TransactionId, request.PromotionId);

        if (string.IsNullOrWhiteSpace(request.TransactionId) || request.PromotionId <= 0)
        {
            return BadRequest(new { message = "Valid TransactionId and PromotionId are required" });
        }

        var response = await _promotionService.ApplyPromotionAsync(request);
        return Ok(response);
    }
}
