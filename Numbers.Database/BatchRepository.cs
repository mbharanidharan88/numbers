using Numbers.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Numbers.Database
{
    public class BatchRepository : BaseRepository<BatchDetail>, IBatchRepository
    {
        public BatchRepository(NumbersDbContext context) : base(context) { }

        //Task<IEnumerable<BatchDetail>> IBatchRepository.BatchDetails => GetAll(include: source => source.Include(a => a.GeneratedNumbers).Include(a => a.MultipliedNumbers));
        Task<IEnumerable<BatchDetail>> IBatchRepository.BatchDetails => GetAll();

        Task<BatchDetail> LastBatch { get; }
    }
}
