using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.Model.Account;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class PortfolioItemTest
    {
        [Test]
        public void UpdatePortfolioItem_StopFlagSet()
        {
            // Arrange: a new, unitilizaed portfolio item
            var portfolioItem = new PortfolioItem();
            var noStopRate = portfolioItem.StopQuoteReached;
            // Act: portfolio item retrieves new data
            portfolioItem.Update(new ShareDailyInformation("SYMBOL", "shareName", 100, -10, 10) { LastTradingDate = DateTime.Now });
            portfolioItem.Update(new ShareDailyInformation("SYMBOL", "shareName", 80, -10, 10) { LastTradingDate = DateTime.Now });
            // Assert: After the update, the stop quote flag should be set if reached
            var stopRate = portfolioItem.StopQuoteReached;

            Assert.That(noStopRate, Is.False);
            Assert.That(stopRate, Is.True);
        }
    }
}
