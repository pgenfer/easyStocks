using System;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class EditableAccountItem : PropertyChangedBase
    {
        private readonly WritableAccountItem _accountItem;
        private readonly Action<EditableAccountItem> _deleteAction;

        public string OverallChangeInPercentString => _accountItem.OverallChangeInPercent.ToPercentStringWithSign();
        public string OverallChangeString => _accountItem.OverallChange.ToStringWithSign();
        public RateChange OverallTrend => _accountItem.OverallTrend;
        public float StopRate => _accountItem.StopRate;
        public bool IsStopRateReached => _accountItem.IsStopQuoteReached;

        /// <summary>
        /// command executed when an item is removed (currently only used in WPF application because
        /// mobile application uses swipe gesture
        /// </summary>
        public ICommand RemoveCommand => new SimpleCommand(() => _deleteAction(this),() => true);

        public AccountItemId Id => _accountItem.Id;

        public float BuyingRate
        {
            get { return _accountItem.BuyingRate; }
            set
            {
                _accountItem.BuyingRate = value;
                NotifyOfPropertyChange(nameof(OverallChangeInPercentString));
                NotifyOfPropertyChange(nameof(OverallChangeString));
                NotifyOfPropertyChange(nameof(OverallTrend));
            }
        }

        public DateTime BuyingDate
        {
            get { return _accountItem.BuyingDate; }
            set { _accountItem.BuyingDate = value; }
        }

        public EditableAccountItem(WritableAccountItem accountItem,Action<EditableAccountItem> deleteAction)
        {
            _accountItem = accountItem;
            _deleteAction = deleteAction;
        }
    }
}