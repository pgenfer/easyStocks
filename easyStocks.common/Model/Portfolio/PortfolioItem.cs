using System;
using EasyStocks.Extension;
using EasyStocks.ViewModel;

namespace EasyStocks.Model.Account
{
    /// <summary>
    /// stores information and logic for a single portfolio item
    /// </summary>
    public class PortfolioItem
    {
        public static PortfolioItem CreateByUser(
            string symbol,
            string shareName,
            float buyingRate,
            DateTime buyingDate,
            float dailyChange,
            float dailyChangeInPercent)
        {
            var portfolioItem = new PortfolioItem
            {
                Symbol = symbol,
                ShareName = shareName,
                CurrentRate = buyingRate,
                BuyingRate = buyingRate,
                BuyingDate = buyingDate,
                DailyChange = dailyChange,
                DailyChangeInPercent = dailyChangeInPercent,
                StopRate = buyingRate * Constants.StopRatePercentage
            };
            return portfolioItem;
        }

        public static PortfolioItem CreateFromStorage(
            string symbol,
            float buyingRate,
            DateTime buyingDate,
            float stopRate)
        {
            var portfolioItem = new PortfolioItem
            {
                ShareName = EasyStocksStrings.RetrievingData, // name will be received later
                Symbol = symbol,
                BuyingRate = buyingRate,
                BuyingDate = buyingDate,
                StopRate = stopRate
            };
            return portfolioItem;
        }

        /// <summary>
        /// symbol can only be set initially
        /// </summary>
        public string Symbol { get; private set; }
        /// <summary>
        /// stop rate is calculated initially but is then updated whenever
        /// the current rate changes
        /// </summary>
        public float StopRate { get; private set; }
        /// <summary>
        /// buying information can be changed any time by the user
        /// </summary>
        public DateTime BuyingDate { get; set; }
        public float BuyingRate { get; set; }
        /// <summary>
        /// stock information can only be changed by the stock ticker
        /// </summary>
        public string ShareName { get; set; }
        public float CurrentRate { get; set; }
        public float DailyChange { get; set; }
        public float DailyChangeInPercent { get; set; }
        /// <summary>
        /// calculated values
        /// </summary>
        public float OverallChange => CurrentRate - BuyingRate;
        public float OverallChangeInPercent => 100f / BuyingRate * OverallChange;
        public bool StopQuoteReached => _dataReceived && CurrentRate < StopRate;
        public RateChange DailyTrend => DailyChange.GetTrend();
        public RateChange OverallTrend => OverallChange.GetTrend();
        public DateTime LastTradingDate { get; private set; }

        /// <summary>
        /// flag is used to control whether we have already
        /// received any information for this portfolio item
        /// </summary>
        private bool _dataReceived = false;


        private void RecalculateStopRate()
        {
            // the stop rate should
            var newStopRate = CurrentRate * Constants.StopRatePercentage;
            // the stop quote does never decrease, so if it reaches one value
            // it can only become higher but never decrease
            if (newStopRate > StopRate)
            {
                StopRate = newStopRate;
            }
        }

        /// <summary>
        /// updates the current account information with the lasted daily stock data.
        /// </summary>
        /// <param name="dailyInformation">the latest daily stock data</param>
        /// <returns>true if the item has changed since the last update,
        /// returns false in case the account item did not change</returns>
        public bool Update(ShareDailyInformation dailyInformation)
        {
            _dataReceived = true;
            // check if the current rate has changed compared to the last time
            // we received the values (the new rate must be newer than our last one
            var rateHasChanged = 
                dailyInformation.LastTradingDate > LastTradingDate && 
                Math.Abs(dailyInformation.CurrentRate - CurrentRate) > 0.009;
            // if rate has changed, also update the stop rate
            if (rateHasChanged)
            {
                ShareName = dailyInformation.ShareName;
                CurrentRate = dailyInformation.CurrentRate;
                DailyChange = dailyInformation.DailyChange;
                DailyChangeInPercent = dailyInformation.DailyChangeInPercent;
                LastTradingDate = dailyInformation.LastTradingDate;
                RecalculateStopRate();
            }
            return rateHasChanged;
        }
    }
}