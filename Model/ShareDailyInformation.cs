namespace EasyStocks.Model
{
    /// <summary>
    /// the data that will be updated daily for this share
    /// </summary>
    public class ShareDailyInformation
    {
        public static readonly ShareDailyInformation NoInfo = new ShareDailyInformation(0.0f) {IsAccurate =  false};
        
        /// <summary>
        /// the rate at which this share is currently traded
        /// </summary>
        public Quote Rate { get; }
        public QuoteChangePercent Percent { get; }

        public ShareDailyInformation(float value,string percent = QuoteChangePercent.NoChange)
        {
            Rate = new Quote(value);
            Percent = new QuoteChangePercent(percent);
            IsAccurate = true;
        }

        public bool IsAccurate { get; private set; }
        public override string ToString() => IsAccurate ? $"{Rate} ({Percent})" : "no data available";
    }
}