using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EasyStocks.Model
{
    public abstract class QuoteChange
    {
        protected QuoteChange(
            string changeAsString,
            Regex changeStringRegEx)
        {
            var match = changeStringRegEx.Match(changeAsString);
            IsValid = match.Success;
            if (IsValid)
            {
                var sign = match.Groups[1].Value;
                Value = float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                // set negative value if necessary
                if (sign == "-") Value *= -1;
            }
        }

        public bool IsValid { get; }
        public bool IsNegative => Value < 0;
        public bool IsPositive => Value > 0;
        public bool HasNoChange => Math.Abs(Value) <= 0.00;
        public float Value { get; }
        public override string ToString() => Value.ToString();
    }
}