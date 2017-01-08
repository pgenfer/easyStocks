using System.Threading.Tasks;

namespace EasyStocks.Model
{
    /// <summary>
    /// interface for accessing stock information from a remote source.
    /// </summary>
    public interface IStockTicker
    {
        /// <summary>
        /// returns a Share object if a share for this symbol could be found,
        /// otherwise null.
        /// </summary>
        /// <param name="symbol">symbol used to identify the share</param>
        /// <returns>the valid share object or a dummy share object
        /// used to represent a share that was not found</returns>
        Task<Result<Share>> GetShareBySymbolAsync(string symbol);
        /// <summary>
        /// returns the daily information for the given share.
        /// </summary>
        /// <param name="share">a reference to the share object</param>
        /// <returns>the daily information for this share or an empty
        /// information object if no information was found.</returns>
        Task<Result<ShareDailyInformation>> GetDailyInformationForShareAsync(Share share);
    }
}