using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.Model.Account;
using NSubstitute;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class PortfolioSerializerTest
    {
        private PortfolioSerializer _serializer;
        private IStorage _storage;
        private IPortfolioPersistentRepository _portfolio;
        private readonly AccountItemDto _accountItemDto = new AccountItemDto();
        private PortfolioDto _portfolioDto;

        [SetUp]
        public void Init()
        {
            _portfolioDto = new PortfolioDto {AccountItems = {_accountItemDto}};
            _storage = For<IStorage>();
            _portfolio = For<IPortfolioPersistentRepository>();
            _serializer = new PortfolioSerializer(_storage);
        }

        [Test]
        public async Task ClearPortfolioBeforeReading()
        {
            await _serializer.LoadAsync(_portfolio);
            _portfolio.Received().Clear();
        }

        [Test]
        public async Task PortfolioLoaded_AccountItemAddedToPortfolio()
        {
            // arrange
            _storage.LoadFromStorageAsync().Returns(_portfolioDto);
            // act
            await _serializer.LoadAsync(_portfolio);
            // assert
            _portfolio.Received().AddAccountItemFromPersistentStorage(_accountItemDto);
        }

        [Test]
        public async Task PortfolioSaved_StorageCalled()
        {
            // arrange
            _portfolio.ToDto().Returns(_portfolioDto);
            // act
            await _serializer.SaveAsync(_portfolio);
            // assert
            await _storage.Received().SaveToStorageAsync(_portfolioDto);
        }
    }
}
