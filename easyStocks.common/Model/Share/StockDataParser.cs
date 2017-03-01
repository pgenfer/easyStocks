using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyStocks.Model.Share
{
    public static class StockDataParser
    {
        private static readonly Regex _ChangeString = new Regex(@"([+|-]?)\s*(\d+\.\d+)\s*");
        private static readonly Regex _PercentageString = new Regex(@"([+|-]?)\s*(\d+\.\d+)\s*%");

        private static float Parse(Regex regex, string text)
        {
            var match = regex.Match(text);
            var isValid = match.Success;
            if (!isValid)
                return 0.0f;
            var sign = match.Groups[1].Value;
            var value = float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            // set negative value if necessary
            if (sign == "-") value *= -1;
            return value;
        }

        public static float DailyChangeFromString(string changeAsString) => Parse(_ChangeString, changeAsString);
        public static float DailyChangeInPercentFromString(string changePercentAsString)=> Parse(_PercentageString, changePercentAsString);
    }
}
