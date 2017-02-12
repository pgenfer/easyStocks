using System;
using System.Threading;

namespace EasyStocks.Model
{
    /// <summary>
    /// updates the portfolio in given intervals
    /// </summary>
    public class PortfolioUpdater
    {
        private readonly IPortfolioUpdateRepository _portfolio;
        private readonly IStockTicker _stockTicker;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(10);
        private readonly Timer _portfolioUpdateTimer;

        public PortfolioUpdater(IPortfolioUpdateRepository portfolio,IStockTicker stockTicker)
        {
            _portfolio = portfolio;
            _stockTicker = stockTicker;
            // create the timer but do not start it yet
            _portfolioUpdateTimer = new Timer(
                OnUpdatePortfolioAsync,
                null,
                Timeout.InfiniteTimeSpan,
                Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// explicitly start the update process
        /// </summary>
        public void StartUpdate() => _portfolioUpdateTimer.Change(0,Timeout.Infinite);

        /// <summary>
        /// updates the portfolio. 
        /// The timer will be stopped until all portfolio items
        /// were updated, then the interval starts again.
        /// Method must return async void because of the
        /// delegate signature
        /// </summary>
        /// <param name="state">not used</param>
        public async void OnUpdatePortfolioAsync(object state)
        {
            _portfolioUpdateTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            await _portfolio.CheckForUpdatesAsync(_stockTicker);
            _portfolioUpdateTimer.Change(_updateInterval, Timeout.InfiniteTimeSpan);
        }
    }
}
