using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// since having a navigatable view in WPF requires us to
    /// change the ActiveItem and bind it to a ContentControl,
    /// we need an additional view that hosts the MainView,
    /// otherwise we cannot replace the main view with other views.
    /// </summary>
    public class RootViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly INavigationService _navigationService;

        public RootViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            DisplayName = EasyStocksStrings.EasyStocks;
        }

        /// <summary>
        /// show the portfolio and search view as soon as the main view is activated
        /// </summary>
        protected override void OnActivate() => _navigationService.NavigateToPortfolio();
    }
}
