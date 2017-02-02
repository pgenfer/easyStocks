using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.ViewModel;

namespace EasyStocks.Extension
{
    public static class FloatExtension
    {
        public static string WithSign(this float value) => 
            $"{(value > 0 ? "+" : string.Empty)}{value:N2}";

        public static RateChange GetTrend(this float value) =>
           value < 0.0 ? RateChange.Negative : value > 0.0 ? RateChange.Positive : RateChange.NoChange;
        
        public static string ToPercentStringWithSign(this float value) => $"{value.WithSign()} %";
        public static string ToStringWithSign(this float value) => value.WithSign();
    }
}
