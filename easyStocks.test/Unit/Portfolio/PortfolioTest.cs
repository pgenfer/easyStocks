using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model.Account;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    class PortfolioTest
    {
        [Test]
        public void Portfolio_ToDto_LastChangeConverted()
        {
            var current = DateTime.Now;
            var portfolio = new PortfolioRepository {TimeOfLastChange = current};
            var dto = portfolio.ToDto();

            Assert.That(current, Is.EqualTo(dto.LastChange));
        }
    }
}
