using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model that can be used to edit the variable
    /// parts of an account item, the daily data are also part of
    /// this viewmodel but cannot be changed by the user
    /// </summary>
    public class AccountItemDataViewModel : PropertyChangedBase
    {
        /// <summary>
        /// daily data are also part of this view model
        /// </summary>
        private readonly DailyDataViewModel _dailyData;

        private DateTime _buyingDate;
        private float _buyingRate;
        private float _stopRate;
        private string _shareName;
   
        public string ShareName
        {
            get { return _shareName; }
            set
            {
                if (value == _shareName) return;
                _shareName = value;
                NotifyOfPropertyChange();
            }
        }

        public string Symbol { get; }

        public DateTime BuyingDate
        {
            get { return _buyingDate; }
            set
            {
                if (value.Equals(_buyingDate)) return;
                _buyingDate = value;
                NotifyOfPropertyChange();
            }
        }

        public string BuyingRateString
        {
            get { return _buyingRate.ToString("N2"); }
            set
            {
                _buyingRate = float.Parse(value);
                NotifyOfPropertyChange();
            }
        }

        public float BuyingRate
        {
            get { return _buyingRate; }
            set
            {
                if (value.Equals(_buyingRate)) return;
                _buyingRate = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(BuyingRateString));
            }
        }

        public float StopRate
        {
            get { return _stopRate; }
            set
            {
                if (value.Equals(_stopRate)) return;
                _stopRate = value;
                NotifyOfPropertyChange();
            }
        }

        public AccountItemDataViewModel(
            Share share,
            DateTime buyingDate, 
            float buyingRate, 
            float stopRate)
        {
            Symbol = share.Symbol;
            ShareName = share.Name;
            BuyingDate = buyingDate;
            BuyingRate = buyingRate;
            StopRate = stopRate;

            _dailyData = new DailyDataViewModel(share.DailyData);
            _dailyData.PropertyChanged += (s, e) => NotifyOfPropertyChange(e.PropertyName);
            _dailyData.PercentChanged += x => PercentChanged?.Invoke(x);
        }

        /// <summary>
        /// event is triggered when the daily percentage has changed,
        /// AccountItemViewModel can react on this and fire a property update
        /// </summary>
        public event Action<float> PercentChanged;

        public string NameAndSymbol => $"{ShareName} ({Symbol})";

        public void RecalculateStopRate()
        {
            // TODO: this should be done by the business layer?
        }

        public string ChangePercentString => _dailyData.ChangePercentString;
        public string ChangeAbsoluteString => _dailyData.ChangeAbsoluteString;
        public string CurrentRateString => _dailyData.CurrentRateString;
        public string ChangeAbsoluteAndPercentString => _dailyData.ChangeAbsoluteAndPercentString;
        public RateChange ChangeOfCurrentRate => _dailyData.ChangeOfCurrentRate;
        public bool IsActualDataAvailable => _dailyData.IsActualDataAvailable;
        public void Update(ShareDailyInformation info) => _dailyData.Update(info);
    }
}
