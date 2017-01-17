using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Styles
{
    /// <summary>
    /// unfortunately, Xamarin.Forms and WPF use different color
    /// classes but instead of defining each color twice, the color
    /// values are defined here and can be resued
    /// </summary>
    public class ColorValues
    {
        public static readonly Tuple<int, int, int> Neutral = Tuple.Create(0, 0, 0);
        public static readonly Tuple<int, int, int> Ok = Tuple.Create(0x00, 0x88, 0x00);
        public static readonly Tuple<int, int, int> Failure = Tuple.Create(0xCC, 0x00, 0x00);

    }
}
