using Numbers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DTO
{
    public class BatchResponse
    {
        public BatchStatus CurrentStatus { get; set; }

        public List<GeneratorBatch> ResponseData { get; set; } = new List<GeneratorBatch>();

        public int BatchesTotal { get; set; }
    }
}
