using System;

namespace EasyStocks.Model
{
    /// <summary>
    /// An account item is a share that is placed
    /// in a portfolio. The account item stores
    /// the quote of the share at the buying date
    /// and can also calculate the value difference of
    /// the shares at the current time.
    /// </summary>
    public class AccountItem
    {
        /// <summary>
        /// this multiplicator is used to calculate the new stop rate.
        /// </summary>
        private const float StopRatePercentage = 0.9f;

        /// <summary>
        /// the share that is stored under this account item
        /// </summary>
        public Share Share { get; }
        /// <summary>
        /// the rate and the date when this account item was created.
        /// </summary>
        public HistoricalQuote BuyingQuote { get; }
        /// <summary>
        /// the quote at which this account item should be sold.
        /// The stop quote will be adjusted whenever the current quote increases.
        /// </summary>
        public Quote StopQuote { get; private set; }

        /// <summary>
        /// flag is set in case that this account item should be sold
        /// because it dropped below the stop rate
        /// </summary>
        public bool ShouldBeSold => Share.DailyData?.Rate < StopQuote;

        /// <summary>
        /// constructor used when a new account item was created by the user
        /// </summary>
        /// <param name="share"></param>
        /// <param name="buyingDate"></param>
        public AccountItem(Share share, DateTime buyingDate)
        {
            Share = share;
            BuyingQuote = new HistoricalQuote(share.DailyData.Rate, buyingDate);
            AdjustStopRate(share.DailyData.Rate);
        }

        public event Action<AccountItem> AccountItemUpdated;

        /// <summary>
        /// creates a new accountitem with all provided information.
        /// Used during deserialization.
        /// </summary>
        /// <param name="share">share</param>
        /// <param name="buyingDate">date when share was bought</param>
        /// <param name="buyingRate">rate at which the share was bought</param>
        /// <param name="stopQuote">the quote at which the share should be sold</param>
        public AccountItem(Share share, DateTime buyingDate,float buyingRate, float stopQuote)
        {
            Share = share;
            BuyingQuote = new HistoricalQuote(new Quote(buyingRate), buyingDate);
            StopQuote = new Quote(stopQuote);
        }

        public void AdjustStopRate(Quote newShareRate)
        {
            // the stop rate should
            var newStopQuote = newShareRate* StopRatePercentage;
            // the stop quote does never decrease, so if it reaches one value
            // it can only become higher but never decrease
            if (newStopQuote > StopQuote)
                StopQuote = newStopQuote;
        }

        public void UpdateDailyData(ShareDailyInformation newData)
        {
            // check if we have already any daily data stored
            var oldData = Share.DailyData;
            if (oldData != null)
            {
                AdjustStopRate(newData.Rate);
                Share.DailyData = newData;
            }
            AccountItemUpdated?.Invoke(this);
        }
    }
}