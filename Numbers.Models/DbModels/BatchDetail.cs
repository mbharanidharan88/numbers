using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DbModels
{
    public class BatchDetail : BaseEntity
    {
        public string BatchId { get; set; }

        public int NumberOfBatches { get; set; }

        public int NumberPerBatches { get; set; }

        public int BatchTotal { get; set; }

        public virtual ICollection<BatchGeneratedNumber> GeneratedNumbers { get; set; }

        public virtual ICollection<BatchMultipliedNumber> MultipliedNumbers { get; set; }
    }
}
