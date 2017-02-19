using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Settings;
using EasyStocks.Storage;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using static NSubstitute.Substitute;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class StorageWithBackupStrategyTest
    {
        private IStorage _defaultStorage;
        private IStorage _fallbackStorage;
        private ApplicationSettings _settings;
        private readonly IErrorService _errorService = For<IErrorService>();
        private ISettingsService _settingsService;
        private StorageWithBackupStrategy _storageStrategy;

        [SetUp]
        public void Init()
        {
            _defaultStorage = For<IStorage>();
            _fallbackStorage = For<IStorage>();
            _settingsService = For<ISettingsService>();
            _settings = For<ApplicationSettings>(_settingsService);

            _storageStrategy = new StorageWithBackupStrategy(
                _defaultStorage,
                _fallbackStorage,
                _errorService,
                _settings);
        }

        [Test(Description="Error while loading from default storage => read data from fallback")]
        public async Task DefaultError_FallbackIsUsed()
        {
            // arrange: default storage should throw exception
            _defaultStorage.LoadFromStorageAsync().Throws<Exception>();
            // act: try to load from default storage
            await _storageStrategy.LoadFromStorageAsync();
            // assert: fallback storage was called
            await _fallbackStorage.Received().LoadFromStorageAsync();
        }

        [Test(Description="Data saved to default and also to fallback")]
        public async Task DefaultSaved_FallbackSaved()
        {
            var portfolio = new PortfolioDto();
            await _storageStrategy.SaveToStorageAsync(portfolio);
            // assert
            await _defaultStorage.Received().SaveToStorageAsync(portfolio);
            await _fallbackStorage.Received().SaveToStorageAsync(portfolio);

        }

        [Test(Description = "Error while saving to default => set flag that fallback was used")]
        public async Task ErrorSavingDefault_SetFallbackFlag()
        {
            var portfolio = new PortfolioDto();
            _defaultStorage.SaveToStorageAsync(portfolio).Throws<Exception>();
            await _storageStrategy.SaveToStorageAsync(portfolio);
            // assert
            await _fallbackStorage.Received().SaveToStorageAsync(portfolio);
            _settings.Received().BackupStorageWasUsed = true;
        }

        [Test(Description = "Fallback data available => read it and sync default")]
        public async Task FallbackDataAvailable_LoadAndSync()
        {
            // arrange: backupdata available
            _settings.BackupStorageWasUsed = true;
            // act: try to load data
            await _storageStrategy.LoadFromStorageAsync();
            // assert: use backup storage
            await _fallbackStorage.Received().LoadFromStorageAsync();
            // assert: write backupdata to default storage
            await _defaultStorage.Received().SaveToStorageAsync(Arg.Any<PortfolioDto>());
            // assert: reset flag
            Assert.That(_settings.BackupStorageWasUsed, Is.False);
        }
    }
}
