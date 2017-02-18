using System;
using System.IO;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Storage;

namespace EasyStocks.App.Wpf.Platform
{
    /// <summary>
    /// TODO: we could put file IO in a service and then Mobile and Desktop could use the same FileStorage implementation
    /// </summary>
    public class WindowsFileStorage : JsonBaseStorage, IFileSystemStorage
    {
        private readonly IErrorService _errorService;
        private readonly string _fileName;

        public WindowsFileStorage(IErrorService errorService)
        {
            _errorService = errorService;
            _fileName = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "easystocks.json");
        }

        public override async Task<PortfolioDto> LoadFromStorageAsync()
        {
            if (File.Exists(_fileName))
            {
                try
                {
                    using (var reader = File.OpenText(_fileName))
                    {
                        var content = await reader.ReadToEndAsync();
                        var portfolioDto = FromJson(content);
                        return portfolioDto;
                    }
                }
                catch (Exception exception)
                {
                    _errorService.TrackError(exception, ErrorId.CannotLoadPortfolioFromStorage);
                }
            }
            return new PortfolioDto();
        }

        public override async Task SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                var content = ToJson(portfolio);

                using (var writer = File.CreateText(_fileName))
                {
                    // always overwrite the file
                    await writer.WriteAsync(content);
                }
            }
            catch (Exception exception)
            {
                _errorService.TrackError(exception, ErrorId.CannotSavePortfolioToStorage);
            }
        }
   
        public override Task<bool> HasDataAsync() => Task.Run(() => File.Exists(_fileName));

        public override Task ClearAsync()
        {
            return Task.Run(() =>
            {
                if (File.Exists(_fileName))
                {
                    try
                    {
                        File.Delete(_fileName);
                    }
                    catch (Exception ex)
                    {
                        // could not delete storage, which is 
                        // not such a big issue
                        _errorService.TrackWarning(ex, ErrorId.CannotClearPortfolioInStorage);
                    }
                }
                return true; // no file to delete, so don't bother
            });
        }
    }
}
