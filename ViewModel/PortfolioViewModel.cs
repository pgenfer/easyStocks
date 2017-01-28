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
        private readonly Action<AccountItem> _editAccountItemAction;
        public BindableCollection<AccountItemViewModel> Items { get; } = new BindableCollection<AccountItemViewModel>();

        /// <summary>
        /// flag that can be used to control sorting of the vie wmodel
        /// </summary>
        public bool IsSorting { get;} = true;
      
        public AccountItemViewModel SelectedItem { get; set; }

        public PortfolioViewModel(
            Portfolio portfolio,
            Action<AccountItem> editAccountItemAction)
        {
            _editAccountItemAction = editAccountItemAction;
            portfolio.Loaded += OnPortfolioLoaded;
            portfolio.AccountItemAdded += OnPortfolioItemAdded;
            portfolio.AccountItemRemoved += RemoveAccountItemViewModel;
        }

        private void OnPortfolioLoaded(Portfolio portfolio)
        {
            Items.Clear();
            foreach (var item in portfolio.Items)
                CreateAndAddAccountItemViewModel(item);
        }

        public void OnPortfolioItemAdded(AccountItem businessObject)
        {
            CreateAndAddAccountItemViewModel(businessObject);
        }

        private void CreateAndAddAccountItemViewModel(AccountItem businessObject)
        {
            var accountItemViewModel = new AccountItemViewModel(businessObject);
            accountItemViewModel.PercentChanged += ItemViewModelOnPercentChanged;
            AddItemAtCorrectPosition(accountItemViewModel);
        }

        private void AddItemAtCorrectPosition(AccountItemViewModel accountItemViewModel)
        {
            if (IsSorting)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    var nextItem = Items[i];
                    if (accountItemViewModel.ChangePercent > nextItem.ChangePercent)
                    {
                        Items.Insert(i, accountItemViewModel);
                        return;
                    }
                }
            }
            Items.Add(accountItemViewModel);
        }

        private void RemoveAccountItemViewModel(AccountItem businessObject)
        {
            var accountItemViewModel = Items.Single(vm => vm.BusinessObject == businessObject);
            accountItemViewModel.PercentChanged -= ItemViewModelOnPercentChanged;
            Items.Remove(accountItemViewModel);
        }

        /// <summary>
        /// The usual WPF issues: A collection view source does not react on a property changes
        /// of the properties it uses for sorting.
        /// So when our percentage chanes, the list order is not updated.
        /// What we have to do is:
        /// Whenenver the percentage changes, remove the item and readd it to the list, this
        /// seems to fix the issue:
        /// http://stackoverflow.com/questions/11271048/collectionviewsource-does-not-re-sort-on-property-change
        /// </summary>
        /// <param name="accountItemViewModel"></param>
        private void ItemViewModelOnPercentChanged(AccountItemViewModel accountItemViewModel)
        {
            if (!IsSorting) // check if sorting is disabled
                return;
            Items.Refresh();
            Items.Remove(accountItemViewModel);
            AddItemAtCorrectPosition(accountItemViewModel);
        }

        public void OnAccountItemSelected()
        {
            if(SelectedItem != null)
               _editAccountItemAction(SelectedItem.BusinessObject);
        }
    }
}
