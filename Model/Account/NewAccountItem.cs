using System;
using EasyStocks.Extension;
using EasyStocks.ViewModel;

namespace EasyStocks.Model
{
    /// <summary>
    /// class contains all information that are
    /// required to create a new account item.
    /// </summary>
    public class NewAccountItem
    {
        public string ShareName { get; }
        public string Symbol { get; }
        /// <summary>
        /// current rate and buying rate will be the same
        /// since the item is bought right now.
        /// </summary>
        public float BuyingRate { get; set; }
        public DateTime BuyingDate { get; set; }
        public float DailyChange { get; }
        public float DailyChangeInPercent { get; }
        public RateChange DailyTrend { get; }

        public NewAccountItem(
            string shareName, 
            string symbol, 
            float buyingRate,
            float dailyChange, 
            float dailyChangeInPercent)
        {
            ShareName = shareName;
            Symbol = symbol;
            BuyingRate = buyingRate;
            BuyingDate = DateTime.Now;
            DailyChange = dailyChange;
            DailyChangeInPercent = dailyChangeInPercent;
            DailyTrend = dailyChange.GetTrend();
        }
    }
}
