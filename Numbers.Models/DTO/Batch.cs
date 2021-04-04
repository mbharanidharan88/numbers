using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Numbers.Models.DTO
{
    public class Batch
    {
        public string BatchId { get; set; }

        public int BatchNumber { get; set; }
        
        public int NumbersPerBatch { get; set; }

        public int Total { get; set; }
        
    }
}
