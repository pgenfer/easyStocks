using System.Collections.Generic;
using EasyStocks.Dto;

namespace EasyStocks.Model.Account
{
    public interface IPortfolioPersistentRepository
    {
        void Clear();
        void AddAccountItemFromPersistentStorage(AccountItemDto dto);
        IEnumerable<AccountItemDto> ToDtos();
    }
}