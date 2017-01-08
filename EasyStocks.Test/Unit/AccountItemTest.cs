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
    public class AccountItemTest
    {
        private readonly Share _share = new Share("symbol","name");
        private readonly ShareDailyInformation _initialData = new ShareDailyInformation(10);
        private readonly DateTime _anyDate = DateTime.Now;

        [SetUp]
        public void Init()
        {
            _share.DailyData = _initialData;
        }

        [Test]
        public void CreateItem_InitialStopRateSet()
        {
            var accountItem = new AccountItem(_share,_anyDate);
            Assert.That(accountItem.StopQuote.Value, Is.EqualTo(9f));
        }

        [Test]
        public void AdjustStopRate_RateRaised_StopRateRaised()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.AdjustStopRate(new Quote(20));
            Assert.That(accountItem.StopQuote.Value, Is.EqualTo(18f));
        }

        [Test]
        public void AdjustStopRate_RateRaisedAfterDrop_StopRateNotRaised()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.AdjustStopRate(new Quote(20));
            accountItem.AdjustStopRate(new Quote(15));
            // stop rate should never decrease so it should keep the highest value it had
            Assert.That(accountItem.StopQuote.Value, Is.EqualTo(18f));
        }

        [Test]
        public void AdjustStopRate_RateDropped_StopRateNotRaised()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.AdjustStopRate(new Quote(7));
            // stop rate should be relative to its inital value
            Assert.That(accountItem.StopQuote.Value, Is.EqualTo(9));
        }

        [Test]
        public void AdjustStopRate_RateRaisedAfterRaise_StopRateRaised()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.AdjustStopRate(new Quote(20));
            accountItem.AdjustStopRate(new Quote(40));
            // stop rate should always adjust to the highest rate
            Assert.That(accountItem.StopQuote.Value, Is.EqualTo(36));
        }

        [Test]
        public void ShareDroppedBelowStop_ShouldBeSold()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.UpdateDailyData(new ShareDailyInformation(7));
            // stop rate should always adjust to the highest rate
            Assert.That(accountItem.ShouldBeSold, Is.True);
        }

        [Test]
        public void InitialShare_ShouldNotBeSold()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            // stop rate should always adjust to the highest rate
            Assert.That(accountItem.ShouldBeSold, Is.False);
        }

        [Test]
        public void ShareHigherThanStop_ShouldNotBeSold()
        {
            var accountItem = new AccountItem(_share, _anyDate);
            accountItem.UpdateDailyData(new ShareDailyInformation(20));
            // stop rate should always adjust to the highest rate
            Assert.That(accountItem.ShouldBeSold, Is.False);
        }
    }
}
