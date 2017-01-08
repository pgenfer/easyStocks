using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;

namespace EasyStocks.Dto
{
    /// <summary>
    /// used to convert the portfolio into a dto object
    /// that can be stored later.
    /// </summary>
    public class PortfolioSerializer
    {
        public PortfolioDto ToDto(Portfolio portfolio)
        {
            return new PortfolioDto
            {
                AccountItems = portfolio.Items.Select(x => new AccountItemDto
                {
                    Symbol = x.Share.Symbol,
                    BuyingDate = x.BuyingQuote.Date,
                    BuyingRate = x.BuyingQuote.Quote.Value,
                    StopRate = x.StopQuote.Value
                }).ToList()
            };
        }

        public async Task<Result<bool>> SaveAsync(Portfolio portfolio, IStorage storage)
        {
            var result = await storage.SaveToStorageAsync(ToDto(portfolio));
            return result;
        }
    }
}
