using Microsoft.EntityFrameworkCore;
using PosApi.Data;
using PosApi.Models;

namespace PosApi.Repositories;

public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
{
    public PromotionRepository(PosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
    {
        return await _dbSet.Where(p => p.IsActive).ToListAsync();
    }

    
}
