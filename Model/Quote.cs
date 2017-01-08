namespace EasyStocks.Model
{
    /// <summary>
    /// Represents the rate of a share at a given time.
    /// A quote object is immutable
    /// </summary>
    public struct Quote
    {
        public Quote(Quote quote)
        {
            Value = quote.Value;
        }

        public Quote(float value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString("F2");

        public static Quote operator *(Quote quote, float multiplicator) => new Quote(quote.Value*multiplicator);
        public static bool operator >(Quote quote, Quote other) => quote.Value > other.Value;
        public static bool operator <(Quote quote, Quote other) => quote.Value < other.Value;
        public static bool operator >=(Quote quote, Quote other) => quote.Value >= other.Value;
        public static bool operator <=(Quote quote, Quote other) => quote.Value <= other.Value;
        public float Value { get; }
    }
}