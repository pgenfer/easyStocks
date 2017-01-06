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
    public class PortfolioDeSerializerTest
    {
        private readonly Portfolio _portfolio = new Portfolio();
        private IStockTicker _stockTicker;
        private const string Symbol = "symbol";
        private readonly Share _share = new Share("Symbol",string.Empty);
        private readonly PortfolioDto _portfolioDto = new PortfolioDto();
        private readonly AccountItemDto _itemDto = new AccountItemDto();
        private readonly DateTime _buyingDate = DateTime.Now;
        private const float BuyingRate = 10.0f;
        private const float StopQuote = 8.0f;

        [SetUp]
        public void Init()
        {
            _itemDto.Symbol = Symbol;
            _itemDto.BuyingDate = _buyingDate;
            _itemDto.BuyingRate = BuyingRate;
            _itemDto.StopRate = StopQuote;
            _portfolioDto.AccountItems = new List<AccountItemDto> {_itemDto};

            _stockTicker = For<IStockTicker>();
            _stockTicker.GetShareBySymbolAsync(Symbol).Returns(Result<Share>.Success(_share));
        }

        [Test]
        public async Task DeSerializePortfolio()
        {
            await new PortfolioDeserializer(_stockTicker).FromDto(_portfolio, _portfolioDto);
            var item = _portfolio.Items.Single();
            
            Assert.That(item.Share.Symbol, Is.EqualTo("Symbol"));
            Assert.That(item.BuyingQuote.Date,Is.EqualTo(_buyingDate));
            Assert.That(item.BuyingQuote.Quote.Value,Is.EqualTo(BuyingRate));
            Assert.That(item.StopQuote.Value, Is.EqualTo(StopQuote));
        }

        [Test]
        public async Task DeSerializePortfolio_ClearPortfolioBeforeLoading()
        {
            var portfolio = For<Portfolio>();
            await new PortfolioDeserializer(_stockTicker).FromDto(portfolio, _portfolioDto);
            portfolio.Received().Clear();
        }
    }
}
