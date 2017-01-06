using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class QuoteTest
    {
        private readonly Quote _ten = new Quote(10);
        private readonly Quote _five = new Quote(5);

        [Test] public void GreaterThan() => Assert.That(_ten > _five, Is.True);
        [Test] public void GreaterThanOrEqual() => Assert.That(_ten >= _five, Is.True);
        [Test] public void SmallerThan() => Assert.That(_five < _ten, Is.True);
        [Test] public void SmallerThanOrEqual() => Assert.That(_five <= _ten, Is.True);
    }
}
