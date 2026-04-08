using PosApi.Models;

namespace PosApi.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<Transaction?> GetByTransactionIdAsync(string transactionId);

}
