using System.Text.RegularExpressions;

namespace EasyStocks.Model
{
    public class QuoteChangeAbsolute : QuoteChange
    {
        public const string NoChange = "0.00";
        // string can be in the form + 1.34 % or -22.32% 
        private static readonly Regex _ChangeString = new Regex(@"([+|-]?)\s*(\d+\.\d+)\s*");
        public QuoteChangeAbsolute(string absoluteString):base(absoluteString, _ChangeString)
        {
        }
    }
}