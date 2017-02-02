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
    }
}