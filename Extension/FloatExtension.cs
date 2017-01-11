using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Extension
{
    public static class FloatExtension
    {
        public static string WithSign(this float value) => 
            $"{(value > 0 ? "+" : string.Empty)}{value:N2}";
    }
}
