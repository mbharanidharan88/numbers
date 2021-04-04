using Numbers.Models.DbModels;
using Numbers.Models.DTO;
using Numbers.Models.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Numbers.Worker
{
    public interface IProcessor
    {

        BatchRequest BatchRequest { get; set; }

        BatchStatus CurrentStatus { get; set; }

        ConcurrentDictionary<int, GeneratorBatch> BatchDetails { get; set; }

        Task<BatchResponse> GetSessionData(string batchId);

        Task<string> StartProcess(BatchRequest batchDetail);

        Task<bool> Clear();

        Task<IEnumerable<BatchDetail>> GetLastBatchDetails();
    }
}