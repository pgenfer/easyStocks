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

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// mainview model which is responsible for the navigation within
    /// the application.
    /// </summary>
    public class MainViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly IPortfolioRepository _portfolio;
        private readonly INavigationService _navigationService;
        private bool _isBusy;

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

        public ICommand SearchCommand { get; }

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
            IErrorService errorService)
        {
            _portfolio = portfolio;
            _navigationService = navigationService;
            Portfolio = new PortfolioViewModel(
                portfolio,
                OnEditAccountViewModel);
            SearchCommand = new SimpleCommand(OnSearchNewShare,() => true);

            Error = new ErrorViewModel(errorService);

            // in case the stock ticker is currently processing a request, keep the view model in sync
            IsBusy = stockTicker.IsProcessing;
            // get notified if a request is processed by the stock ticker
            stockTicker.RequestStarted += () => IsBusy = true;
            stockTicker.RequestFinished += () => IsBusy = false;
        }

        private void OnSearchNewShare()
        {
            _navigationService.NavigateToSearchView();
        }

        /// <summary>
        /// show the portfolio and search view as soon as the main view is activated
        /// </summary>
        protected override void OnActivate() => _navigationService.NavigateToPortfolio();

        private void OnEditAccountViewModel(AccountItemId accountItem)
        {
            _navigationService.NavigateToEditAccountItem(accountItem);
        }

        public void RemoveAccountItem(AccountItemId accountItem)
        {
            _portfolio.RemoveAccountItem(accountItem);
        }
    }
}
