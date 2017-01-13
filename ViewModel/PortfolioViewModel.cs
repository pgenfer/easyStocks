using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model for portfolio items
    /// </summary>
    public class PortfolioViewModel : PropertyChangedBase
    {
        private readonly Action<AccountItem> _editAccountItemAction;
        public BindableCollection<AccountItemViewModel> Items { get; } = new BindableCollection<AccountItemViewModel>();

        public AccountItemViewModel SelectedItem { get; set; }

        public PortfolioViewModel(
            Portfolio portfolio,
            Action<AccountItem> editAccountItemAction)
        {
            _editAccountItemAction = editAccountItemAction;
            portfolio.Loaded += OnPortfolioLoaded;
            portfolio.AccountItemAdded += x => Items.Add(new AccountItemViewModel(x));
            portfolio.AccountItemRemoved += x => Items.Remove(Items.Single(vm => vm.BusinessObject == x));
        }

        private void OnPortfolioLoaded(Portfolio portfolio)
        {
            Items.Clear();
            foreach (var item in portfolio.Items)
            {
                var itemViewModel = new AccountItemViewModel(item);
                Items.Add(itemViewModel);
            }
        }

        public void OnAccountItemSelected()
        {
            if(SelectedItem != null)
               _editAccountItemAction(SelectedItem.BusinessObject);
        } 
    }
}
