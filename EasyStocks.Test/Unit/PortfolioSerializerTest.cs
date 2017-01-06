using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using NSubstitute;
using NUnit.Framework.Internal;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class PortfolioSerializerTest
    {
        private Portfolio _portfolio;
        private readonly Share _share = new Share("Symbol",string.Empty);
        private AccountItem _accountItem;
        private readonly DateTime _buyingDate = DateTime.Now;
        private const float BuyingRate = 10.0f;
        private const float StopQuote = 8.0f;

        [SetUp]
        public void Init()
        {
            _accountItem = new AccountItem(_share, _buyingDate, BuyingRate, StopQuote);
            _portfolio = For<Portfolio>();
            _portfolio.Items.Returns(new[] {_accountItem});
        }

        [Test]
        public void SerializePortfolio()
        {
            var portfolioDto = new PortfolioSerializer().ToDto(_portfolio);
            var dto = portfolioDto.AccountItems[0];
            Assert.That(dto.Symbol, Is.EqualTo("Symbol"));
            Assert.That(dto.BuyingDate,Is.EqualTo(_buyingDate));
            Assert.That(dto.BuyingRate,Is.EqualTo(BuyingRate));
            Assert.That(StopQuote, Is.EqualTo(StopQuote));
        }
    }
}
