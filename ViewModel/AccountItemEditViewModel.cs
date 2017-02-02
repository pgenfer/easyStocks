using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemEditViewModel : Screen
    {
        private readonly IPortfolioRepository _portfolio;
        private readonly INavigationService _navigationService;
        private WritableAccountItem _accountItem;
        
        public ICommand ConfirmAccountItemChangesCommand { get; }
        
        public AccountItemEditViewModel(
            INavigationService navigationService,
            IPortfolioRepository portfolio)
        {
            _navigationService = navigationService;
            _portfolio = portfolio;
            ConfirmAccountItemChangesCommand = new SimpleCommand(ConfirmChanges, () => true);
        }

        public string DailyChangeString => _accountItem.DailyChange.ToStringWithSign();
        public string DailyChangeInPercentString => _accountItem.DailyChangeInPercent.ToPercentStringWithSign();
        public string OverallChangeInPercentString => _accountItem.OverallChangeInPercent.ToPercentStringWithSign();
        public string OverallChangeString => _accountItem.OverallChange.ToStringWithSign();
        public RateChange OverallTrend => _accountItem.OverallTrend;
        public RateChange DailyTrend => _accountItem.DailyTrend;
        public float CurrentRate => _accountItem.CurrentRate;
        public float StopRate => _accountItem.StopRate;
        public bool IsStopRateReached => _accountItem.IsStopQuoteReached;

        public float BuyingRate
        {
            get { return _accountItem.BuyingRate; }
            set { _accountItem.BuyingRate = value; }
        }

        public DateTime BuyingDate
        {
            get { return _accountItem.BuyingDate; }
            set { _accountItem.BuyingDate = value; }
        }

        private void Setup(AccountItemId accountItemId)
        {
            _accountItem = _portfolio.GetWriteableAccountItemById(accountItemId);
        }

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public AccountItemId Parameter
        {
            set { Setup(value); }
        }

        public void ConfirmChanges()
        {
            _portfolio.WriteDataToAccountItem(new UserChangeableAccountData(
                _accountItem.Id,
                _accountItem.BuyingRate,
                _accountItem.BuyingDate));
            _navigationService.NavigateToPortfolio();
        }
    }
}
