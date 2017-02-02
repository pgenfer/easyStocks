using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using EasyStocks.Model.Share;
using Newtonsoft.Json.Linq;

namespace EasyStocks.Model
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
        
        public async Task<IEnumerable<ShareDailyInformation>> GetDailyInformationForShareAsync(IEnumerable<string> symbols)
        {
            var dailyInformations = new List<ShareDailyInformation>();
            try
            {
                var response = await _yahooFinanceClient.GetAsync(
                    $"?q=select * from yahoo.finance.quotes where symbol=\"{string.Join(",",symbols)}\"&format={Format}&env={Environment}");
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
                    var count = query.count;
                    var result = query.results;
                    var quote = result.quote;
                    // if there is only one item, we cannot iterate so we store them in a list
                    var quotes = count == 0 ? new dynamic [] {} : count == 1 ? new [] {quote} : quote;
                    foreach (var entry in quotes)
                    {
                        var symbol = entry.symbol;
                        var name = entry.Name;
                        var ask = entry.Ask?.Value ?? entry.LastTradePriceOnly;
                        var percentageChange = entry.PercentChange;
                        var change = entry.Change;

                        if (ask != null && percentageChange != null && change != null)
                            dailyInformations.Add(new ShareDailyInformation(
                                symbol.ToString(),
                                name.ToString(),
                                float.Parse(
                                    ask.ToString(CultureInfo.InvariantCulture),
                                    CultureInfo.InvariantCulture),
                                StockDataParser.DailyChangeFromString(change.ToString()),
                                StockDataParser.DailyChangeInPercentFromString(percentageChange.ToString())));
                    }
                    
                }
                return dailyInformations;
            }
            catch (Exception ex)
            {
                // TODO: Show error message somewhere
                return dailyInformations;
            }
        }
    }
}
