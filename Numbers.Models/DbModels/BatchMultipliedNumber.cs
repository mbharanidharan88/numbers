using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DbModels
{
    public class BatchMultipliedNumber : BaseEntity
    {
        public int MultipliedNumber { get; set; }

        public BatchDetail BatchDetail { get; set; }
    }
}
