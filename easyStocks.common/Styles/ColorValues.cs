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
        public static readonly Tuple<int, int, int> Ok = Tuple.Create(0x9C, 0xCC, 0x65);
        public static readonly Tuple<int, int, int> Failure = Tuple.Create(0xEF, 0x53, 0x50);
        public const string PrimaryColor = "42A5F5";
        public const string AccentColor = "FFC400";
        public const string PrimaryTextColor = "DD000000"; // 87%
        public const string SecondaryTextColor = "89000000"; // 54%
        public const string HintColor = "60000000"; // 38%;
        public const string PrimaryTextLightColor = "FFFFFF";
        public const string SecondaryTextLightColor = "B2FFFFFF"; // 70%
        public const string DividerColor = "0C000000"; // 12%
    }
}
