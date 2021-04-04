using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Models.Handlers
{
    public class GenerateBatchEventArgs : EventArgs
    {
        public int BatchNumber { get; set; }
        public int GeneratedNumber { get; set; }
    }
}
