using PosApi.Models;

namespace PosApi.Repositories;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetByCustomerIdAsync(string customerId);
}
