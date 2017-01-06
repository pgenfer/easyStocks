using System.Threading.Tasks;

namespace EasyStocks.Dto
{
    /// <summary>
    /// takes a given portfolio object and fills it with the data retrieved
    /// from the dto object.
    /// </summary>
    public class PortfolioDeserializer
    {
        private readonly IStockTicker _stockTicker;

        public PortfolioDeserializer(IStockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public async Task FromDto(Portfolio portfolio, PortfolioDto portfolioDto)
        {
            portfolio.Clear();
            foreach (var dto in portfolioDto.AccountItems)
            {
                var share = await _stockTicker.GetShareBySymbolAsync(dto.Symbol);
                // no matter if this share could be loaded or not, we will add it
                // we might have invalid stock items in the portfolio, but the user has to fix this manually
                portfolio.AddShare(share.Value, dto.BuyingDate, dto.BuyingRate, dto.StopRate);
            }
        }
    }
}