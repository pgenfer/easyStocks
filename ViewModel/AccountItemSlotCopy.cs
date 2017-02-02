using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemSlotCopy
    {
        public AccountItemSlotCopy(
            AccountItemId id,
            string name, 
            string symbol, 
            string currentRate,
            float dailyChange, 
            RateChange dailyTrend, 
            bool isStopQuoteReached)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
            CurrentRate = currentRate;
            DailyChange = dailyChange;
            DailyTrend = dailyTrend;
            IsStopQuoteReached = isStopQuoteReached;
        }

        public AccountItemId Id { get; }
        public string Name { get; }
        public string Symbol { get; }
        public string CurrentRate { get; }
        public float DailyChange { get; }
        public RateChange DailyTrend { get; }
        public bool IsStopQuoteReached { get; }
    }
}