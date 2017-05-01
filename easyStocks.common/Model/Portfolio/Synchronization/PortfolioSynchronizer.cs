using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model.Account;

namespace EasyStocks.Model.Portfolio
{
    /// <summary>
    /// class is responsible for keeping the local repository and the remote repositories
    /// in sync.
    /// Every time items are added or removed from the repository, its change date will be updated.
    /// If the items between the local and the remote repository are different,
    /// items will either be added or removed from the repository, depending on which repository has the lates change.
    /// </summary>
    public class PortfolioSynchronizer
    {
        private readonly List<PortfolioRemoteChange> _remoteChanges = new List<PortfolioRemoteChange>();
        public IEnumerable<PortfolioRemoteChange> RemoteChanges => _remoteChanges;

        public void ExecuteChanges(PortfolioRepository local)
        {
            // execute the changes on the current portfolio
            foreach (var change in _remoteChanges)
                change.ApplyChangeToPortfolio(local);
        }

        public async Task SyncPortfolioFromRemote(PortfolioRepository local, IStorage storage)
        {
            _remoteChanges.Clear();

            var remote = await storage.LoadFromStorageAsync();
            // our portfolio is newer, so write data from portfolio back to remote (happens during normal save operation)
            if (local.TimeOfLastChange.HasValue && local.TimeOfLastChange > remote.LastChange)
            {
               // nothing to do here, regular write operation will write back to portfolio
            }
            if (!local.TimeOfLastChange.HasValue || remote.LastChange > local.TimeOfLastChange.Value)
            {
                // get all items that are in the remote repository but not yet in our repository
                var itemsToAdd = (
                    from remoteItem in remote.AccountItems
                    where !local.HasItem(remoteItem.Symbol, remoteItem.BuyingDate)
                    select new NewAccountItem(string.Empty, remoteItem.Symbol, remoteItem.BuyingRate, 0f, 0f,remoteItem.BuyingDate))
                    .ToList();
                // get all items that were removed from the remote repository
                var itemsToRemove = local.GetItemsNotInDto(remote)
                    .ToList();

                if (itemsToAdd.Any())
                    _remoteChanges.Add(new ShareRemoteAdded(itemsToAdd));
                if(itemsToRemove.Any())
                    _remoteChanges.Add(new ShareRemoteRemoved(itemsToRemove));
            }
        }
    }
}
