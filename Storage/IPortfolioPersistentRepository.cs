using System.Collections.Generic;
using EasyStocks.Dto;

namespace EasyStocks.Model.Account
{
    public interface IPortfolioPersistentRepository
    {
        void Clear();
        void AddAccountItemFromPersistentStorage(AccountItemDto dto);
        IEnumerable<AccountItemDto> ToDtos();
        // when a serializer is registered, it will save the portfolio whenever
        // it changes
        void RegisterSerializerForChanges(PortfolioSerializer serializer);
    }
}