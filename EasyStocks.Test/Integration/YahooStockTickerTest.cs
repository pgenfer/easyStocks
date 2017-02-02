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
        public async Task RetrieveStockData()
        {
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetDailyInformationForShareAsync(new [] {"TSLA"});
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().Symbol, Is.EqualTo("TSLA"));
        }

        [Test]
        public async Task RetrieveSeveralStockData()
        {
            var ticker = new YahooFinanceStockTicker();
            var result = await ticker.GetDailyInformationForShareAsync(new[] { "TSLA","MSFT" });
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
