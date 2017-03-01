using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;
using Newtonsoft.Json;

namespace EasyStocks.Storage
{
    /// <summary>
    /// stores class that provides information
    /// for serializing the data from and to json
    /// </summary>
    public abstract class JsonBaseStorage : IStorage
    {
        protected PortfolioDto FromJson(string jsonContent)
        {
            var portfolioDto = JsonConvert.DeserializeObject<PortfolioDto>(jsonContent);
            // result can be null in case the file is empty
            return portfolioDto ?? new PortfolioDto();
        }

        protected string ToJson(PortfolioDto portfolio)
        {
            var jsonContent = JsonConvert.SerializeObject(
                   portfolio,
                   Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       DefaultValueHandling = DefaultValueHandling.Ignore
                   });
            return jsonContent;
        }

        public abstract Task<PortfolioDto> LoadFromStorageAsync();
        public abstract Task SaveToStorageAsync(PortfolioDto portfolio);
        public abstract Task<bool> HasDataAsync();
        public abstract Task ClearAsync();
    }
}
