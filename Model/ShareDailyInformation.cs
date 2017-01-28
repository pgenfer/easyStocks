namespace EasyStocks.Model
{
    /// <summary>
    /// the data that will be updated daily for this share
    /// </summary>
    public class ShareDailyInformation
    {
        public static readonly ShareDailyInformation NoInfo = new ShareDailyInformation();
        
        /// <summary>
        /// the rate at which this share is currently traded
        /// </summary>
        public Quote Rate { get; }
        public QuoteChangePercent ChangeInPercent { get; }
        public QuoteChangeAbsolute ChangeAbsolute { get; }

        public ShareDailyInformation()
        {
            Rate = new Quote(0.0f);
            ChangeInPercent = new QuoteChangePercent(QuoteChangePercent.NoChange);
            ChangeAbsolute = new QuoteChangeAbsolute(QuoteChangeAbsolute.NoChange);
            IsAccurate = false;
        }

        public ShareDailyInformation(
            float value,
            string percent = QuoteChangePercent.NoChange,
            string absolute = QuoteChangeAbsolute.NoChange)
        {
            Rate = new Quote(value);
            ChangeInPercent = new QuoteChangePercent(percent);
            ChangeAbsolute = new QuoteChangeAbsolute(absolute);
            IsAccurate = true;
        }

        public bool IsAccurate { get;}
        public override string ToString() => IsAccurate ? $"{Rate} ({ChangeInPercent})" : "no data available";
    }
}