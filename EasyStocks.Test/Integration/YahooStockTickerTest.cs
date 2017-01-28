using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using NUnit.Framework;

namespace EasyStocks.Test.Integration
{
    [TestFixture]
    public class YahooStockTickerTest
    {
        [Test]
        public async Task RetrieveSymbolName()
        {
            const string tesla = "GS";
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetShareBySymbolAsync(tesla);
            Assert.That(result.IsSuccessful);
            Assert.That(result.Value.IsValid);
        }

        [Test]
        public async Task RetrieveInvalidSymbol()
        {
            const string tesla = "nosymbol";
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetShareBySymbolAsync(tesla);
            Assert.That(result.IsSuccessful,Is.False);
            Assert.That(result.Value.IsValid,Is.False);
        }

        [Test]
        public async Task RetrieveStockData()
        {
            var share = new Share("TSLA",string.Empty);
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetDailyInformationForShareAsync(share);
            Assert.That(result.IsSuccessful);
            Assert.That(result.Value.IsAccurate);
        }
    }
}
