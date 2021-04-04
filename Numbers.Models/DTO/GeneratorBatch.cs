using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.DTO
{
    public class GeneratorBatch : Batch
    {
        public IList<int> GeneratedNumbers { get; set; } = new List<int>();

        public IList<int> MultipliedNumbers { get; set; } = new List<int>();
    }
}
