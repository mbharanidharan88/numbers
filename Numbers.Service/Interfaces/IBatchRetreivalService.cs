using Numbers.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Numbers.Service.Interfaces
{
    public interface IBatchRetreivalService
    {
        Task<IList<GeneratorBatch>> GetAllBatches();
    }
}
