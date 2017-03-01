using System;
using Caliburn.Micro;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemSlot : PropertyChangedBase
    {
        public AccountItemId Id { get; private set; }
        private string _name;
        private string _symbol;
        private string _currentRate;
        private float _dailyChangeInPercent;
        private RateChange _dailyTrend;
        private bool _isStopQuoteReached;
        private DateTime _lastTradingDate;

        public float DailyChangeInPercent
        {
            get { return _dailyChangeInPercent; }
            set
            {
                if (value.Equals(_dailyChangeInPercent)) return;
                _dailyChangeInPercent = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(DailyChangeInPercentString));
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange();
            }
        }
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                if (value == _symbol) return;
                _symbol = value;
                NotifyOfPropertyChange();
            }
        }
        public string CurrentRate
        {
            get { return _currentRate; }
            set
            {
                if (value == _currentRate) return;
                _currentRate = value;
                NotifyOfPropertyChange();
            }
        }



        public string DailyChangeInPercentString => $"{DailyChangeInPercent.WithSign()} %";
        public RateChange DailyTrend
        {
            get { return _dailyTrend; }
            set
            {
                if (value == _dailyTrend) return;
                _dailyTrend = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsStopQuoteReached
        {
            get { return _isStopQuoteReached; }
            set
            {
                if (value == _isStopQuoteReached) return;
                _isStopQuoteReached = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime LastTradingDate
        {
            get { return _lastTradingDate; }
            private set
            {
                if (value.Equals(_lastTradingDate)) return;
                _lastTradingDate = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(LastTradingDateString));
            }
        }

        public AccountItemSlot(
            AccountItemId id,
            string symbol,
            float dailyChange,
            float currentRate = 0.0f,
            string name = "",
            RateChange dailyTrend = RateChange.NoChange,
            bool isStopQuoteReached = false)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            _dailyChangeInPercent = dailyChange;
            CurrentRate = currentRate.ToString("N2");
            DailyTrend = dailyTrend;
            IsStopQuoteReached = isStopQuoteReached;
        }

        public AccountItemSlot(AccountItemSlotCopy copy)
        {
            Set(copy);
        }

        public AccountItemSlot(ReadonlyAccountItem accountItem)
        {
            Set(accountItem);
        }

        public void Set(ReadonlyAccountItem accountItem)
        {
            Id = accountItem.Id;
            Name = accountItem.ShareName;
            Symbol = accountItem.Symbol;
            CurrentRate = accountItem.CurrentRate.ToString("N2");
            DailyChangeInPercent = accountItem.DailyChangeInPercent;
            DailyTrend = accountItem.DailyTrend;
            IsStopQuoteReached = accountItem.IsStopQuoteReached;
            LastTradingDate = accountItem.LastTradingDate;
        }

        public AccountItemSlotCopy Copy()
        {
            return new AccountItemSlotCopy(
                Id,
                Name,
                Symbol,
                CurrentRate,
                DailyChangeInPercent,
                DailyTrend,
                IsStopQuoteReached,
                LastTradingDate);
        }

        public void Set(AccountItemSlotCopy copy)
        {
            Id = copy.Id;
            Name = copy.Name;
            Symbol = copy.Symbol;
            CurrentRate = copy.CurrentRate;
            DailyChangeInPercent = copy.DailyChange;
            DailyTrend = copy.DailyTrend;
            IsStopQuoteReached = copy.IsStopQuoteReached;
            LastTradingDate = copy.LastTradingDate;
        }

        public override string ToString() => $"{Symbol} ({DailyChangeInPercentString})";
        public string LastTradingDateString => $"{LastTradingDate.ToDailyString()}, {LastTradingDate:HH:mm}";
    }
}