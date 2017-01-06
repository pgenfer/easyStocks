using System;

namespace EasyStocks
{
    /// <summary>
    /// a rate at a specific time.
    /// </summary>
    public struct HistoricalQuote
    {
        public DateTime Date { get; }
        public Quote Quote { get; }

        public HistoricalQuote(Quote quote, DateTime date)
        {
            Date = date;
            Quote = new Quote(quote);
        }
    }
}