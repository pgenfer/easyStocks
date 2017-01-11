using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// search view model is responsible for retrieving
    /// a symbol string and tries to a share for this symbol.
    /// </summary>
    public class SearchShareViewModel : PropertyChangedBase
    {
        private string _searchString;
        private readonly IStockTicker _stockTicker;
        private readonly Action<Share> _createNewAccountAction;
        private string _message;
        private bool _hasError;

        /// <summary>
        /// creates a new search view model.
        /// </summary>
        /// <param name="findShareCommand">Command implementation is platform dependent (WPF or Xamarin),
        /// so the command itself must be created ouside of the view model but can be initialized
        /// with the methods that are provided by the view model.</param>
        /// <param name="stockTicker">used to retrieved the stock data for the search symbol.</param>
        /// <param name="createNewAccountAction"></param>
        public SearchShareViewModel(
            ICommand findShareCommand, 
            IStockTicker stockTicker,
            Action<Share> createNewAccountAction)
        {
            FindShareByNameCommand = findShareCommand;
            _stockTicker = stockTicker;
            _createNewAccountAction = createNewAccountAction;
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
                ResetMessage();
            }
        }

        /// <summary>
        /// message that is shown as search result,
        /// can either be an error or the search result.
        /// </summary>
        public string Message
        {
            get { return _message; }
            private set
            {
                if (value == _message) return;
                _message = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// flag is set in case an error occured while searching the share (e.g. share not found or no connection).
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
            private set
            {
                if (value == _hasError) return;
                _hasError = value;
                NotifyOfPropertyChange();
            }
        }

        private void ResetMessage()
        {
            HasError = false;
            Message = string.Empty;
        }

        /// <summary>
        /// searches for the given symbol.
        /// </summary>
        /// <returns>returns the share object if found or null if no share was found.
        /// Updates the error state of the view model.</returns>
        public async Task Search()
        {
            // reset error state before a new search starts
            ResetMessage();

            var result = await _stockTicker.GetShareBySymbolAsync(SearchString);
            if (result.IsSuccessful)
            {
                SearchString = string.Empty; // clear search after we found the share
                var share = result.Value;
                var dailyData = await _stockTicker.GetDailyInformationForShareAsync(result.Value);
                if(dailyData.IsSuccessful)
                    share.DailyData = dailyData.Value;
                _createNewAccountAction(share);
            }
            // if no symbol was found, show error message
            HasError = true;
            Message = result.ErrorMessage;
        }

        public bool CanSearch => !string.IsNullOrEmpty(SearchString);
        /// <summary>
        /// command can be used for search operation
        /// </summary>
        public ICommand FindShareByNameCommand { get; private set; }
    }
}
