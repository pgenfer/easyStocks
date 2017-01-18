using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemCreateViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        private Share _share;
        private Portfolio _portfolio;

        public AccountItemCreateViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            CreateAccountItemCommand = new SimpleCommand(CreateAccountItem, () => true);
        }

        private void Setup(Share share, Portfolio portfolio)
        {
            _share = share;
            _portfolio = portfolio;
            AccountData = new AccountItemDataViewModel(
                share,
                DateTime.Now,
                share.DailyData.Rate.Value,
                share.DailyData.Rate.Value * AccountItem.StopRatePercentage);

            DisplayName = EasyStocksStrings.AddShare;
        }

        public ICommand CreateAccountItemCommand { get; }

        public void CreateAccountItem()
        {
            _portfolio.AddShare(_share, AccountData.BuyingDate);
            _navigationService.NavigateToPortfolio();
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

        public override string DisplayName { get; set; }
    }
}
