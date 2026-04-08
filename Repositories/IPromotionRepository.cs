using PosApi.Models;

namespace PosApi.Repositories;

public interface IPromotionRepository : IGenericRepository<Promotion>
{
    Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
}
