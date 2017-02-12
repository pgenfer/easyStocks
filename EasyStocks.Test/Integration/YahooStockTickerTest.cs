using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Error;
using EasyStocks.Model;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace EasyStocks.Test.Integration
{
    [TestFixture]
    public class YahooStockTickerTest
    {
        private IStockTicker _ticker;

        [SetUp]
        public void Setup()
        {
            _ticker = new YahooFinanceStockTicker(For<IErrorService>());
        }

        [Test]
        public async Task RetrieveStockData()
        {
            var result = await _ticker.GetDailyInformationForShareAsync(new [] {"TSLA"});
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().Symbol, Is.EqualTo("TSLA"));
        }

        [Test]
        public async Task RetrieveSeveralStockData()
        {
            var result = await _ticker.GetDailyInformationForShareAsync(new[] { "TSLA","MSFT" });
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task FindStocksByName()
        {
            var result = await _ticker.FindStocksForSearchString("TES");
            Assert.That(result.Any(x => x.Symbol == "TSLA"));
        }
    }
}
