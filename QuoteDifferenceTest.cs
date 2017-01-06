using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EasyStocks;

namespace EasyStocks.Test
{
    [TestFixture]
    public class QuoteDifferenceTest
    {
        [Test]
        public void RateHasRaised()
        {
            var old =  new Quote(160f);
            var @new = new Quote(200f);
            var diff = @new - old;
            Assert.That(diff.Value,Is.EqualTo(40f));
            Assert.That(diff.InPercent, Is.EqualTo(25f));
            Assert.That(diff.IsCritical, Is.False);
        }
    }
}
