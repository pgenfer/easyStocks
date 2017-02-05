using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// search view model is responsible for retrieving
    /// a symbol string and tries to a share for this symbol.
    /// </summary>
    public class SearchShareViewModel : Screen
    {
        private string _searchString;
        private readonly IStockTicker _stockTicker;
        private readonly INavigationService _navigationService;

        /// <summary>
        /// stores results of the fuzzy search
        /// </summary>
        public BindableCollection<ShareDailyInformation> Items { get; } = new BindableCollection<ShareDailyInformation>();
       
        /// <summary>
        /// creates a new search view model.
        /// </summary>
        /// so the command itself must be created ouside of the view model but can be initialized
        /// with the methods that are provided by the view model.
        /// <param name="stockTicker">used to retrieved the stock data for the search symbol.</param>
        /// <param name="navigationService"></param>
        public SearchShareViewModel(
            IStockTicker stockTicker,
            INavigationService navigationService)
        {
            FindShareByNameCommand = new SimpleCommand(async () => await Search(),() => CanSearch);
            _stockTicker = stockTicker;
            _navigationService = navigationService;

            DisplayName = EasyStocksStrings.Search;
        }

        /// <summary>
        /// called when the user chooses one of the item of the fuzzy search.
        /// </summary>
        /// <param name="shareInfo"></param>
        public void SelectShare(ShareDailyInformation shareInfo)
        {
            if(shareInfo != null)
                _navigationService.NavigateToCreateAccountItem(shareInfo);
        } 
        
        /// <summary>
        /// search string entered by the user
        /// </summary>
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (value == _searchString) return;
                _searchString = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CanSearch));
                // everytime the user enters text, we reset the message
            }
        }

        public bool HasSearchResult => Items.Count > 0;

        /// <summary>
        /// searches for the given symbol.
        /// </summary>
        /// <returns>returns the share object if found or null if no share was found.
        /// Updates the error state of the view model.</returns>
        public async Task Search()
        {
            // if user did not provide any search string, skip search
            if (string.IsNullOrEmpty(SearchString))
                return;

            Items.Clear();
            var result = await _stockTicker.FindStocksForSearchString(SearchString);
            Items.AddRange(result);
            NotifyOfPropertyChange(nameof(HasSearchResult));
        }

        public bool CanSearch => true; // must always be true, otherwise user cannot enter text
        /// <summary>
        /// command can be used for search operation
        /// </summary>
        public ICommand FindShareByNameCommand { get; private set; }
    }
}
