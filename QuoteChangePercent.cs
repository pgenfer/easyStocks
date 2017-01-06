using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EasyStocks
{
    /// <summary>
    /// represents the procentual change of a quote
    /// </summary>
    public class QuoteChangePercent
    {
        public const string NoChange = "0.00 %";

        private readonly float _percent;
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
                _percent = float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                // set negative value if necessary
                if (sign == "-") _percent *= -1;
            }
        }

        public bool IsNegative => _percent < 0;
        public bool IsPositive => _percent > 0;
        public bool HasNoChange => Math.Abs(_percent) <= 0.00;

        public double Value => _percent;

        public override string ToString() => $"{_percent:N2} %";
    }
}