using PosApi.DTOs;
using PosApi.Models;
using PosApi.Repositories;

namespace PosApi.Services;

public class CashbackService : ICashbackService
{
    private readonly ICashbackRequestRepository _cashbackRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<CashbackService> _logger;

    public CashbackService(ICashbackRequestRepository cashbackRepository, 
        ITransactionRepository transactionRepository, ILogger<CashbackService> logger)
    {
        _cashbackRepository = cashbackRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<CashbackResponseDto> RequestCashbackAsync(CashbackRequestDto request)
    {
        _logger.LogInformation("Processing cashback request for transaction {TransactionId}, Amount: {Amount}", 
            request.TransactionId, request.Amount);

        try
        {
            var transaction = await _transactionRepository.GetByTransactionIdAsync(request.TransactionId);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found for cashback: {TransactionId}", request.TransactionId);
                return new CashbackResponseDto
                {
                    Success = false,
                    Status = "Invalid",
                    Message = "Transaction not found"
                };
            }

            var existingRequest = await _cashbackRepository.GetByTransactionIdAsync(request.TransactionId);
            if (existingRequest != null)
            {
                _logger.LogInformation("Cashback already requested for transaction {TransactionId}", request.TransactionId);
                return new CashbackResponseDto
                {
                    Success = false,
                    Status = existingRequest.Status.ToString(),
                    Message = "Cashback already requested for this transaction"
                };
            }

            var cashbackRequest = new CashbackRequest
            {
                TransactionId = request.TransactionId,
                Amount = request.Amount,
                Status = CashbackStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _cashbackRepository.AddAsync(cashbackRequest);
            await _cashbackRepository.SaveChangesAsync();

            // Simulate EPS approval with delay
            _ = ApproveEpsAsync(cashbackRequest.Id);

            _logger.LogInformation("Cashback request created with ID {CashbackId}, status: Pending", cashbackRequest.Id);

            return new CashbackResponseDto
            {
                Success = true,
                Status = "Pending",
                CreatedAt = cashbackRequest.CreatedAt,
                Message = "Cashback request submitted for approval (simulated EPS processing)"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing cashback request");
            return new CashbackResponseDto
            {
                Success = false,
                Status = "Error",
                Message = "Failed to process cashback request"
            };
        }
    }

    private async Task ApproveEpsAsync(int cashbackRequestId)
    {
        // Simulate EPS approval delay (5 seconds)
        await Task.Delay(5000);

        var cashbackRequest = await _cashbackRepository.GetByIdAsync(cashbackRequestId);
        if (cashbackRequest != null)
        {
            // 90% approval rate
            cashbackRequest.Status = Random.Shared.Next(100) < 90 ? 
                CashbackStatus.Approved : CashbackStatus.Rejected;

            await _cashbackRepository.UpdateAsync(cashbackRequest);
            await _cashbackRepository.SaveChangesAsync();

            _logger.LogInformation("Cashback request {CashbackId} approved by EPS with status: {Status}", 
                cashbackRequestId, cashbackRequest.Status);
        }
    }
}
