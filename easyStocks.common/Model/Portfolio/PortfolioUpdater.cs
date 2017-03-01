using System;
using System.Threading;
using EasyStocks.Network;

namespace EasyStocks.Model
{
    /// <summary>
    /// updates the portfolio in given intervals
    /// </summary>
    public class PortfolioUpdater
    {
        private static readonly TimeSpan _IntervalForWifiConnection = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan _IntervalForRoamingConnection = TimeSpan.FromSeconds(60);

        private readonly IPortfolioUpdateRepository _portfolio;
        private readonly IStockTicker _stockTicker;
        private TimeSpan _currentInterval = _IntervalForWifiConnection;
        private readonly Timer _portfolioUpdateTimer;

        public PortfolioUpdater(
            IPortfolioUpdateRepository portfolio,
            IStockTicker stockTicker)
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
        private void StopUpdate() => _portfolioUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);

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
            _portfolioUpdateTimer.Change(_currentInterval, Timeout.InfiniteTimeSpan);
        }

        internal void ReactOnNetworkChanges(Connectivity connectivity)
        {
            StopUpdate();
            if (connectivity == Connectivity.Wifi)
            {
                _currentInterval = _IntervalForWifiConnection;
                StartUpdate();
            }
            if (connectivity == Connectivity.Other)
            {
                _currentInterval = _IntervalForRoamingConnection;
                StartUpdate();
            }

        }
    }
}
