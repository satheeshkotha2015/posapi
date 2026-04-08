using PosApi.Models;

namespace PosApi.Repositories;

public interface ICashbackRequestRepository : IGenericRepository<CashbackRequest>
{
    Task<CashbackRequest?> GetByTransactionIdAsync(string transactionId);
}
