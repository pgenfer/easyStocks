using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyStocks.Model
{
    /// <summary>
    /// handles access to the account items of the user.
    /// The repository implements a CQRS, depending on the use case
    /// different business objects are returned (either for read access or
    /// for write access).
    /// </summary>
    public interface IPortfolioRepository
    {
        /// <summary>
        /// returns the account item in writable form.
        /// </summary>
        /// <param name="id">id of the account item</param>
        /// <returns>the account item</returns>
        WritableAccountItem GetWriteableAccountItemById(AccountItemId id);
        /// <summary>
        /// the given account item should be written to the persistent storage
        /// </summary>
        /// <param name="changedData">new account data to be written</param>
        void WriteDataToAccountItem(UserChangeableAccountData changedData);
        /// <summary>
        /// returns a list of all available account items
        /// </summary>
        /// <returns></returns>
        IEnumerable<ReadonlyAccountItem> GetAllAccountItems();
        /// <summary>
        /// event is fired whenever an account item has changed.
        /// </summary>
        event Action<IEnumerable<ReadonlyAccountItem>> AccountItemsUpdated;
        /// <summary>
        /// event is fired when a new account item was added to the portfolio
        /// </summary>
        event Action<ReadonlyAccountItem> AccountItemAdded;
        /// <summary>
        /// event is fired when an account item was removed
        /// </summary>
        event Action<AccountItemId> AccountItemRemoved;
        /// <summary>
        /// event will be fired when the portfolio has loaded all data
        /// </summary>
        event Action<IEnumerable<ReadonlyAccountItem>> PortfolioLoaded;
        /// <summary>
        /// an item with the given id will be removed from the portfolio.
        /// </summary>
        /// <param name="id"></param>
        void RemoveAccountItem(AccountItemId id);
        /// <summary>
        /// removes all given account items at once
        /// </summary>
        /// <param name="ids"></param>
        void RemoveAccountItems(IEnumerable<AccountItemId> ids);
        /// <summary>
        /// creates a new account item with the given initial data
        /// </summary>
        /// <param name="newItem"></param>
        void CreateNewAccountItem(NewAccountItem newItem);
        /// <summary>
        /// fires the loaded event
        /// </summary>
        void FirePortfolioLoaded();
    }
}