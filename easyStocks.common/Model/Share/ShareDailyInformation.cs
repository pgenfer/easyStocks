using System;

namespace EasyStocks.Model
{
    /// <summary>
    /// the data that will be updated daily for this share
    /// </summary>
    public class ShareDailyInformation
    {
        public string Symbol { get;}
        public string ShareName { get; }
        public string StockExchange { get; set; }
        public float CurrentRate { get; }
        public float DailyChange { get;}
        public float DailyChangeInPercent { get; }
        public DateTime LastTradingDate { get; set; }
        
        public ShareDailyInformation(
            string symbol,
            string shareName,
            float currentRate,
            float dailyChange,
            float dailyChangeInPercent)
        {
            Symbol = symbol;
            ShareName = shareName;
            CurrentRate = currentRate;
            DailyChange = dailyChange;
            DailyChangeInPercent = dailyChangeInPercent;
        }
    }
}