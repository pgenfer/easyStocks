using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks
{
    /// <summary>
    /// the portfolio is the container
    /// where all shares of the user are stored.
    /// </summary>
    public class Portfolio
    {
        /// <summary>
        /// stores the account items by their share symbol.
        /// Share symbol should be unique, so a hash map can be used.
        /// </summary>
        private readonly Dictionary<Share,AccountItem> _items = new Dictionary<Share,AccountItem>();

        public virtual void AddShare(Share newShare, DateTime buyingDate, float buyingRate, float stopQuote)
        {
            AccountItem item;
            if (!_items.TryGetValue(newShare, out item))
                _items.Add(newShare, new AccountItem(newShare, buyingDate,buyingRate,stopQuote));
        }

        public virtual void AddShare(Share newShare, DateTime buyingDate)
        {
            AccountItem item;
            if(!_items.TryGetValue(newShare, out item))
                _items.Add(newShare,new AccountItem(newShare,buyingDate));
            // TODO: show message in case item would be added twice
        }

        public virtual IEnumerable<AccountItem> Items => _items.Values;
        public void Clear() => _items.Clear();

        public async Task UpdatePortfolioItemsAsync(IStockTicker ticker)
        {
            foreach (var accountItem in _items.Values)
            {
                // TODO: if share could not be loaded, try to load it again, then add daily data
                var newDailyData = await ticker.GetDailyInformationForShareAsync(accountItem.Share);
                if(newDailyData.IsSuccessful)
                   accountItem.UpdateDailyData(newDailyData.Value);
                // TODO: show invalid state of account item because data could not be retrieved.
            }
        }
    }
}
