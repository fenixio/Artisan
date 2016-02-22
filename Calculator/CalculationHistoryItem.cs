using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Calculator
{
    public struct CalculationHistoryItem
    {
        public string Code { get; set; }
        public double Result { get; set; }

        public CalculationHistoryItem(string code, double result) : this()
        {
            this.Code = code;
            this.Result = result;
        }
    }
}
