using PosApi.DTOs;
using PosApi.Models;
using PosApi.Repositories;

namespace PosApi.Services;

public class PurchaseService : IPurchaseService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<PurchaseService> _logger;

    public PurchaseService(ITransactionRepository transactionRepository, ILogger<PurchaseService> logger)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<PurchaseResponseDto> ProcessPurchaseAsync(PurchaseRequestDto request)
    {
        _logger.LogInformation("Processing purchase for customer: {CustomerId}, Amount: {Amount}", 
            request.CustomerId, request.Amount);

        // Simulate random service failure
        if (Random.Shared.Next(100) < 5) // 5% chance of failure
        {
            _logger.LogError("Random service failure during purchase processing");
            return new PurchaseResponseDto
            {
                Success = false,
                Message = "Service temporarily unavailable"
            };
        }

        // Simulate timeout
        if (request.Amount > 5000)
        {
            _logger.LogWarning("Purchase amount exceeds threshold, simulating timeout");
            await Task.Delay(2000); // Simulate slow processing
        }

        try
        {
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString().Substring(0, 12),
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase successful: {TransactionId}", transaction.TransactionId);

            return new PurchaseResponseDto
            {
                Success = true,
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Message = "Purchase processed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase");
            return new PurchaseResponseDto
            {
                Success = false,
                Message = "Failed to process purchase"
            };
        }
    }
}
