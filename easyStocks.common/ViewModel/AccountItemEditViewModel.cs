using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class AccountItemEditViewModel : Screen
    {
        private readonly IPortfolioRepository _portfolio;
        private readonly INavigationService _navigationService;
        public BindableCollection<EditableAccountItem> AccountItems { get; } = new BindableCollection<EditableAccountItem>();


        public ICommand ConfirmAccountItemChangesCommand { get; }
        
        public AccountItemEditViewModel(
            INavigationService navigationService,
            IPortfolioRepository portfolio)
        {
            _navigationService = navigationService;
            _portfolio = portfolio;
            ConfirmAccountItemChangesCommand = new SimpleCommand(ConfirmChanges, () => true);
        }

        public string DailyChangeString { get; private set; }
        public string DailyChangeInPercentString { get; private set; }

        public RateChange DailyTrend { get; private set; }
        public float CurrentRate { get; private set; }


        public string ShareName { get; private set; } 
        public string Symbol { get; private set; }

        private void Setup(IEnumerable<AccountItemId> accountItemIds)
        {
            var writableAccountItems = accountItemIds.Select(x => _portfolio.GetWriteableAccountItemById(x)).ToList();
            AccountItems.AddRange(writableAccountItems.Select(x => new EditableAccountItem(x,RemoveAccountItemFromPortfolio)));
            // first item is used to grab the daily data of the account item
            var firstItem = writableAccountItems.First();
            //_accountItem = _portfolio.GetWriteableAccountItemById(accountItemId);
            DisplayName = firstItem.ShareName;
            DailyTrend = firstItem.DailyTrend;
            DailyChangeInPercentString = firstItem.DailyChangeInPercent.ToPercentStringWithSign();
            DailyChangeString = firstItem.DailyChange.ToStringWithSign();
            CurrentRate = firstItem.CurrentRate;
            ShareName = firstItem.ShareName;
            Symbol = firstItem.Symbol;
        }

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public IEnumerable<AccountItemId> Parameter
        {
            set { Setup(value); }
        }

        public void RemoveAccountItemFromPortfolio(EditableAccountItem accountItem)
        {
            AccountItems.Remove(accountItem);
            _portfolio.RemoveAccountItem(accountItem.Id);
            if(AccountItems.Count == 0)
               _navigationService.NavigateToPortfolio();
        }

        public void ConfirmChanges()
        {
            foreach(var accountItem in AccountItems)
                _portfolio.WriteDataToAccountItem(new UserChangeableAccountData(
                    accountItem.Id,
                    accountItem.BuyingRate,
                    accountItem.BuyingDate));
            _navigationService.NavigateToPortfolio();
        }
    }
}
