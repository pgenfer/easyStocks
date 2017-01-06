using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EasyStocks.Test.Integration
{
    [TestFixture]
    public class StockTickerTest
    {
        [Test]
        public async Task RetrieveSymbolName()
        {
            const string tesla = "TSLA";
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetShareBySymbolAsync(tesla);
            Assert.That(result.IsSuccessful);
            Assert.That(result.Value.IsValid);
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
