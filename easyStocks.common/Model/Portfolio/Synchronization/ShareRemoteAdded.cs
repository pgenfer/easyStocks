using System.Collections.Generic;
using EasyStocks.Model.Account;

namespace EasyStocks.Model.Portfolio
{
    /// <summary>
    /// a new share was added in the remote portfolio but was not yet synced
    /// to the current portfolio. This action adds the new portfolio items
    /// to the local portfolio
    /// </summary>
    public class ShareRemoteAdded : PortfolioRemoteChange
    {
        private readonly IEnumerable<NewAccountItem> _newItems;

        public ShareRemoteAdded(IEnumerable<NewAccountItem> newItems )
        {
            _newItems = new List<NewAccountItem>(newItems);
        }

        /// <summary>
        /// adds the portfolio items to the local portfolio so that remote and local
        /// are in sync again.
        /// </summary>
        /// <param name="portfolio"></param>
        public override void ApplyChangeToPortfolio(PortfolioRepository portfolio)
        {
            foreach (var item in _newItems)
                portfolio.CreateNewAccountItem(item);
        }
    }
}