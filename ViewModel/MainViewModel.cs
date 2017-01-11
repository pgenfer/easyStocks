using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// mainview model which is responsible for the navigation within
    /// the application.
    /// </summary>
    public class MainViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly Portfolio _portfolio;
        /// <summary>
        /// view model that handles the search and navigation within the portfolio.
        /// </summary>
        public PortfolioSearchViewModel PortfolioAndSearch { get;}

        /// <summary>
        /// creates a new mainviewmodel and activates the portfolio and search view
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="stockTicker"></param>
        ///  <param name="findShareCommand">command that is used for searching a share,
        /// needs to be injected because command implementation is platform dependent.</param>
        public MainViewModel(
            Portfolio portfolio,
            IStockTicker stockTicker,
            ICommand findShareCommand)
        {
            _portfolio = portfolio;
            PortfolioAndSearch = new PortfolioSearchViewModel(
                portfolio,
                new SearchShareViewModel(
                    findShareCommand, 
                    stockTicker,
                    OnNavigateToCreateAccountViewModel));
        }

        /// <summary>
        /// show the portfolio and search view as soon as the main view is activated
        /// </summary>
        protected override void OnActivate() => ActivateItem(PortfolioAndSearch);

        /// <summary>
        /// handler is called when the search for a share was completed and the new share
        /// should be added to the portfolio. This method will activate the view model
        /// for creating new account items.
        /// </summary>
        /// <param name="newShare">the new share that should be added to the portfolio</param>
        private void OnNavigateToCreateAccountViewModel(Share newShare)
        {
            var createAccountItemViewModel = new AccountItemCreateViewModel(newShare,_portfolio);
            ActivateItem(createAccountItemViewModel);
        }
    }
}
