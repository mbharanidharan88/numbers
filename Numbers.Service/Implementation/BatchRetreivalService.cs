using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Numbers.Database;
using Numbers.Models.DTO;
using Numbers.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Numbers.Service.Implementation
{
    public class BatchRetreivalService : IBatchRetreivalService
    {
        private readonly ILogger<BatchRetreivalService> _logger;
        private readonly NumbersDbContext _dbContext;

        public BatchRetreivalService(ILogger<BatchRetreivalService> logger, 
                                    NumbersDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IList<GeneratorBatch>> GetAllBatches()
        {
            //var aaa =  await _dbContext.LastBatch.ToListAsync();

            return null;
        }
    }
}
