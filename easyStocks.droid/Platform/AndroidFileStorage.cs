using System;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Model;
using EasyStocks.Storage;
using Newtonsoft.Json;

namespace EasyStocks.App.Droid.Platform
{
    public class AndroidFileStorage : JsonBaseStorage, IFileSystemStorage
    {
        private readonly IErrorService _errorService;
        private readonly string _fileName;

        public AndroidFileStorage(IErrorService errorService)
        {
            _errorService = errorService;
            _fileName = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "easystocks.json");
        }

        public override async Task<PortfolioDto> LoadFromStorageAsync()
        {
            if (System.IO.File.Exists(_fileName))
            {
                try
                {
                    using (var reader = System.IO.File.OpenText(_fileName))
                    {
                        var content = await reader.ReadToEndAsync();
                        return FromJson(content);
                    }
                }
                catch (Exception exception)
                {
                    // critical error, no access to storage
                    _errorService.TrackError(exception,ErrorId.CannotLoadPortfolioFromStorage);
                    return new PortfolioDto();
                }
            }
            return new PortfolioDto(); // no file => just return empty portfolio
        }

        public override async Task SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                var content = ToJson(portfolio);

                using (var writer = System.IO.File.CreateText(_fileName))
                {
                    // always overwrite the file
                    await writer.WriteAsync(content);
                }
            }
            catch (Exception exception)
            {
                // critical, cannot write back to storage
                _errorService.TrackError(exception, ErrorId.CannotSavePortfolioToStorage);
            }
        }

        public override Task<bool> HasDataAsync() => Task.Run(() => System.IO.File.Exists(_fileName));
        public override Task ClearAsync()
        {
            return Task.Run(() =>
            {
                if (System.IO.File.Exists(_fileName))
                {
                    try
                    {
                        System.IO.File.Delete(_fileName);
                    }
                    catch (Exception ex)
                    {
                        // could not delete storage, which is 
                        // not such a big issue
                        _errorService.TrackWarning(ex,ErrorId.CannotClearPortfolioInStorage);
                    }
                }
                return true; // no file to delete, so don't bother
            });
        }
    }
}
