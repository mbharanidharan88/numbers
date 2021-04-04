using Numbers.Models.DbModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Numbers.Database
{
    public interface IBatchRepository
    {
        Task<IEnumerable<BatchDetail>> BatchDetails { get; }

        Task<BatchDetail> LastBatch { get; }

        Task AddAsync(BatchDetail batchDetail);
        Task AddRangeAsync(IEnumerable<BatchDetail> entities);
        Task SaveChangesAsync();
    }
}