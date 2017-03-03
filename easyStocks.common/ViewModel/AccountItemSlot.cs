using System;
using System.Collections.Generic;
using Caliburn.Micro;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// represents all portfolio items for a specific symbol.
    /// E.g. if the user has two account items for Amazon (each one represents
    /// a different buying order) these both items are represented by a single slot item.
    /// If the user starts editing the item, the data of both items will be visible.
    /// </summary>
    public class AccountItemSlot : PropertyChangedBase
    {
        /// <summary>
        /// list of all account item ids that are represented by this slot
        /// </summary>
        private readonly HashSet<AccountItemId> _accountItemIds = new HashSet<AccountItemId>();

        private string _name;
        private string _symbol;
        private string _currentRate;
        private float _dailyChangeInPercent;
        private RateChange _dailyTrend;
        private bool _isStopQuoteReached;
        private DateTime _lastTradingDate;

        /// <summary>
        /// adds another account item to this slot.
        /// If any of the account items has reached the stop quote
        /// we set the overall stop quote to true
        /// </summary>
        /// <param name="newAccountItem"></param>
        public void AddAccountItem(ReadonlyAccountItem newAccountItem)
        {
            _accountItemIds.Add(newAccountItem.Id);
            IsStopQuoteReached = IsStopQuoteReached || newAccountItem.IsStopQuoteReached;
        }
        /// <summary>
        /// returns true if this slot contains one account item with the same id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasId(AccountItemId id) => _accountItemIds.Contains(id);
        /// <summary>
        /// returns true if this slot does not hold any account items any more
        /// </summary>
        public bool IsEmpty => _accountItemIds.Count == 0;
        /// <summary>
        /// removes this id from this slot
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveId(AccountItemId id) => _accountItemIds.Remove(id);

        public IEnumerable<AccountItemId> Ids => _accountItemIds;

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

        /// <summary>
        /// constructor for account slot item that initially holds
        /// only a single account item id. 
        /// Main purpose is for testing.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="symbol"></param>
        /// <param name="dailyChange"></param>
        /// <param name="currentRate"></param>
        /// <param name="name"></param>
        /// <param name="dailyTrend"></param>
        /// <param name="isStopQuoteReached"></param>
        public AccountItemSlot(
            AccountItemId id,
            string symbol,
            float dailyChange,
            float currentRate = 0.0f,
            string name = "",
            RateChange dailyTrend = RateChange.NoChange,
            bool isStopQuoteReached = false)
        {
            SetAccountIds(new [] {id});
            Name = name;
            Symbol = symbol;
            _dailyChangeInPercent = dailyChange;
            CurrentRate = currentRate.ToString("N2");
            DailyTrend = dailyTrend;
            IsStopQuoteReached = isStopQuoteReached;
        }

        private void SetAccountIds(IEnumerable<AccountItemId> ids)
        {
            _accountItemIds.Clear();
            foreach (var id in ids)
                _accountItemIds.Add(id);
        }

        public AccountItemSlot(AccountItemSlotCopy copy)
        {
            Set(copy);
        }

        public AccountItemSlot(ReadonlyAccountItem accountItem)
        {
            Set(accountItem);
        }

        public void Set(ReadonlyAccountItem accountItem, IEnumerable<AccountItemId> ids = null )
        {
            SetAccountIds(ids ?? new [] {accountItem.Id});
            Name = accountItem.ShareName;
            Symbol = accountItem.Symbol;
            CurrentRate = accountItem.CurrentRate.ToString("N2");
            DailyChangeInPercent = accountItem.DailyChangeInPercent;
            DailyTrend = accountItem.DailyTrend;
            IsStopQuoteReached =accountItem.IsStopQuoteReached; 
            LastTradingDate = accountItem.LastTradingDate;
        }

        public AccountItemSlotCopy Copy()
        {
            return new AccountItemSlotCopy(
                _accountItemIds,
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
            SetAccountIds(copy.Ids);
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