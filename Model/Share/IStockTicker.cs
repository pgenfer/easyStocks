using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyStocks.Model
{
    /// <summary>
    /// interface for accessing stock information from a remote source.
    /// </summary>
    public interface IStockTicker
    {
        /// <summary>
        /// request information for all the given shares
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<IEnumerable<ShareDailyInformation>> GetDailyInformationForShareAsync(IEnumerable<string> symbols);
        /// <summary>
        /// does a fuzzy search for the search parameter and returns all
        /// stocks that have a name similar to the search.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>list of shares that match the search string or an empty collection
        /// if no items were found.</returns>
        Task<IEnumerable<ShareDailyInformation>> FindStocksForSearchString(string searchString);
    }
}