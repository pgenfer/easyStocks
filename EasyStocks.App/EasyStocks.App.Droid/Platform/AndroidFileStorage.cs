using System;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.Storage;
using Newtonsoft.Json;

namespace EasyStocks.App.Droid.Platform
{
    public class AndroidFileStorage : JsonBaseStorage
    {
        private readonly string _fileName;

        public AndroidFileStorage()
        {
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
                    // TODO: show error message somewhere
                    return new PortfolioDto();
                }
            }
            return new PortfolioDto(); // no file => just return empty portfolio
        }

        public override async Task<bool> SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                var content = ToJson(portfolio);

                using (var writer = System.IO.File.CreateText(_fileName))
                {
                    // always overwrite the file
                    await writer.WriteAsync(content);
                    return true;
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public override Task<bool> HasDataAsync() => Task.Run(() => System.IO.File.Exists(_fileName));
        public override Task<bool> ClearAsync()
        {
            return Task.Run(() =>
            {
                if (System.IO.File.Exists(_fileName))
                {
                    try
                    {
                        System.IO.File.Delete(_fileName);
                        return true;
                    }
                    catch (Exception)
                    {
                        // TODO: something went wrong with the delete operation,
                        // show error message
                        return false;
                    }
                }
                return true; // no file to delete, so don't bother
            });
        }
    }
}
