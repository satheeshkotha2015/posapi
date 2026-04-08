using Microsoft.EntityFrameworkCore;
using PosApi.Data;
using PosApi.Models;

namespace PosApi.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(PosDbContext context) : base(context)
    {
    }

    public async Task<Transaction?> GetByTransactionIdAsync(string transactionId)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
    }
    
}
