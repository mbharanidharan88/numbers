using Numbers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DTO
{
    public class BatchRequest
    {
        public string BatchId { get; set; } = Guid.NewGuid().ToString();

        public int NumberOfBatches { get; set; }

        public int NumbersPerBatch { get; set; }

        public BatchStatus CurrentStatus { get; set; }
    }
}
