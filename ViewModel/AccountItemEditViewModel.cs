using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemEditViewModel : Screen
    {
        private readonly AccountItem _accountItem;
        private readonly Portfolio _portfolio;
        public AccountItemDataViewModel AccountData { get; }
        

        public AccountItemEditViewModel(AccountItem accountItem,Portfolio portfolio)
        {
            _accountItem = accountItem;
            _portfolio = portfolio;
            AccountData = new AccountItemDataViewModel(
                accountItem.Share,
                accountItem.BuyingQuote.Date,
                accountItem.BuyingQuote.Quote.Value,
                accountItem.StopQuote.Value);
        }

        public void ConfirmChanges()
        {
            _accountItem.BuyingQuote = new HistoricalQuote(
                new Quote(AccountData.BuyingRate), AccountData.BuyingDate);
            _accountItem.StopQuote = new Quote(AccountData.StopRate);
            TryClose();
        }

        public void RemoveAccountItem()
        {
            _portfolio.RemoveAccountItem(_accountItem);
            TryClose();
        }
    }
}
