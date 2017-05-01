using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model.Account;
using EasyStocks.Model.Portfolio;
using NSubstitute;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class PortfolioSynchronizationTest
    {
        private readonly IStorage _storage = For<IStorage>();
        private PortfolioRepository _local;
        private PortfolioDto _remote = new PortfolioDto();

        [SetUp]
        public void Init()
        {
            
        }

        [Test]
        public async Task RemoteItemAdded_Synchronize_ChangeEventCreated()
        {
            // local contains only one item, remote contains two items => add one item to local
            var currentDate = DateTime.Now;
            _local = new PortfolioRepository();
            _local.AddAccountItemFromPersistentStorage(new AccountItemDto { BuyingDate = currentDate,Symbol = "SHARE_1"});    
            _remote = new PortfolioDto
            {
                AccountItems =
                {
                    new AccountItemDto { BuyingDate = currentDate,Symbol= "SHARE_1"},
                    new AccountItemDto { BuyingDate = currentDate,Symbol= "SHARE_2"}
                },
                LastChange = currentDate
            };
            _storage.LoadFromStorageAsync().Returns(_remote);

            var synchronizer = new PortfolioSynchronizer();
            await synchronizer.SyncPortfolioFromRemote(_local, _storage);

            Assert.That(synchronizer.RemoteChanges.Count() == 1);
            Assert.That(synchronizer.RemoteChanges.ToArray()[0] is ShareRemoteAdded);
        }

        [Test]
        public async Task RemoteItemRemoved_Synchronize_ChangeEventCreated()
        {
            // local contains only one item, remote contains two items => add one item to local
            var currentDate = DateTime.Now;
            _local = new PortfolioRepository();
            _local.AddAccountItemFromPersistentStorage(new AccountItemDto { BuyingDate = currentDate, Symbol = "SHARE_1" });
            _local.AddAccountItemFromPersistentStorage(new AccountItemDto { BuyingDate = currentDate, Symbol = "SHARE_2" });
            _remote = new PortfolioDto
            {
                AccountItems ={new AccountItemDto { BuyingDate = currentDate,Symbol= "SHARE_1"},},
                LastChange = currentDate
            };
            _storage.LoadFromStorageAsync().Returns(_remote);

            var synchronizer = new PortfolioSynchronizer();
            await synchronizer.SyncPortfolioFromRemote(_local, _storage);

            Assert.That(synchronizer.RemoteChanges.Count() == 1);
            Assert.That(synchronizer.RemoteChanges.ToArray()[0] is ShareRemoteRemoved);
        }

        [Test]
        public async Task RemoteItemAddedButLocalIsNewer_Synchronize_DontCreateEvent()
        {
            // local contains only one item, remote contains two items => add one item to local
            var currentDate = DateTime.Now;
            _local = new PortfolioRepository {TimeOfLastChange = currentDate};
            _local.AddAccountItemFromPersistentStorage(new AccountItemDto { BuyingDate = currentDate, Symbol = "SHARE_1" });
            _remote = new PortfolioDto
            {
                AccountItems =
                {
                    new AccountItemDto { BuyingDate = currentDate,Symbol= "SHARE_1"},
                    new AccountItemDto { BuyingDate = currentDate,Symbol= "SHARE_2"}
                },
                LastChange = currentDate - TimeSpan.FromDays(1)
            };
            _storage.LoadFromStorageAsync().Returns(_remote);

            var synchronizer = new PortfolioSynchronizer();
            await synchronizer.SyncPortfolioFromRemote(_local, _storage);

            Assert.That(synchronizer.RemoteChanges,Is.Empty);
        }
    }
}
