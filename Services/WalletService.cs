using PosApi.DTOs;
using PosApi.Models;
using PosApi.Repositories;

namespace PosApi.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ILogger<WalletService> _logger;

    public WalletService(IWalletRepository walletRepository, ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<WalletResponseDto?> GetWalletAsync(string customerId)
    {
        _logger.LogInformation("Fetching wallet for customer: {CustomerId}", customerId);

        var wallet = await _walletRepository.GetByCustomerIdAsync(customerId);
        if (wallet == null)
        {
            _logger.LogWarning("Wallet not found for customer: {CustomerId}", customerId);
            return null;
        }

        return MapToDto(wallet);
    }

    public async Task<WalletResponseDto?> CreditAsync(string customerId, decimal amount)
    {
        _logger.LogInformation("Crediting wallet for customer {CustomerId} with amount {Amount}", customerId, amount);

        var wallet = await _walletRepository.GetByCustomerIdAsync(customerId);
        if (wallet == null)
        {
            wallet = new Wallet { CustomerId = customerId, Balance = 0 };
            await _walletRepository.AddAsync(wallet);
        }

        wallet.Balance += amount;
        await _walletRepository.UpdateAsync(wallet);
        await _walletRepository.SaveChangesAsync();

        _logger.LogInformation("Wallet credited successfully. New balance: {Balance}", wallet.Balance);
        return MapToDto(wallet);
    }

    public async Task<WalletResponseDto?> DebitAsync(string customerId, decimal amount)
    {
        _logger.LogInformation("Debiting wallet for customer {CustomerId} with amount {Amount}", customerId, amount);

        var wallet = await _walletRepository.GetByCustomerIdAsync(customerId);
        if (wallet == null)
        {
            _logger.LogWarning("Wallet not found for customer: {CustomerId}", customerId);
            return null;
        }

        if (wallet.Balance < amount)
        {
            _logger.LogWarning("Insufficient balance for customer {CustomerId}. Balance: {Balance}, Required: {Amount}", 
                customerId, wallet.Balance, amount);
            return null;
        }

        wallet.Balance -= amount;
        await _walletRepository.UpdateAsync(wallet);
        await _walletRepository.SaveChangesAsync();

        _logger.LogInformation("Wallet debited successfully. New balance: {Balance}", wallet.Balance);
        return MapToDto(wallet);
    }

    private static WalletResponseDto MapToDto(Wallet wallet)
    {
        return new WalletResponseDto
        {
            Id = wallet.Id,
            CustomerId = wallet.CustomerId,
            Balance = wallet.Balance
        };
    }
}
