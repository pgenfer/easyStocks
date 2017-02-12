using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Error;

namespace EasyStocks.Storage
{
    /// <summary>
    /// a storage that used a default storage strategy and also 
    /// has a backup strategy in case the default version does not wordk
    /// </summary>
    public class StorageWithBackupStrategy : IStorage
    {
        private readonly IStorage _defaultStorage;
        private readonly IStorage _fallbackStorage;
        private readonly IErrorService _errorService;

        public StorageWithBackupStrategy(
            IStorage defaultStorage, 
            IStorage fallbackStorage,
            IErrorService errorService)
        {
            _defaultStorage = defaultStorage;
            _fallbackStorage = fallbackStorage;
            _errorService = errorService;
        }

        public async Task<PortfolioDto> LoadFromStorageAsync()
        {
            try
            {
                // before reading the default storage, we should sync the fallback storage
                // with our current one
                var hasFallbackData = await _fallbackStorage.HasDataAsync();
                // TODO: we need a flag (which should be set if fallback is more recent than primary storage)
                // checking fallback for data is not enough because fallback will always have the latest data.
                // Until then, do not restore from local storage
                hasFallbackData = false;
                if (hasFallbackData)
                {
                    // read data from fallback and store it in default
                    var fallbackData = await _fallbackStorage.LoadFromStorageAsync();
                    await _defaultStorage.SaveToStorageAsync(fallbackData);
                    await _fallbackStorage.ClearAsync();
                    return fallbackData;
                }

                var portfolioDto = await _defaultStorage.LoadFromStorageAsync();
                return portfolioDto;
            }
            catch (Exception defaultLoadingException)
            {
                _errorService.TrackWarning(defaultLoadingException, ErrorId.CouldNotLoadFromDefaultStorage);
                // could not load, so use backup strategy
                try
                {
                    var portfolioDto = await _fallbackStorage.LoadFromStorageAsync();
                    return portfolioDto;
                }
                catch (Exception fallbackLoadingException)
                {
                    _errorService.TrackError(fallbackLoadingException, ErrorId.CannotLoadPortfolioFromStorage);
                    return new PortfolioDto();
                }
            }
        }

        public async Task SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                await _defaultStorage.SaveToStorageAsync(portfolio);
                // after the default was updated, also ensure that our fallback system has the correct data
                try
                {
                    await _fallbackStorage.SaveToStorageAsync(portfolio);
                }
                catch (Exception ex)
                {
                    // we could not keep our fallback in sync
                    _errorService.TrackWarning(ex,ErrorId.CannotSavePortfolioToStorage);
                }
            }
            catch (Exception defaultSavingException)
            {
                _errorService.TrackWarning(defaultSavingException, ErrorId.CouldNotSaveToDefaultStorage);
                // could not save, so use backup strategy
                try
                {
                    await _fallbackStorage.SaveToStorageAsync(portfolio);
                }
                catch (Exception fallbackSavingException)
                {
                    _errorService.TrackError(fallbackSavingException, ErrorId.CannotSavePortfolioToStorage);
                }
            }
        }

        public Task<bool> HasDataAsync()
        {
           throw new NotImplementedException("HasDataAsync of backupstrategy storage should not be needed");
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException("ClearAsync of backupstrategy storage should not be needed");
        }
    }
}
