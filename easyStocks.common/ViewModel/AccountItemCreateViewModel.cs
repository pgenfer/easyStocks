using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemCreateViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        private readonly IPortfolioRepository _portfolio;
        private NewAccountItem _newItem;
      
        public AccountItemCreateViewModel(
            INavigationService navigationService,
            IPortfolioRepository portfolio)
        {
            _navigationService = navigationService;
            _portfolio = portfolio;
            CreateAccountItemCommand = new SimpleCommand(CreateAccountItem, () => true);
        }

        public string ShareName => _newItem.ShareName;
        public string Symbol => _newItem.Symbol;

        public float CurrentRate
        {
            get { return _newItem.BuyingRate; }
            set
            {
                _newItem.BuyingRate = value;
                NotifyOfPropertyChange(nameof(StopRate));
            }
        }

        public DateTime BuyingDate
        {
            get { return _newItem.BuyingDate; }
            set { _newItem.BuyingDate = value; }
        }

        public string StopRate => (CurrentRate*Constants.StopRatePercentage).ToString("N2");

        public string DailyChangeString => _newItem.DailyChange.ToStringWithSign();
        public string DailyChangeInPercentString => _newItem.DailyChangeInPercent.ToPercentStringWithSign();

        public RateChange DailyTrend => _newItem.DailyTrend;

        private void Setup(ShareDailyInformation dailyInformation)
        {
            _newItem = new NewAccountItem(
                dailyInformation.ShareName,
                dailyInformation.Symbol,
                dailyInformation.CurrentRate,
                dailyInformation.DailyChange,
                dailyInformation.DailyChangeInPercent);

            DisplayName = EasyStocksStrings.AddShare;
        }

        public ICommand CreateAccountItemCommand { get; }

        public void CreateAccountItem()
        {
            _portfolio.CreateNewAccountItem(_newItem);
            _navigationService.NavigateToPortfolio();
        }

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public ShareDailyInformation Parameter
        {
            set { Setup(value); }
        }

        public void Cancel() => TryClose();
        public override string DisplayName { get; set; }
    }
}
