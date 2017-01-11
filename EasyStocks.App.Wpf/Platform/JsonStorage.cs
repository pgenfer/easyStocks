using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using EasyStocks.Model;
using Newtonsoft.Json;

namespace EasyStocks.App.Wpf.Platform
{
    public class JsonStorage : IStorage
    {
        private readonly string _fileName;

        public JsonStorage()
        {
            _fileName = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "easystocks.json");
        }


        public async Task<Result<PortfolioDto>> LoadFromStorageAsync()
        {
            if (System.IO.File.Exists(_fileName))
            {
                try
                {
                    using (var reader = System.IO.File.OpenText(_fileName))
                    {
                        var content = await reader.ReadToEndAsync();
                        var portfolioDto = JsonConvert.DeserializeObject<PortfolioDto>(content);
                        // result can be null in case the file is empty
                        return Result<PortfolioDto>.Success(portfolioDto ?? new PortfolioDto());
                    }
                }
                catch (Exception exception)
                {
                    return Result<PortfolioDto>.Error(new PortfolioDto(), exception.Message);
                }
            }
            return Result<PortfolioDto>.Success(new PortfolioDto()); // no file => just return empty portfolio
        }

        public async Task<Result<bool>> SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                var content = JsonConvert.SerializeObject(
                    portfolio, 
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                using (var writer = System.IO.File.CreateText(_fileName))
                {
                    // always overwrite the file
                    await writer.WriteAsync(content);
                    return Result<bool>.Success(true);
                }
            }
            catch (Exception exception)
            {
                return Result<bool>.Error(false, exception.Message);
            }
        }
    }
}
