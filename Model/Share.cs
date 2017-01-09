

namespace EasyStocks.Model
{
    /// <summary>
    /// represents a single share with a unique symbol.
    /// </summary>
    public class Share
    {
        /// <summary>
        /// can be used to construct an invalid share in case the share information could not be
        /// retrieved from the ticker
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Share EmptyShare(string symbol) => new Share(symbol, string.Empty) {IsValid = false};
        
        /// <summary>
        /// the symbol used to identify the share.
        /// This symbol should be unique.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Name of the share
        /// </summary>
        public string Name { get; }

        public Share(string symbol, string name)
        {
            Symbol = symbol;
            Name = name;
            IsValid = true;
        }

        /// <summary>
        /// the daily information for this share.
        /// </summary>
        public ShareDailyInformation DailyData { get; set; } = new ShareDailyInformation();

        /// <summary>
        /// a flag used to determine if this share is valid or not
        /// </summary>
        public bool IsValid { get; private set; }
        public override string ToString() => IsValid ? $"{Name} ({Symbol})" : $"{Symbol} (not found)";
        public override int GetHashCode() => Symbol.GetHashCode();
        public override bool Equals(object obj) => ((Share) obj)?.Symbol == Symbol;
    }
}
