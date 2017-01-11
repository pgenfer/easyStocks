using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyStocks.Model
{
    /// <summary>
    /// the portfolio is the container
    /// where all shares of the user are stored.
    /// </summary>
    public class Portfolio
    {
        /// <summary>
        /// used to synchronize the access to the internal items
        /// </summary>
        private readonly object _itemsAccessLock = new object();
        /// <summary>
        /// stores the account items by their share symbol.
        /// Share symbol should be unique, so a hash map can be used.
        /// </summary>
        private readonly Dictionary<Share, AccountItem> _items = new Dictionary<Share, AccountItem>();

        /// <summary>
        /// event is fired whenever the account data has changed
        /// </summary>
        public event Action<AccountItem> AccountDataChanged;

        /// <summary>
        /// add a share not by user but by reading it from the persistent storage
        /// </summary>
        /// <param name="newShare"></param>
        /// <param name="buyingDate"></param>
        /// <param name="buyingRate"></param>
        /// <param name="stopQuote"></param>
        public virtual void AddShareFromPersistantStorage(
            Share newShare, 
            DateTime buyingDate, 
            float buyingRate, 
            float stopQuote)
        {
            // when loading from persistent storage,
            // it should not be necessary to lock the items collection
            // because the timer should stop during load operation
            AccountItem item;
            if (!_items.TryGetValue(newShare, out item))
            {
                item = new AccountItem(newShare, buyingDate, buyingRate, stopQuote);
                _items.Add(newShare, item);
                // in case the stop quote of the account item has 
                // changed, raise an event 
                item.StopQuoteRaised += (_,__) => AccountDataChanged?.Invoke(item);
            }
        }

        /// <summary>
        /// event will be fired whenever an account item is added to the portfolio
        /// </summary>
        public event Action<AccountItem> AccountItemAdded;

        public virtual void AddShare(Share newShare, DateTime buyingDate)
        {
            lock (_itemsAccessLock)
            {
                AccountItem item;
                if (!_items.TryGetValue(newShare, out item))
                {
                    item = new AccountItem(newShare, buyingDate);
                    _items.Add(newShare, item);
                    AccountItemAdded?.Invoke(item);
                    item.StopQuoteRaised += (_, __) => AccountDataChanged?.Invoke(item);
                }
            }
            // TODO: show message in case item would be added twice
        }

        public virtual IEnumerable<AccountItem> Items => _items.Values.ToList();
        public void Clear() => _items.Clear();

        public async Task UpdatePortfolioItemsAsync(IStockTicker ticker)
        {
            List<AccountItem> itemsCopy;
            lock (_itemsAccessLock)
            {
                itemsCopy = _items.Values.ToList();
            }

            foreach (var accountItem in itemsCopy)
            {
                // TODO: if share could not be loaded, try to load it again, then add daily data
                var newDailyData = await ticker.GetDailyInformationForShareAsync(accountItem.Share);
                accountItem.UpdateDailyData(newDailyData.Value);
            }
        }

        /// <summary>
        /// event is fired when the portfolio data were loaded after deserialization.
        /// </summary>
        public event Action<Portfolio> Loaded;
        public void FireLoaded() => Loaded?.Invoke(this);
    }
}
