using System.Text.RegularExpressions;

namespace EasyStocks.Model
{
    /// <summary>
    /// represents the procentual change of a quote
    /// </summary>
    public class QuoteChangePercent : QuoteChange
    {
        public const string NoChange = "0.00 %";
        // string can be in the form + 1.34 % or -22.32% 
        private static readonly Regex _PercentageString = new Regex(@"([+|-]?)\s*(\d+\.\d+)\s*%");
        public QuoteChangePercent(string percentAsString):base(percentAsString, _PercentageString)
        {
        }
    }
}