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
        private Share _share;
        private Portfolio _portfolio;

        private void Setup(Share share, Portfolio portfolio)
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

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public Tuple<Share, Portfolio> Parameter
        {
            set { Setup(value.Item1, value.Item2); }
        }

        public void Cancel() => TryClose();

        public AccountItemDataViewModel AccountData { get; private set; }
    }
}
