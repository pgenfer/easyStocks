using System;
using System.Collections.Generic;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemSlotCopy
    {
        public AccountItemSlotCopy(
            IEnumerable<AccountItemId> id,
            string name, 
            string symbol, 
            string currentRate,
            float dailyChange, 
            RateChange dailyTrend, 
            bool isStopQuoteReached,
            DateTime lastTradingDate)
        {
            _ids.AddRange(id);
            Name = name;
            Symbol = symbol;
            CurrentRate = currentRate;
            DailyChange = dailyChange;
            DailyTrend = dailyTrend;
            IsStopQuoteReached = isStopQuoteReached;
            LastTradingDate = lastTradingDate;
        }

        private readonly List<AccountItemId> _ids = new List<AccountItemId>();
        public string Name { get; }
        public string Symbol { get; }
        public string CurrentRate { get; }
        public float DailyChange { get; }
        public RateChange DailyTrend { get; }
        public bool IsStopQuoteReached { get; }
        public DateTime LastTradingDate { get; }
        public IEnumerable<AccountItemId> Ids => _ids;
    }
}