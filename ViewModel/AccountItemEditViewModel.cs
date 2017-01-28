using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemEditViewModel : Screen
    {
        private AccountItem _accountItem;
        private Portfolio _portfolio;
        private readonly INavigationService _navigationService;
        public AccountItemDataViewModel AccountData { get; private set; }
        
        public ICommand ConfirmAccountItemChangesCommand { get; }
        public ICommand RemoveAccountItemCommand { get; }

        public AccountItemEditViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ConfirmAccountItemChangesCommand = new SimpleCommand(ConfirmChanges, () => true);
            RemoveAccountItemCommand = new SimpleCommand(RemoveAccountItem, () => true);
        }


        private void Setup(AccountItem accountItem, Portfolio portfolio)
        {
            _accountItem = accountItem;
            _portfolio = portfolio;
            AccountData = new AccountItemDataViewModel(
                accountItem.Share,
                accountItem.BuyingQuote.Date,
                accountItem.BuyingQuote.Quote.Value,
                accountItem.StopQuote.Value);

            DisplayName = accountItem.Share.Symbol;
        }

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public Tuple<AccountItem, Portfolio> Parameter
        {
            set { Setup(value.Item1, value.Item2); }
        }

        public void ConfirmChanges()
        {
            _accountItem.BuyingQuote = new HistoricalQuote(
                new Quote(AccountData.BuyingRate), AccountData.BuyingDate);
            _accountItem.StopQuote = new Quote(AccountData.StopRate);
            _navigationService.NavigateToPortfolio();
        }

        public void RemoveAccountItem()
        {
            _portfolio.RemoveAccountItem(_accountItem);
            _navigationService.NavigateToPortfolio();
        }
    }
}
