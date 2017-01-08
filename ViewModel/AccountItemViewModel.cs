using System;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model for account items.
    /// Observes the account item business object
    /// and reacts on its changes.
    /// </summary>
    public class AccountItemViewModel : PropertyChangedBase
    {
        private string _shareName;
        private float _changePercent;
        private float _changeAbsolute;
        private float _currentRate;
        private float _stopRate;
        private float _buyingRate;
        private DateTime _buyingDate;
        private bool _isActualDataAvailable;
        private RateChange _changeOfCurrentRate = RateChange.NoChange;

        /// <summary>
        /// name of the share
        /// </summary>
        public string ShareName
        {
            get { return _shareName; }
            private set
            {
                if (value == _shareName) return;
                _shareName = value;
                NotifyOfPropertyChange();
            }
        }

        public string Symbol { get; }


        private void Update(AccountItem item)
        {
            var share = item.Share;
            var dailyData = share.DailyData;
            ShareName = share.Name;

            // check if data is accurate
            IsActualDataAvailable = dailyData.IsAccurate;

            _changePercent = dailyData.Percent.Value;
            _currentRate = dailyData.Rate.Value;
            _buyingRate = item.BuyingQuote.Quote.Value;
            _buyingDate = item.BuyingQuote.Date;
            _stopRate = item.StopQuote.Value;
            // update the state indicator used for coloring
            ChangeOfCurrentRate =
                dailyData.Percent.IsPositive
                    ? RateChange.Positive
                    : dailyData.Percent.IsNegative
                        ? RateChange.Negative
                        : RateChange.NoChange;

            NotifyOfPropertyChange(nameof(ChangePercentString));
            NotifyOfPropertyChange(nameof(ChangeAbsoluteString));
            NotifyOfPropertyChange(nameof(CurrentRateString));
            NotifyOfPropertyChange(nameof(StopString));

            // no need to update buying date and rate because they won't change 
            // after initialization
        }

        public AccountItemViewModel(AccountItem item)
        {
            Symbol = item.Share.Symbol;
            Update(item);
       
            item.AccountItemUpdated += OnAccountItemUpdated;
        }

        public bool IsActualDataAvailable
        {
            get { return _isActualDataAvailable; }
            private set
            {
                if (value == _isActualDataAvailable) return;
                _isActualDataAvailable = value;
                NotifyOfPropertyChange();
            }
        }

        private static string GetSign(float value) => value > 0 ? "+" : string.Empty;

        public RateChange ChangeOfCurrentRate
        {
            get { return _changeOfCurrentRate; }
            private set
            {
                if (value == _changeOfCurrentRate) return;
                _changeOfCurrentRate = value;
                NotifyOfPropertyChange();
            }
        }

        private void OnAccountItemUpdated(AccountItem item) => Update(item);

        public string ChangePercentString => $"{GetSign(_changePercent)}{_changePercent:N2} %";
        public string CurrentRateString => _currentRate.ToString("N2");
        public string StopString => _stopRate.ToString("N2");
        public string ChangeAbsoluteString => $"{GetSign(_changeAbsolute)}{_changeAbsolute:N2}";
        public string BuyingDateString => _buyingDate.ToString("d");
        public string BuyingRateString => _buyingRate.ToString("N2");
        // TODO: changes since buying
    }
}
    
    