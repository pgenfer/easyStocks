using System;
using EasyStocks.ViewModel;

namespace EasyStocks.Model
{
    /// <summary>
    /// account item with write access
    /// </summary>
    public class WritableAccountItem : AccountItemBase
    {
        public float CurrentRate { get; }
        public float StopRate { get; }
        public float BuyingRate { get; set; }
        public DateTime BuyingDate { get; set; }
        public float DailyChange { get; }
        public float DailyChangeInPercent { get; }
        public float OverallChange { get; }
        public float OverallChangeInPercent { get; }
        public RateChange DailyTrend { get; }
        public RateChange OverallTrend { get; }
        public bool IsStopQuoteReached { get; }

        public WritableAccountItem(
            AccountItemId id, 
            string shareName,
            string symbol,
            float currentRate, 
            float stopRate,
            float buyingRate,
            DateTime buyingDate, 
            float dailyChange, 
            float dailyChangeInPercent, 
            float overallChange, 
            float overallChangeInPercent, 
            RateChange dailyTrend, 
            RateChange overallTrend,
            bool isStopQuoteReached)
            :base(id,shareName,symbol)
        {
            CurrentRate = currentRate;
            StopRate = stopRate;
            BuyingRate = buyingRate;
            BuyingDate = buyingDate;
            DailyChange = dailyChange;
            DailyChangeInPercent = dailyChangeInPercent;
            OverallChange = overallChange;
            OverallChangeInPercent = overallChangeInPercent;
            DailyTrend = dailyTrend;
            OverallTrend = overallTrend;
            IsStopQuoteReached = isStopQuoteReached;
        }
    }

    /// <summary>
    /// contains data that were changed by the user
    /// </summary>
    public class UserChangeableAccountData
    {
        public UserChangeableAccountData(AccountItemId id, float buyingRate, DateTime buyingDate)
        {
            Id = id;
            BuyingRate = buyingRate;
            BuyingDate = buyingDate;
        }

        public AccountItemId Id { get; }
        public float BuyingRate { get; set; }
        public DateTime BuyingDate { get; set; }
        // remark: maybe stop quote will be added here later
    }
}