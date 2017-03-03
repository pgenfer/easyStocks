using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;
using EasyStocks.Settings;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfNavigationService : INavigationService
    {
        private readonly SimpleContainer _container;
        private readonly IPortfolioRepository _portfolio;
        private readonly IStockTicker _stockTicker;

        public WpfNavigationService(
            SimpleContainer container,
            IPortfolioRepository portfolio,
            IStockTicker stockTicker)
        {
            _container = container;
            _portfolio = portfolio;
            _stockTicker = stockTicker;
        }

        public void NavigateToCreateAccountItem(ShareDailyInformation stockInformation)
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var accountItemCreateViewModel = new AccountItemCreateViewModel(this, _portfolio)
            {
                Parameter = stockInformation
            };
            rootViewModel.ActivateItem(accountItemCreateViewModel);
        }

        public void NavigateToEditAccountItem(IEnumerable<AccountItemId> accountItemId)
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var accountItemEditViewModel = new AccountItemEditViewModel(this, _portfolio)
            {
                Parameter = accountItemId
            };
            rootViewModel.ActivateItem(accountItemEditViewModel);

        }

        public void NavigateToPortfolio()
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var mainViewModel = _container.GetInstance<MainViewModel>();
            rootViewModel.ActivateItem(mainViewModel);
        }

        public void NavigateToSearchView()
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var searchViewModel = new SearchShareViewModel(_stockTicker, this);
            rootViewModel.ActivateItem(searchViewModel);
        }

        public async Task NavigateToStorageSelection(ApplicationSettings settings)
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var storageSelectionViewModel = new StorageSelectionViewModel {Parameter = settings};
            rootViewModel.ActivateItem(storageSelectionViewModel);
            await storageSelectionViewModel.ViewModelClosedTask;
        }

        public async Task NavigateToDropBoxLogin(ApplicationSettings settings)
        {
            var rootViewModel = _container.GetInstance<RootViewModel>();
            var dropboxLoginViewModel = new DropboxLoginViewModel(this) { Parameter = settings };
            rootViewModel.ActivateItem(dropboxLoginViewModel);
            await dropboxLoginViewModel.ViewModelClosedTask;
        }
    }
}
