using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;

namespace EasyStocks.Storage
{
    /// <summary>
    /// used to copy data from an old storage to a new storage,
    /// can be used when storages are changed.
    /// </summary>
    public class CombinedStorage : IStorage
    {
        private readonly IStorage _oldStorage;
        private readonly IStorage _newStorage;

        public CombinedStorage(IStorage oldStorage, IStorage newStorage)
        {
            _oldStorage = oldStorage;
            _newStorage = newStorage;
        }

        public async Task<PortfolioDto> LoadFromStorageAsync()
        {
            if (await _oldStorage.HasDataAsync())
            {
                // when loading data the first time, try to store them in the file storage
                var data = await _oldStorage.LoadFromStorageAsync();
                if (await _newStorage.SaveToStorageAsync(data))
                    await _oldStorage.ClearAsync();
                return data;
            }
            return await _newStorage.LoadFromStorageAsync();
        }

        public async Task<bool> SaveToStorageAsync(PortfolioDto portfolio) => await _newStorage.SaveToStorageAsync(portfolio);
        public async Task<bool> HasDataAsync() => await _oldStorage.HasDataAsync() || await _newStorage.HasDataAsync();
        public async Task<bool> ClearAsync() => await _oldStorage.ClearAsync() && await _newStorage.ClearAsync();
    }
}
