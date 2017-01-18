using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfNavigationService : INavigationService
    {
        private readonly SimpleContainer _container;

        public WpfNavigationService(SimpleContainer container)
        {
            _container = container;
        }

        public void NavigateToCreateAccountItem(Share newShare, Portfolio portfolio)
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            var createAccountItemViewModel = 
                new AccountItemCreateViewModel(this) {Parameter = Tuple.Create(newShare, portfolio)};
            mainViewModel.ActivateItem(createAccountItemViewModel);
        }

        public void NavigateToEditAccountItem(AccountItem accountItem, Portfolio portfolio)
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            var editAccountItemViewModel = new AccountItemEditViewModel(accountItem, portfolio);
            mainViewModel.ActivateItem(editAccountItemViewModel);
        }

        public void NavigateToPortfolio()
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            mainViewModel.ActivateItem(mainViewModel.PortfolioAndSearch);
        }
    }
}
