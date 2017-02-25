using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EasyStocks.Model
{
    /// <summary>
    /// problem is we use the bloomberg API to lookup the symbol 
    /// (because we can then use the ISIN for search)
    /// and then we have to find the stock by its symbol via Yahoo Financial API.
    /// For this we get the exchange code from bloomberg, lookup 
    /// the name of the stock exchange in our data table and take the
    /// suffix for this stockexchange. We then add the suffix to the
    /// Yahoo-Symbol and have the correct stock ticker then.
    /// Example:
    /// BB API returns symbol "SIE" and exchange code "GF" (Frankfurt Stock Exchange).
    /// We use the exchange with the same name in our Yahoo Stock list. We find the following entry:
    /// Name = "Frankfurt Stock Exchange", Suffix = "F".
    /// We add the suffix to the symbol which results in "SIE.F". This is the symbol name
    /// we can provide to the Yahoo financial API. 
    /// </summary>
    public class StockExchangeFinder
    {
        public StockExchangeFinder()
        {
            ReadStockExchanges();
        }

        /// <summary>
        /// returns the stock exchange for the given exchange code or null 
        /// if no stock exchange is available for this code.
        /// </summary>
        /// <param name="exchangeCode"></param>
        /// <returns></returns>
        public StockExchange FindByExchangeCode(string exchangeCode)
        {
            StockExchange stockExchange;
            _stocksByExchangeCode.TryGetValue(exchangeCode, out stockExchange);
            return stockExchange;
        }

        /// <summary>
        /// stores all stock exchanges organized by their code.
        /// </summary>
        private readonly Dictionary<string,StockExchange> _stocksByExchangeCode = new Dictionary<string, StockExchange>();

        /// <summary>
        /// reads the BB exchange codes and names
        /// and then the Yahoo names and suffixes and joins all together in a dictionary.
        /// </summary>
        private void ReadStockExchanges()
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            const string resourceNameExchangeCodes = "EasyStocks.Data.bb_exchange_codes.csv";
            var content = new List<string>();

            using (var stream = assembly.GetManifestResourceStream(resourceNameExchangeCodes))
            {
                using (var reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                        content.Add(reader.ReadLine());
                }
            }
            // get stock exchanges by name and exchange code
            var stockExchangesByName = content
                .Skip(2)
                .Select(x => x.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                .Where(x => x.Length > 0)
                .ToDictionary(x => x[0],x => new StockExchange(x[0],x[3])); // [0] = Name, [3] = exchange code

            // now read list with stock names and suffixes 
            const string resourceNameExchangeSuffixes = "EasyStocks.Data.yahoo_exchange_suffixes.csv";
            content = new List<string>();

            using (var stream = assembly.GetManifestResourceStream(resourceNameExchangeSuffixes))
            {
                using (var reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                        content.Add(reader.ReadLine());
                }
            }
            // get stock exchanges by name and exchange code
            foreach (var stockSuffix in content.Select(x => x.Split(new [] {';'},StringSplitOptions.RemoveEmptyEntries)))
            {
                var name = stockSuffix[0].Trim();
                var suffix = stockSuffix[1].Trim().Replace("N/A", string.Empty).Replace(".",string.Empty);
                StockExchange stockExchange;
                if (stockExchangesByName.TryGetValue(name, out stockExchange))
                    stockExchange.Suffix = suffix;
            }

            // exchange codes are not unique, so in case we have duplicates, we simply take the first one
            foreach(var exchange in stockExchangesByName.Values)
                if (!_stocksByExchangeCode.ContainsKey(exchange.ExchangeCode))
                    _stocksByExchangeCode.Add(exchange.ExchangeCode, exchange);
        }
    }
}