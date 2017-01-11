using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemEditViewModel
    {
        private readonly AccountItem _accountItem;
        public AccountItemDataViewModel AccountData { get; }
        public DailyDataViewModel DailyData { get; }

        public AccountItemEditViewModel(AccountItem accountItem)
        {
            _accountItem = accountItem;
            AccountData = new AccountItemDataViewModel(
                accountItem.Share,
                accountItem.BuyingQuote.Date,
                accountItem.BuyingQuote.Quote.Value,
                accountItem.StopQuote.Value);
            // changes done by the user will be written immediately back to the account item
            AccountData.PropertyChanged += OnAccountDataChangedByUser;

            DailyData = new DailyDataViewModel(accountItem.Share.DailyData);
            // update the edit view model when stock data changes
            accountItem.AccountItemUpdated += x => DailyData.Update(x.Share.DailyData);
        }

        private void OnAccountDataChangedByUser(object sender, PropertyChangedEventArgs e)
        {
            _accountItem.BuyingQuote = new HistoricalQuote(
                new Quote(AccountData.BuyingRate),AccountData.BuyingDate );
            _accountItem.StopQuote = new Quote(AccountData.StopRate);
        }
    }
}
