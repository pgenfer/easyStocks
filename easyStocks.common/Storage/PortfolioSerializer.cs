using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.Model.Account;

namespace EasyStocks.Dto
{
    /// <summary>
    /// This class is the connection between the storage and the data that should be stored.
    /// The Serializer holds a reference to the storage it used and gets the portfolio
    /// every time it has to read or write data to or from the portfolio.
    /// </summary>
    public class PortfolioSerializer
    {
        /// <summary>
        /// storage where the portfolio will be placed.
        /// </summary>
        private readonly IStorage _storage;

        public PortfolioSerializer(IStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// loads the portfolio data from the storage and saves it into
        /// the given portfolio object.
        /// </summary>
        /// <param name="portfolio">portfolio where the data should be loaded into</param>
        /// <returns></returns>
        public async Task LoadAsync(IPortfolioPersistentRepository portfolio)
        {
            portfolio.Clear();
            try
            {
                var portfolioDto = await _storage.LoadFromStorageAsync();
                foreach (var dto in portfolioDto.AccountItems)
                    portfolio.AddAccountItemFromPersistentStorage(dto);
            }
            catch (Exception ex)
            {
                // TODO: show message with error
                portfolio.Clear();
            }
        }

        /// <summary>
        /// saves the given portfolio to the storage.
        /// </summary>
        /// <param name="portfolio">portfolio which contains the data that should be stored.</param>
        /// <returns></returns>
        public async Task SaveAsync(IPortfolioPersistentRepository portfolio)
        {
            try
            {
                var newPortfolioDto = portfolio.ToDto();
                await _storage.SaveToStorageAsync(newPortfolioDto);
            }
            catch (Exception ex)
            {
                // TODO: show message that saving was not possible
            }
        }
    }
 }