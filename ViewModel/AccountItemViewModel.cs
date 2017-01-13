using System;
using Caliburn.Micro;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model for account items.
    /// Observes the account BusinessObject business object
    /// and reacts on its changes.
    /// </summary>
    public class AccountItemViewModel : PropertyChangedBase
    {
        public AccountItem BusinessObject { get; }
        private readonly AccountItemDataViewModel _accountData;
    
        private float _profit;
        private float _profitPercent;
        private RateChange _changeOfProfit;
        private bool _isStopQuoteHit;
        private bool _stopQuoteRaisedTriggered;
        private float _stopQuoteChange;

        private void Update(AccountItem item)
        {
            var share = item.Share;
            ShareName = share.Name;
            
            _profit = item.Profit;
            _profitPercent = item.ProfitInPercent;

            BuyingDate = item.BuyingQuote.Date;
            BuyingRate = item.BuyingQuote.Quote.Value;
            StopRate = item.StopQuote.Value;

            // update the state indicator used for coloring
            ChangeOfProfit =
                _profit > 0 
                    ? RateChange.Positive 
                    : _profit < 0 
                        ? RateChange.Negative 
                        : RateChange.NoChange;

            IsStopQuoteHit = item.ShouldBeSold;

            NotifyOfPropertyChange(nameof(StopString));
            NotifyOfPropertyChange(nameof(ProfitString));
            NotifyOfPropertyChange(nameof(BuyingDateString));
            NotifyOfPropertyChange(nameof(BuyingRateString));

            // update daily data
            Update(item.Share.DailyData);
        }

        public AccountItemViewModel(AccountItem businessObject)
        {
            BusinessObject = businessObject;

            _accountData = new AccountItemDataViewModel(
                businessObject.Share,
                businessObject.BuyingQuote.Date,
                businessObject.BuyingQuote.Quote.Value, 
                businessObject.StopQuote.Value);

            // reroute the property changes from the mixins
            _accountData.PropertyChanged += (s, e) => NotifyOfPropertyChange(e.PropertyName);

            Update(businessObject);

            businessObject.AccountItemUpdated += OnAccountItemUpdated;
            businessObject.StopQuoteRaised += StopQuoteRaised;
        }

        private void StopQuoteRaised(Quote old, Quote @new)
        {
            _stopQuoteChange = @new.Value - old.Value;
            StopQuoteRaisedTriggeredTriggered = true;
            NotifyOfPropertyChange(nameof(StopQuoteChange));
        }

        public bool StopQuoteRaisedTriggeredTriggered
        {
            get { return _stopQuoteRaisedTriggered; }
            private set
            {
                if (value == _stopQuoteRaisedTriggered) return;
                _stopQuoteRaisedTriggered = value;
                NotifyOfPropertyChange();
                if (value) // reset property after it was set to true
                    StopQuoteRaisedTriggeredTriggered = false;
            }
        }

        public RateChange ChangeOfProfit
        {
            get { return _changeOfProfit; }
            private set
            {
                if (value == _changeOfProfit) return;
                _changeOfProfit = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsStopQuoteHit
        {
            get { return _isStopQuoteHit; }
            private set
            {
                if (value == _isStopQuoteHit) return;
                _isStopQuoteHit = value;
                NotifyOfPropertyChange();
            }
        }

        private void OnAccountItemUpdated(AccountItem item) => Update(item);

        public string StopString => _accountData.StopRate.ToString("N2");
        public string BuyingDateString => _accountData.BuyingDate.ToString("d");
        public string BuyingRateString => _accountData.BuyingRate.ToString("N2");
        public string ProfitString => $"{_profit.WithSign()} ({_profitPercent.WithSign()} %)";
        public string StopQuoteChange => $"{_stopQuoteChange.WithSign()}";

        public string ShareName
        {
            get { return _accountData.ShareName; }
            private set { _accountData.ShareName = value; }
        }

        public string Symbol => _accountData.Symbol;

        public DateTime BuyingDate
        {
            get { return _accountData.BuyingDate; }
            private set { _accountData.BuyingDate = value; }
        }

        public float BuyingRate
        {
            get { return _accountData.BuyingRate; }
            private set { _accountData.BuyingRate = value; }
        }

        public float StopRate
        {
            get { return _accountData.StopRate; }
            private set { _accountData.StopRate = value; }
        }

        public string ChangePercentString => _accountData.ChangePercentString;
        public string ChangeAbsoluteString => _accountData.ChangeAbsoluteString;
        public string CurrentRateString => _accountData.CurrentRateString;
        public RateChange ChangeOfCurrentRate => _accountData.ChangeOfCurrentRate;
        public bool IsActualDataAvailable => _accountData.IsActualDataAvailable;
        public void RecalculateStopRate() => _accountData.RecalculateStopRate();
        protected void Update(ShareDailyInformation info) => _accountData.Update(info);
    }
}

