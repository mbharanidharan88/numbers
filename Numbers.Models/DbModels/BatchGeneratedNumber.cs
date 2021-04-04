using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DbModels
{
    public class BatchGeneratedNumber : BaseEntity
    {
        public int GeneratedNumber { get; set; }

        public BatchDetail BatchDetail { get; set; }
    }
}
