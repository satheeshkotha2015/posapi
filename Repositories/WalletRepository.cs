using Microsoft.EntityFrameworkCore;
using PosApi.Data;
using PosApi.Models;

namespace PosApi.Repositories;

public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
{
    public WalletRepository(PosDbContext context) : base(context)
    {
    }

    public async Task<Wallet?> GetByCustomerIdAsync(string customerId)
    {
        return await _dbSet.FirstOrDefaultAsync(w => w.CustomerId == customerId);
    }
}
