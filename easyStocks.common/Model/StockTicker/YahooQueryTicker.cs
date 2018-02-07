using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace EasyStocks.Model.StockTicker
{
    public class YahooQueryTicker : IStockTicker
    {
        private readonly IErrorService _errorService;
        /// <summary>
        /// service is needed to retrieve the data for a stock symbol
        /// </summary>
        private readonly HttpClient _yahooFinanceClient = new HttpClient { BaseAddress = new Uri("https://query1.finance.yahoo.com/v8/finance/chart/") };
        private readonly StockExchangeFinder _stockExchangeFinder;
        private readonly StockNameRepository _nameRepository;
        /// <summary>
        /// service is needed to lookup information for a stock by ID (ex. by ISIN)
        /// </summary>
        private readonly HttpClient _openFigi = new HttpClient { BaseAddress = new Uri("https://api.openfigi.com/v1/mapping") };

        public YahooQueryTicker(IErrorService errorService, StockNameRepository nameRepository)
        {
            _errorService = errorService;
            _stockExchangeFinder = new StockExchangeFinder();
            _nameRepository = nameRepository;
        }

        public async Task<IEnumerable<ShareDailyInformation>> GetDailyInformationForShareAsync(IEnumerable<string> symbols)
        {
            var random = new Random();
            symbols = symbols.OrderBy(x => random.Next());

            StartRequest();
            var dailyInformations = new List<ShareDailyInformation>();
            try
            {
                foreach (var symbol in symbols)
                {
                    var response = await _yahooFinanceClient.GetAsync($"{symbol}?range=2d&interval=1d");
                    if (!response.IsSuccessStatusCode)
                        continue;
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        // get the content from the response message
                        var content = response.Content.ReadAsStringAsync();
                        // read the result information from the response
                        var contentResult = content.Result;
                        // convert to json object
                        dynamic json = JObject.Parse(contentResult);
                        // information we need is in chart -> results -> indicators
                        var chart = json.chart;
                        var result = chart.result;
                        
                        foreach (var resultEntry in result)
                        {
                            // in case we do not have data for two days, take the last closing quote from meta data
                            var meta = resultEntry.meta;
                            var chartPreviousClose = float.Parse(meta.chartPreviousClose.ToString(), CultureInfo.CurrentCulture);

                            var timestampEntries = resultEntry.timestamp;
                            var indicatorEntries = resultEntry.indicators;
                            var unadjclosEntries = ((JArray)indicatorEntries.unadjclose).Single()["unadjclose"];

                            var timestamps = new List<float>();
                            var unadjcloses = new List<float>();

                            foreach (var timestamp in timestampEntries)
                                timestamps.Add(float.Parse(timestamp.ToString(), CultureInfo.CurrentCulture));
                            foreach (var unadjclose in unadjclosEntries)
                                unadjcloses.Add(float.Parse(unadjclose.ToString(), CultureInfo.CurrentCulture));
                            // we need at least two timestamp data to calculate the difference
                            // if we have only one timestamp, we assume there was no change since the last time
                            if (timestamps.Count < 1)
                                continue;

                            float timeStampOfCurrentValue = 0f;
                            float oldValue = 0f;
                            float currentValue = 0f;
                            float diff = 0f;
                            float diffInPercent = 0f;

                            if (timestamps.Count == 1 && unadjcloses.Count == 1)
                            {
                                timeStampOfCurrentValue = timestamps[0];
                                currentValue = unadjcloses[0];
                                oldValue = chartPreviousClose;
                            }
                            
                            // ensure that number of entries is correct, otherwise, we have to skip
                            if (timestamps.Count >= 2 && unadjcloses.Count >= 2)
                            {
                                timeStampOfCurrentValue = timestamps[1];
                                oldValue = unadjcloses[0];
                                currentValue = unadjcloses[1];
                            }

                            diff = currentValue - oldValue;
                            diffInPercent = 100.0f / oldValue * diff;

                            var timeOfCurrentValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeStampOfCurrentValue).ToLocalTime();
                            var name = FindStockNameBySymbol(symbol);

                            var shareDailyData = new ShareDailyInformation(
                                   symbol.ToString(),
                                   name,
                                   currentValue,
                                   diff,
                                   diffInPercent);
                            shareDailyData.LastTradingDate = timeOfCurrentValue;
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

        private string FindStockNameBySymbol(string symbol)
        {
            var name = _nameRepository.GetNameBySymbol(symbol);
            if (name == null)
                return string.Empty;
            return name;
        }

        public async Task<IEnumerable<ShareDailyInformation>> FindStocksForSearchString(string searchString)
        {
            StartRequest();
            var dailyInformations = new List<ShareDailyInformation>();
            try
            {
                // the data we send to the lookup service
                var array = new[] { new { idType = "ID_ISIN", idValue = searchString } };
                var jsonData = JsonConvert.SerializeObject(array);
                var response = await _openFigi.PostAsync(string.Empty, new StringContent(jsonData, Encoding.UTF8, "text/json"));
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
                                _nameRepository.AddNameForSymbol(symbol, dataEntry["name"].ToString());
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
