using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.Model.Account;

namespace EasyStocks.Dto
{
    public class PortfolioSerializer
    {
        private readonly IStorage _storage;

        public PortfolioSerializer(IStorage storage)
        {
            _storage = storage;
        }

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

        public async Task SaveAsync(IPortfolioPersistentRepository portfolio)
        {
            try
            {
                var newPortfolioDto = new PortfolioDto
                {
                    AccountItems = new List<AccountItemDto>(portfolio.ToDtos())
                };
                await _storage.SaveToStorageAsync(newPortfolioDto);
            }
            catch (Exception ex)
            {
                // TODO: show message that saving was not possible
            }
        }
    }
 }