using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.Handlers
{
    public class MultiplyBatchEventArgs : EventArgs
    {
        public int BatchNumber { get; set; }
        public int MultipliedNumber { get; set; }
    }
}
