namespace EasyStocks.Model
{
    /// <summary>
    /// stores information of a stock exchange.
    /// </summary>
    public class StockExchange
    {
        public StockExchange(string name, string exchangeCode)
        {
            Name = name;
            ExchangeCode = exchangeCode;
        }

        /// <summary>
        /// Name of the stock exchange. It is used
        /// to map between the exchange code and the suffix
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Exchange code as provided by the BB API
        /// </summary>
        public string ExchangeCode { get; }
        /// <summary>
        /// suffix used for the Yahoo Financial API
        /// </summary>
        public string Suffix { get; set; }
        /// <summary>
        /// Creates a new symbol by adding the suffix of this stock exchange
        /// to the given symbol. If the stock exchange does not have
        /// a suffix, the symbol remains unchained.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public string AddSuffixToSymbol(string symbol) => string.IsNullOrEmpty(Suffix) ? symbol : $"{symbol}.{Suffix}";
    }
}