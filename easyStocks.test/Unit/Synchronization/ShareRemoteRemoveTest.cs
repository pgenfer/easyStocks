using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.Model.Account;
using EasyStocks.Model.Portfolio;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class ShareRemoteAddedTest
    {
        [Test]
        public void ApplyRemoveChangeToRepository()
        {
            var portfolio = new PortfolioRepository();
            portfolio.AddAccountItemFromPersistentStorage(new AccountItemDto {Symbol = "SH1"});
            var accountItemId = portfolio.GetAllAccountItems().Single().Id;
            var removeChange = new ShareRemoteRemoved(new []{accountItemId});

            removeChange.ApplyChangeToPortfolio(portfolio);

            Assert.That(portfolio.GetAllAccountItems(),Is.Empty);
        }
    }
}
