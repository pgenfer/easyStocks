using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DataResult = EasyStocks.Result<EasyStocks.ShareDailyInformation>;
using ShareResult = EasyStocks.Result<EasyStocks.Share>;

namespace EasyStocks
{
    /// <summary>
    /// the stock ticker can be used to retrieve
    /// the information for a share.
    /// </summary>
    public class YahooFinanceStockTicker : IStockTicker
    {
        private readonly HttpClient _yahooFinanceClient = new HttpClient{BaseAddress = new Uri("https://query.yahooapis.com/v1/public/yql")};
        private const string Format = "json";
        private const string Environment = "store://datatables.org/alltableswithkeys";

        public async Task<ShareResult> GetShareBySymbolAsync(string symbol)
        {
            try
            {
                var response = await _yahooFinanceClient.GetAsync(
                    $"?q=select * from yahoo.finance.quotes where symbol=\"{symbol}\"&format={Format}&env={Environment}");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    // get the content from the response message
                    var content = response.Content.ReadAsStringAsync();
                    // read the result information from the response
                    var contentResult = content.Result;
                    // convert to json object
                    dynamic json = JObject.Parse(contentResult);
                    // information we need is in query -> results -> quote
                    var query = json.query;
                    var results = query.results;
                    var quote = results.quote;
                    var name = quote.Name;
                    return name == null ? 
                        ShareResult.Error(Share.EmptyShare(symbol), "symbol not found") : 
                        ShareResult.Success(new Share(symbol, name.ToString()));
                }
                return ShareResult.Error(Share.EmptyShare(symbol), response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return ShareResult.Error(Share.EmptyShare(symbol), ex.Message);
            }
        }

        public async Task<Result<ShareDailyInformation>> GetDailyInformationForShareAsync(Share share)
        {
            if(!share.IsValid) // TODO: translate message
                return DataResult.Error(ShareDailyInformation.NoInfo,"symbol not found");
            try
            {
                var response = await _yahooFinanceClient.GetAsync(
                    $"?q=select * from yahoo.finance.quotes where symbol=\"{share.Symbol}\"&format={Format}&env={Environment}");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    // get the content from the response message
                    var content = response.Content.ReadAsStringAsync();
                    // read the result information from the response
                    var contentResult = content.Result;
                    // convert to json object
                    dynamic json = JObject.Parse(contentResult);
                    // information we need is in query -> results -> quote
                    var query = json.query;
                    var results = query.results;
                    var quote = results.quote;
                    var ask = quote.Ask;
                    var percentageChange = quote.PercentChange;
                    if (ask == null || percentageChange == null)
                        return DataResult.Error(ShareDailyInformation.NoInfo, "no quote data available");
                    return DataResult.Success(
                        new ShareDailyInformation(
                            float.Parse(ask.ToString(),CultureInfo.InvariantCulture),
                            percentageChange.ToString()));
                }
                return DataResult.Error(ShareDailyInformation.NoInfo, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return DataResult.Error(ShareDailyInformation.NoInfo, ex.Message);
            }
        }
    }
}
