using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Calculator
{
    public static class CharExtension
    {
        public static bool IsIn(this char c, char[] values)
        {
            int i;
            for (i = 0; i < values.Length; i++)
            {
                if (c == values[i]) break;
            }
            return i < values.Length;
        }

        public static bool IsDelim(this char c, char[] values)
        {
            return char.IsWhiteSpace(c) || c.IsIn(values);
        }
    }
}
