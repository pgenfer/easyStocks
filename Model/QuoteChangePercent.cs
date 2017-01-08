using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EasyStocks.Model
{
    /// <summary>
    /// represents the procentual change of a quote
    /// </summary>
    public class QuoteChangePercent
    {
        public const string NoChange = "0.00 %";

        // string can be in the form + 1.34 % or -22.32% 
        private static readonly Regex _PercentageString = new Regex(@"([+|-]?)\s*(\d+\.\d+)\s*%");

        public bool IsValid { get; }

        public QuoteChangePercent(string percentAsString)
        {
            var match = _PercentageString.Match(percentAsString);
            IsValid = match.Success;
            if (IsValid)
            {
                var sign = match.Groups[1].Value;
                Value = float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                // set negative value if necessary
                if (sign == "-") Value *= -1;
            }
        }

        public bool IsNegative => Value < 0;
        public bool IsPositive => Value > 0;
        public bool HasNoChange => Math.Abs(Value) <= 0.00;

        public float Value { get; }

        public override string ToString() => $"{Value:N2} %";
    }
}