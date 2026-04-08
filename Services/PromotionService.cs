using PosApi.DTOs;
using PosApi.Repositories;

namespace PosApi.Services;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<PromotionService> _logger;

    public PromotionService(IPromotionRepository promotionRepository, 
        ITransactionRepository transactionRepository, ILogger<PromotionService> logger)
    {
        _promotionRepository = promotionRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<PromotionResponseDto> ApplyPromotionAsync(PromotionRequestDto request)
    {
        _logger.LogInformation("Applying promotion {PromotionId} to transaction {TransactionId}", 
            request.PromotionId, request.TransactionId);

        try
        {
            var transaction = await _transactionRepository.GetByTransactionIdAsync(request.TransactionId);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found: {TransactionId}", request.TransactionId);
                return new PromotionResponseDto
                {
                    Applied = false,
                    Message = "Transaction not found"
                };
            }

            var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId);
            if (promotion == null || !promotion.IsActive)
            {
                _logger.LogWarning("Promotion not found or inactive: {PromotionId}", request.PromotionId);
                return new PromotionResponseDto
                {
                    Applied = false,
                    Message = "Promotion not available"
                };
            }

            if (transaction.Amount < promotion.MinAmount)
            {
                _logger.LogInformation("Transaction amount {Amount} below promotion minimum {MinAmount}", 
                    transaction.Amount, promotion.MinAmount);
                return new PromotionResponseDto
                {
                    Applied = false,
                    Message = $"Minimum purchase amount of {promotion.MinAmount} required"
                };
            }

            _logger.LogInformation("Promotion {PromotionId} applied successfully", request.PromotionId);
          

        
            return new PromotionResponseDto
            {
                Applied = true,
                DiscountAmount = promotion.DiscountAmount,
                Message = $"Promotion '{promotion.Name}' applied successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying promotion");
            return new PromotionResponseDto
            {
                Applied = false,
                Message = "Failed to apply promotion"
            };
        }
    }
}
