using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Calculator
{
    public enum TokenType
    {
        Number,
        Delimiter,
        Variable,
        Function,
        End
    }

    public class Token
    {
        public string Value { get; set; }

        public TokenType Type { get; set; }

    }
}
