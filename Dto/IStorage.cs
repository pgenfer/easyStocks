using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;

namespace EasyStocks.Dto
{
    /// <summary>
    /// interface for reading and writing a portfolio
    /// to a persistant storage. Must be implemented
    /// in a platform dependent way
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// loads data from persistent storage and writes
        /// it into the given portfolio
        /// </summary>
        /// <returns>A result object with an error message in case data could not be loaded.</returns>
        Task<Result<PortfolioDto>>  LoadFromStorageAsync();
        /// <summary>
        /// saves the given portfolio into persistent storage.
        /// </summary>
        /// <param name="portfolio">DTO object with the portfolio data</param>
        /// <returns>result object with an error message in case data could not be written.</returns>
        Task<Result<bool>>  SaveToStorageAsync(PortfolioDto portfolio);
    }
}
