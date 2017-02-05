using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
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
        public ICommand SearchCommand { get; }

        /// <summary>
        /// view model that handles the search and navigation within the portfolio.
        /// </summary>
        public PortfolioViewModel Portfolio { get;}

        /// <summary>
        /// creates a new mainviewmodel and activates the portfolio and search view
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="navigationService"></param>
        public MainViewModel(
            IPortfolioRepository portfolio,
            INavigationService navigationService)
        {
            _portfolio = portfolio;
            _navigationService = navigationService;
            Portfolio = new PortfolioViewModel(
                portfolio,
                OnEditAccountViewModel);
            SearchCommand = new SimpleCommand(OnSearchNewShare,() => true);
        }

        private void OnSearchNewShare()
        {
            _navigationService.NavigateToSearchView();
        }

        /// <summary>
        /// show the portfolio and search view as soon as the main view is activated
        /// </summary>
        protected override void OnActivate() => _navigationService.NavigateToPortfolio();

        /// <summary>
        /// handler is called when the search for a share was completed and the new share
        /// should be added to the portfolio. This method will activate the view model
        /// for creating new account items.
        /// </summary>
        /// <param name="shareDailyInformation">the new share that should be added to the portfolio</param>
        private void OnNavigateToCreateAccountViewModel(ShareDailyInformation shareDailyInformation)
        {
            _navigationService.NavigateToCreateAccountItem(shareDailyInformation);
        }

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
