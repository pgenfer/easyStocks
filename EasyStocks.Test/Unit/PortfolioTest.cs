using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class PortfolioTest
    {
        private readonly Portfolio _portfolio = new Portfolio();
        private readonly Share _share = new Share("symbol", "symbol");

        [SetUp]
        public void Init()
        {
            _portfolio.Clear();
        }

        [Test]
        public void ItemAdded_EventFired()
        {
            var eventFired = false;
            _portfolio.AccountItemAdded += _ => eventFired = true;
            _portfolio.AddShare(_share, DateTime.Now);

            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void ItemAddedTwice_EventFiredOnce()
        {
            var eventFiredCount = 0;
            _portfolio.AccountItemAdded += _ => eventFiredCount++;
            _portfolio.AddShare(_share, DateTime.Now);
            _portfolio.AddShare(_share, DateTime.Now);

            Assert.That(eventFiredCount == 1, Is.True);
        }

        [Test]
        public void SameSymbol_AddedOnlyOnce()
        {
            var shareOne = new Share("symbol", string.Empty);
            var shareTwo = new Share("symbol", string.Empty);

            _portfolio.AddShare(shareOne, DateTime.Now);
            _portfolio.AddShare(shareTwo, DateTime.Now);

            Assert.That(_portfolio.Items.Count() == 1);
        }
    }
}
