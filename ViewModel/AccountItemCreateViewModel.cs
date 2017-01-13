using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemCreateViewModel : Screen
    {
        private readonly Share _share;
        private readonly Portfolio _portfolio;

        public AccountItemCreateViewModel(Share share, Portfolio portfolio)
        {
            _share = share;
            _portfolio = portfolio;
            AccountData = new AccountItemDataViewModel(
                share,
                DateTime.Now,
                share.DailyData.Rate.Value,
                share.DailyData.Rate.Value * AccountItem.StopRatePercentage);
        }

        public void CreateAccountItem()
        {
            _portfolio.AddShare(_share, AccountData.BuyingDate);
            TryClose();
        }

        public void Cancel() => TryClose();

        public AccountItemDataViewModel AccountData { get; }
    }
}
