using PosApi.DTOs;

namespace PosApi.Services;

public interface IWalletService
{
    Task<WalletResponseDto?> GetWalletAsync(string customerId);
    Task<WalletResponseDto?> CreditAsync(string customerId, decimal amount);
    Task<WalletResponseDto?> DebitAsync(string customerId, decimal amount);
}
