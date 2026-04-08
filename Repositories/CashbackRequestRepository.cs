using Microsoft.EntityFrameworkCore;
using PosApi.Data;
using PosApi.Models;

namespace PosApi.Repositories;

public class CashbackRequestRepository : GenericRepository<CashbackRequest>, ICashbackRequestRepository
{
    public CashbackRequestRepository(PosDbContext context) : base(context)
    {
    }

    public async Task<CashbackRequest?> GetByTransactionIdAsync(string transactionId)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.TransactionId == transactionId);
    }
}
