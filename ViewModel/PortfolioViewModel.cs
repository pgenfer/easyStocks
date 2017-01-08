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
        public BindableCollection<AccountItemViewModel> Items { get; } = new BindableCollection<AccountItemViewModel>();

        public PortfolioViewModel(Portfolio portfolio)
        {
            portfolio.Loaded += x => OnPortfolioLoaded(x);
            portfolio.AccountItemAdded += x => Items.Add(new AccountItemViewModel(x));
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
    }
}
