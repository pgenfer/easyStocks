using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro.Xamarin.Forms;
using EasyStocks.Model;
using EasyStocks.Settings;
using EasyStocks.ViewModel;
using Xamarin.Forms;
using INavigationService = EasyStocks.ViewModel.INavigationService;
using IXamarinNavigationService = Caliburn.Micro.Xamarin.Forms.INavigationService;
using Caliburn.Micro;

namespace EasyStocks.App.Platform
{
    public class XamarinNavigationServiceAdapter : INavigationService
    {
        private readonly IXamarinNavigationService _xamarinNavigationService;
        // container is needed so that we can create view models directly
        private readonly SimpleContainer _container;
        private int _pageCount = 0; // we count how many pages were added to the root page so we know when we reach the top

        public XamarinNavigationServiceAdapter(
            IXamarinNavigationService xamarinNavigationService,
            SimpleContainer container)
        {
            _xamarinNavigationService = xamarinNavigationService;
            _container = container;
        }

        public void NavigateToCreateAccountItem(ShareDailyInformation shareInfo)
        {
            _pageCount++;
            // in order to create the view model, the parameters must either be provided
            // as dictionary (which does not work for us, since we want to set all parameters at once)
            // or by using a single parameter which will be set to the "Parameter" property. 
            _xamarinNavigationService.NavigateToViewModelAsync<AccountItemCreateViewModel>(shareInfo);
    }

        public void NavigateToEditAccountItem(IEnumerable<AccountItemId> accountItemIds)
        {
            _pageCount++;
            _xamarinNavigationService.NavigateToViewModelAsync<AccountItemEditViewModel>(accountItemIds);
        }

        public void NavigateToPortfolio()
        {
            if(_pageCount > 0)
                _xamarinNavigationService.GoBackToRootAsync();
            _pageCount = 0;
        }

        public void NavigateToSearchView()
        {
            _pageCount++;
            _xamarinNavigationService.NavigateToViewModelAsync<SearchShareViewModel>();
        }

        public async Task NavigateToStorageSelection(ApplicationSettings settings)
        {
            _pageCount++;
            // there is only one instance of this view model, so we can get it from the container
            var selectStorageByUser = _container.GetInstance<StorageSelectionViewModel>();
            await _xamarinNavigationService.NavigateToViewModelAsync<StorageSelectionViewModel>(settings);
            // when the dialog closes, it should set its task state
            await selectStorageByUser.ViewModelClosedTask;
        }

        public async Task NavigateToDropBoxLogin(ApplicationSettings settings)
        {
            _pageCount++;
            var dropboxLogin = _container.GetInstance<DropboxLoginViewModel>();
            await _xamarinNavigationService.NavigateToViewModelAsync<DropboxLoginViewModel>(settings);
            await dropboxLogin.ViewModelClosedTask;
        }
    }
}
