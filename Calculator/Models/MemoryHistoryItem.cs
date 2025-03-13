using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models
{
    public class MemoryHistoryItem
    {
        public double Value { get; set; }
        public string Description { get; set; }

        public MemoryHistoryItem(double value, string description = "")
        {
            Value = value;
            Description = description;
        }
    }
}
