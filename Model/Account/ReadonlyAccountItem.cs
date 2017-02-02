using EasyStocks.ViewModel;

namespace EasyStocks.Model
{
    /// <summary>
    /// account item that contains only read only information
    /// </summary>
    public class ReadonlyAccountItem : AccountItemBase
    {
        public float CurrentRate { get; }
        public float DailyChangeInPercent { get; }
        public RateChange DailyTrend { get; }
        public bool IsStopQuoteReached { get; }

        public ReadonlyAccountItem(
            AccountItemId id, 
            string shareName, 
            string symbol, 
            float currentRate, 
            float dailyChangeInPercent, 
            RateChange dailyTrend, 
            bool isStopQuoteReached) : 
                base(id, shareName, symbol)
        {
            CurrentRate = currentRate;
            DailyChangeInPercent = dailyChangeInPercent;
            DailyTrend = dailyTrend;
            IsStopQuoteReached = isStopQuoteReached;
        }
    }
}