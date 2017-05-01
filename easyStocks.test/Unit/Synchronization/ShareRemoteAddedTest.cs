using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.Model.Account;
using EasyStocks.Model.Portfolio;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace EasyStocks.Test.Unit.Synchronization
{
    [TestFixture]
    public class ShareRemoteAddedTest
    {
        [Test]
        public void ApplyAddChangeToRepository()
        {
            var addChange = new ShareRemoteAdded(new []{new NewAccountItem("SHARE_1","SH1",20f,5f,25f), });
            var portfolio = new PortfolioRepository();

            addChange.ApplyChangeToPortfolio(portfolio);

            Assert.That(portfolio.GetAllAccountItems().Count() == 1);
            Assert.That(portfolio.GetAllAccountItems().Single(x => x.Symbol == "SH1") != null);
        }
    }
}
