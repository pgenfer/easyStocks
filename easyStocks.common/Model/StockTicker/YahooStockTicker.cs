using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EasyStocks.Error;
using EasyStocks.Model.Share;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyStocks.Model
{
    /// <summary>
    /// the stock ticker can be used to retrieve
    /// the information for a share.
    /// </summary>
    public class YahooFinanceStockTicker : IStockTicker
    {
        private readonly IErrorService _errorService;
        /// <summary>
        /// service is needed to retrieve the data for a stock symbol
        /// </summary>
        private readonly HttpClient _yahooFinanceClient = new HttpClient { BaseAddress = new Uri("https://query.yahooapis.com/v1/public/yql") };
        private const string Format = "json";
        private const string Environment = "store://datatables.org/alltableswithkeys";
        private readonly StockExchangeFinder _stockExchangeFinder;
        /// <summary>
        /// service is needed to lookup information for a stock by ID (ex. by ISIN)
        /// </summary>
        private readonly HttpClient _openFigi = new HttpClient {BaseAddress = new Uri("https://api.openfigi.com/v1/mapping")};

        public YahooFinanceStockTicker(IErrorService errorService)
        {
            _errorService = errorService;
            _stockExchangeFinder = new StockExchangeFinder();
        }

        public async Task<IEnumerable<ShareDailyInformation>> GetDailyInformationForShareAsync(IEnumerable<string> symbols)
        {
            StartRequest();
            var dailyInformations = new List<ShareDailyInformation>();
            try
            {
                var response = await _yahooFinanceClient.GetAsync(
                    $"?q=select * from yahoo.finance.quotes where symbol=\"{string.Join(",", symbols)}\"&format={Format}&env={Environment}");
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
                    var quotes = count == 0 ? new dynamic[] { } : count == 1 ? new[] { quote } : quote;
                    foreach (var entry in quotes)
                    {
                        var symbol = entry.symbol;
                        var name = entry.Name;
                        var ask = entry.Ask?.Value ?? entry.LastTradePriceOnly;
                        var percentageChange = entry.PercentChange;
                        var change = entry.Change;

                        if (ask != null && percentageChange != null && change != null)
                        {
                            var shareDailyData = new ShareDailyInformation(
                                symbol.ToString(),
                                name.ToString(),
                                float.Parse(
                                    ask.ToString(CultureInfo.InvariantCulture),
                                    CultureInfo.InvariantCulture),
                                StockDataParser.DailyChangeFromString(change.ToString()),
                                StockDataParser.DailyChangeInPercentFromString(percentageChange.ToString()));

                            // also try to store the last trading date we have for this stock
                            var dateAndTime = $"{entry.LastTradeDate} {entry.LastTradeTime}";
                            DateTime dateTimeResult;
                            if (DateTime.TryParse(
                                dateAndTime,
                                new CultureInfo("en-US"),
                                DateTimeStyles.None,
                                out dateTimeResult))
                                shareDailyData.LastTradingDate = dateTimeResult;

                            dailyInformations.Add(shareDailyData);
                        }
                    }

                }
                return dailyInformations;
            }
            catch (Exception ex)
            {
                _errorService.TrackWarning(ex, ErrorId.CannotRetrieveDailyStockData);

                return dailyInformations;
            }
            finally
            {
                StopRequest();
            }
        }

        public async Task<IEnumerable<ShareDailyInformation>> FindStocksForSearchString(string searchString)
        {
            StartRequest();
            var dailyInformations = new List<ShareDailyInformation>();
            try
            {
                // the data we send to the lookup service
                var array = new [] {new{idType = "ID_ISIN",idValue = searchString}};
                var jsonData = JsonConvert.SerializeObject(array);
                var response = await _openFigi.PostAsync(string.Empty, new StringContent(jsonData,Encoding.UTF8, "text/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    // get the content from the response message
                    var content = response.Content.ReadAsStringAsync();
                    // read the result information from the response
                    var contentResult = content.Result;
                    // convert to json object
                    var jsonDataArray = JArray.Parse(contentResult);
                    var sharesAndStockExchanges = new Dictionary<string, string>();
                    foreach (var resultEntries in jsonDataArray)
                    {
                        if (resultEntries["data"] != null) // stocks found for the id
                        {
                            foreach (var dataEntry in resultEntries["data"]) // there is an entry for every stock exchange
                            {
                                var ticker = dataEntry["ticker"].ToString();
                                var exchangeCode = dataEntry["exchCode"].ToString();
                                var stockExchange = _stockExchangeFinder.FindByExchangeCode(exchangeCode);
                                var symbol = stockExchange != null ? stockExchange.AddSuffixToSymbol(ticker) : ticker;
                                string stockExchangeName;
                                // remember the name of the stock exchange for this symbol, 
                                // if we did not have a name of the exchange so far, take the newest stock exchange where this symbol is traded
                                if (sharesAndStockExchanges.TryGetValue(symbol, out stockExchangeName))
                                {
                                    if (string.IsNullOrEmpty(stockExchangeName))
                                        sharesAndStockExchanges[symbol] = stockExchange?.Name ?? EasyStocksStrings.NoStockExchange;
                                }
                                else
                                {
                                    sharesAndStockExchanges.Add(symbol, stockExchange?.Name);
                                }
                            }
                        }
                    }
                    
                    // now retrieve the daily data for each stock
                    dailyInformations = new List<ShareDailyInformation>(
                        await GetDailyInformationForShareAsync(sharesAndStockExchanges.Keys));

                    // now store the exchange data for every share
                    foreach (var dailyInformation in dailyInformations)
                        dailyInformation.StockExchange = sharesAndStockExchanges[dailyInformation.Symbol];
                }
                return dailyInformations;
            }
            catch (Exception ex)
            {
                _errorService.TrackWarning(ex, ErrorId.CannotLookupStocks);
                return dailyInformations;
            }
            finally
            {
                StopRequest();
            }
        }

        private void StartRequest()
        {
            RequestStarted?.Invoke();
            IsProcessing = true;
        }

        private void StopRequest()
        {
            RequestFinished?.Invoke();
            IsProcessing = false;
        }

        public event Action RequestStarted;
        public event Action RequestFinished;
        public bool IsProcessing { get; private set; }
    }
}
