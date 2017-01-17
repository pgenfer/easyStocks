﻿using System;
using Caliburn.Micro.Xamarin.Forms;
using EasyStocks.Model;
using EasyStocks.ViewModel;
using Xamarin.Forms;
using INavigationService = EasyStocks.ViewModel.INavigationService;
using IXamarinNavigationService = Caliburn.Micro.Xamarin.Forms.INavigationService;

namespace EasyStocks.App.Platform
{
    public class XamarinNavigationServiceAdapter : INavigationService
    {
        private readonly IXamarinNavigationService _xamarinNavigationService;
        private int _pageCount = 0; // we count how many pages were added to the root page so we know when we reach the top

        public XamarinNavigationServiceAdapter(IXamarinNavigationService xamarinNavigationService)
        {
            _xamarinNavigationService = xamarinNavigationService;
        }

        public void NavigateToCreateAccountItem(Share share, Portfolio portfolio)
        {
            _pageCount++;
            // in order to create the view model, the parameters must either be provided
            // as dictionary (which does not work for us, since we want to set all parameters at once)
            // or by using a single parameter which will be set to the "Parameter" property. 
            _xamarinNavigationService.NavigateToViewModelAsync<AccountItemCreateViewModel>(
                Tuple.Create(share, portfolio));
    }

        public void NavigateToEditAccountItem(AccountItem accountItem, Portfolio portfolio)
        {
            _pageCount++;
            _xamarinNavigationService.NavigateToViewModelAsync<AccountItemEditViewModel>(
                new object[] { accountItem, portfolio });
        }

        public void NavigateToPortfolio()
        {
            if(_pageCount > 0)
                _xamarinNavigationService.GoBackToRootAsync();
            _pageCount = 0;
        }
    }
}
