using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Error;
using EasyStocks.Model;
using EasyStocks.Network;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// mainview model which is responsible for the navigation within
    /// the application.
    /// </summary>
    public class MainViewModel : Screen
    {
        private readonly IPortfolioRepository _portfolio;
        private readonly INavigationService _navigationService;
        private bool _isBusy;
        private bool _isConnected;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// flag is used to control visibility
        /// of the refresh button. Depends on the network connectivity
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                if (value == _isConnected) return;
                _isConnected = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand RefreshPortfolioCommand { get; }

        /// <summary>
        /// view model that handles the search and navigation within the portfolio.
        /// </summary>
        public PortfolioViewModel Portfolio { get;}

        public ErrorViewModel Error { get; }

        /// <summary>
        /// creates a new mainviewmodel and activates the portfolio and search view
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="navigationService"></param>
        /// <param name="stockTicker"></param>
        /// <param name="errorService"></param>
        public MainViewModel(
            IPortfolioRepository portfolio,
            INavigationService navigationService,
            IStockTicker stockTicker,
            IErrorService errorService,
            IConnectivityService connectivityService)
        {
            _portfolio = portfolio;
            _navigationService = navigationService;
            Portfolio = new PortfolioViewModel(
                portfolio,
                OnEditAccountViewModel);
            SearchCommand = new SimpleCommand(OnSearchNewShare,() => true);
            RefreshPortfolioCommand = new SimpleCommand(async () => await RefreshPortfolio(portfolio as IPortfolioUpdateRepository, stockTicker), () => !IsConnected);

            Error = new ErrorViewModel(errorService);

            // in case the stock ticker is currently processing a request, keep the view model in sync
            IsBusy = stockTicker.IsProcessing;
            // get notified if a request is processed by the stock ticker
            stockTicker.RequestStarted += () => IsBusy = true;
            stockTicker.RequestFinished += () => IsBusy = false;

            connectivityService.ConnectivityChanged += x => IsConnected = x != Connectivity.None;
        }

        private async Task RefreshPortfolio(IPortfolioUpdateRepository portfolio,IStockTicker stockTicker)
        {
            if(IsConnected)
                await portfolio.CheckForUpdatesAsync(stockTicker);
        }

        private void OnSearchNewShare()
        {
            _navigationService.NavigateToSearchView();
        }

        private void OnEditAccountViewModel(IEnumerable<AccountItemId> accountItems)
        {
            _navigationService.NavigateToEditAccountItem(accountItems);
        }

        public void RemoveAccountItem(IEnumerable<AccountItemId> accountItems)
        {
            _portfolio.RemoveAccountItems(accountItems);
        }
    }
}
