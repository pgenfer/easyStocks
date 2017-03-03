using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model for portfolio items
    /// </summary>
    public class PortfolioViewModel : PropertyChangedBase
    {
        public AccountItemSlotList Items { get; } = new AccountItemSlotList();
        private readonly IPortfolioRepository _portfolio;
        private readonly Action<IEnumerable<AccountItemId>> _editAccountItemAction;

        public PortfolioViewModel(
            IPortfolioRepository portfolio,
            Action<IEnumerable<AccountItemId>> editAccountItemAction)
        {
            _portfolio = portfolio;
            _editAccountItemAction = editAccountItemAction;
            portfolio.PortfolioLoaded += OnPortfolioLoaded;
        }

        private void OnPortfolioLoaded(IEnumerable<ReadonlyAccountItem> accountItems)
        {
            _portfolio.AccountItemAdded += Items.AddNewAccountItem;
            _portfolio.AccountItemRemoved += Items.RemoveAccountItem;
            _portfolio.AccountItemsUpdated += Items.ChangeAccountItems;
            Items.AddAccountItems(accountItems);
        }

        public void SelectAccountItem(IEnumerable<AccountItemId> accountItemIds)
        {
            if(accountItemIds != null)
               _editAccountItemAction(accountItemIds);
        }
    }
}
