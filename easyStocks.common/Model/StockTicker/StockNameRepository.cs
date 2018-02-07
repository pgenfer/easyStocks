using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Model.StockTicker
{
    public class StockNameRepository
    {
        /// <summary>
        /// the openfigi service only has a limited quota, so we store
        /// the names we already retrieved here
        /// </summary>
        private readonly Dictionary<string, string> _nameBySymbol = new Dictionary<string, string>();

        public void AddNameForSymbol(string symbol, string name)
        {
            if(!_nameBySymbol.ContainsKey(symbol))
                _nameBySymbol.Add(symbol, name);
        }

        public string GetNameBySymbol(string symbol)
        {
            var name = string.Empty;
            if (_nameBySymbol.TryGetValue(symbol, out name))
                return name;
            return null;
        }

        internal void RegisterNames(IEnumerable<ReadonlyAccountItem> allAccountItems)
        {
            foreach (var item in allAccountItems)
                if(!_nameBySymbol.ContainsKey(item.Symbol))
                    _nameBySymbol.Add(item.Symbol, item.ShareName);
        }
    }
}
