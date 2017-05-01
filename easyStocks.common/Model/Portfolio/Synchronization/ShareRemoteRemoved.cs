using System.Collections.Generic;
using EasyStocks.Model.Account;

namespace EasyStocks.Model.Portfolio
{
    public class ShareRemoteRemoved : PortfolioRemoteChange
    {
        private readonly IEnumerable<AccountItemId> _removedItems;

        public ShareRemoteRemoved(IEnumerable<AccountItemId> removedItems)
        {
            _removedItems = new List<AccountItemId>(removedItems);
        }

        public override void ApplyChangeToPortfolio(PortfolioRepository portfolio)
        {
            portfolio.RemoveAccountItems(_removedItems);
        }
    }
}