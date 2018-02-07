using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;

namespace EasyStocks.Model.Account
{
    public class PortfolioRepository : 
        IPortfolioRepository,
        IPortfolioPersistentRepository
    {
        private readonly Dictionary<AccountItemId,PortfolioItem> _portfolioItems = new Dictionary<AccountItemId, PortfolioItem>();

        /// <summary>
        /// we store the date whenever an item was added or removed from the portfolio.
        /// Initializes with null means the portfolio was not changed by the user so far
        /// </summary>
        public DateTime? TimeOfLastChange { get; set; } = null;

        public WritableAccountItem GetWriteableAccountItemById(AccountItemId id)
        {
            var portfolioItem = _portfolioItems[id];
            return new WritableAccountItem(
                id,
                portfolioItem.ShareName,
                portfolioItem.Symbol,
                portfolioItem.CurrentRate,
                portfolioItem.StopRate,
                portfolioItem.BuyingRate,
                portfolioItem.BuyingDate,
                portfolioItem.DailyChange,
                portfolioItem.DailyChangeInPercent,
                portfolioItem.DailyTrend,
                portfolioItem.StopQuoteReached);
        }

        public void WriteDataToAccountItem(UserChangeableAccountData changedData)
        {
            var portfolioItem = _portfolioItems[changedData.Id];
            portfolioItem.BuyingDate = changedData.BuyingDate;
            portfolioItem.BuyingRate = changedData.BuyingRate;
            AccountItemsUpdated?.Invoke(new[] {ToReadonlyItem(changedData.Id)});
        }

        public IEnumerable<ReadonlyAccountItem> GetAllAccountItems() => 
            _portfolioItems.Select(x => ToReadonlyItem(x.Key));
        
        private ReadonlyAccountItem ToReadonlyItem(AccountItemId id)
        {
            var portfolioItem = _portfolioItems[id];
            return new ReadonlyAccountItem(
                id,
                portfolioItem.ShareName,
                portfolioItem.Symbol,
                portfolioItem.CurrentRate,
                portfolioItem.DailyChangeInPercent,
                portfolioItem.DailyTrend,
                portfolioItem.StopQuoteReached,
                portfolioItem.LastTradingDate);
        }

        public event Action<IEnumerable<ReadonlyAccountItem>> AccountItemsUpdated;

        public event Action<ReadonlyAccountItem> AccountItemAdded;
        public event Action<AccountItemId> AccountItemRemoved;
        public event Action<IEnumerable<ReadonlyAccountItem>> PortfolioLoaded;

        public void RemoveAccountItem(AccountItemId id)
        {
            _portfolioItems.Remove(id);
            AccountItemRemoved?.Invoke(id);
        }

        public void RemoveAccountItems(IEnumerable<AccountItemId> ids)
        {
            TimeOfLastChange = DateTime.Now;

            foreach (var id in ids)
            {
                _portfolioItems.Remove(id);
                AccountItemRemoved?.Invoke(id);
            }
        }

        public void CreateNewAccountItem(NewAccountItem newItem)
        {
            // create new portfolio item and store it
            var newAccountItemId = new AccountItemId();
            var portfolioItem = PortfolioItem.CreateByUser(
                newItem.Symbol,
                newItem.ShareName,
                newItem.BuyingRate,
                newItem.BuyingDate,
                newItem.DailyChange,
                newItem.DailyChangeInPercent);
            _portfolioItems.Add(newAccountItemId, portfolioItem);

            TimeOfLastChange = DateTime.Now;
            
            // fire event that new item was created
            AccountItemAdded?.Invoke(new ReadonlyAccountItem(
                newAccountItemId,
                portfolioItem.ShareName,
                portfolioItem.Symbol,
                portfolioItem.CurrentRate,
                portfolioItem.DailyChangeInPercent,
                portfolioItem.DailyTrend,
                portfolioItem.StopQuoteReached,
                portfolioItem.LastTradingDate));

        }

        public void FirePortfolioLoaded() => PortfolioLoaded?.Invoke(GetAllAccountItems());

        public async Task CheckForUpdatesAsync(IStockTicker stockTicker)
        {
            // store all items that have changed in a list
            var changedAccountItems = new List<ReadonlyAccountItem>();
            // request update information for complete portfolio
            var dailyInformations = (await stockTicker.GetDailyInformationForShareAsync(
                _portfolioItems.Values
                .Select(x => x.Symbol)
                .Distinct()))
                .ToList();
            // find the portfolio items that were updated
            foreach (var dailyInformation in dailyInformations)
            {
                foreach (var portfolioItemEntry in _portfolioItems)
                {
                    var portfolioItem = portfolioItemEntry.Value;
                    var accountItemId = portfolioItemEntry.Key;
                    if (portfolioItem.Symbol == dailyInformation.Symbol)
                    {
                        if (portfolioItem.Update(dailyInformation))
                            changedAccountItems.Add(ToReadonlyItem(accountItemId));
                    }
                }
            }
            if(changedAccountItems.Any()) // no need to fire update if no data was retrieved
               AccountItemsUpdated?.Invoke(changedAccountItems);
        }

        public void Clear() => _portfolioItems.Clear();

        public void AddAccountItemFromPersistentStorage(AccountItemDto dto)
        {
            var portfolioItem = PortfolioItem.CreateFromStorage(dto);
            _portfolioItems.Add(new AccountItemId(), portfolioItem);
        }

        public PortfolioDto ToDto()
        {
            return new PortfolioDto
            {
                AccountItems = _portfolioItems.Values.Select(x => new AccountItemDto
                {
                    BuyingDate = x.BuyingDate,
                    BuyingRate = x.BuyingRate,
                    StopRate = x.StopRate,
                    Symbol = x.Symbol,
                    Name = x.ShareName
                }).ToList(),
                LastChange = TimeOfLastChange ?? DateTime.MinValue
            };
        }

        public IEnumerable<AccountItemId> GetItemsNotInDto(PortfolioDto dto)
        {
            var ids = new List<AccountItemId>();
            foreach (var item in _portfolioItems)
            {
                var found = false;
                foreach (var dtoItem in dto.AccountItems)
                {
                    if (item.Value.Symbol == dtoItem.Symbol && 
                        item.Value.BuyingDate == dtoItem.BuyingDate)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    ids.Add(item.Key);
            }
            return ids;
        }

        public bool HasItem(string symbolName, DateTime buyingDate)
        {
            return _portfolioItems.Values.Any(x => x.Symbol == symbolName && x.BuyingDate == buyingDate);
        }

        public void RegisterSerializerForChanges(PortfolioSerializer serializer)
        {
            AccountItemAdded += async _ => await serializer.SaveAsync(this);
            AccountItemRemoved += async _ => await serializer.SaveAsync(this);
            AccountItemsUpdated += async _ => await serializer.SaveAsync(this);
        }
    }
}
