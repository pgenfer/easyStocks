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
        private readonly IPortfolioRepository _portfolio;

        public WpfNavigationService(
            SimpleContainer container,
            IPortfolioRepository portfolio)
        {
            _container = container;
            _portfolio = portfolio;
        }

        public void NavigateToCreateAccountItem(ShareDailyInformation stockInformation)
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            var accountItemCreateViewModel = new AccountItemCreateViewModel(this, _portfolio)
            {
                Parameter = stockInformation
            };
            mainViewModel.ActivateItem(accountItemCreateViewModel);

        }

        public void NavigateToEditAccountItem(AccountItemId accountItemId)
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            var accountItemEditViewModel = new AccountItemEditViewModel(this, _portfolio)
            {
                Parameter = accountItemId
            };
            mainViewModel.ActivateItem(accountItemEditViewModel);
        }

        public void NavigateToPortfolio()
        {
            var mainViewModel = _container.GetInstance<MainViewModel>();
            mainViewModel.ActivateItem(mainViewModel.Portfolio);
        }

        public void NavigateToSearchView()
        {
            throw new NotImplementedException();
        }
    }
}
