using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Extension;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// viewmodel for representing the daily changing data of a stock
    /// </summary>
    public class DailyDataViewModel : PropertyChangedBase
    {
        private float ChangePercent
        {
            get { return _changePercent; }
            set
            {
                if (value.Equals(_changePercent)) return;
                _changePercent = value;
                PercentChanged?.Invoke(value);
                NotifyOfPropertyChange(nameof(ChangePercentString));
            }
        }

        private float _changeAbsolute;
        private float _currentRate;
        private RateChange _changeOfCurrentRate = RateChange.NoChange;
        private bool _isActualDataAvailable;
        private float _changePercent;

        public DailyDataViewModel(ShareDailyInformation info)
        {
            Update(info);
        }

        public void Update(ShareDailyInformation info)
        {
            _currentRate = info.Rate.Value;
            _changeAbsolute = info.ChangeAbsolute.Value;
            ChangePercent = info.ChangeInPercent.Value;

            // check if data is accurate
            IsActualDataAvailable = info.IsAccurate;

            NotifyOfPropertyChange(nameof(CurrentRateString));
            NotifyOfPropertyChange(nameof(ChangeAbsoluteString));
           
            ChangeOfCurrentRate =
                info.ChangeInPercent.IsPositive
                    ? RateChange.Positive
                    : info.ChangeInPercent.IsNegative
                        ? RateChange.Negative
                        : RateChange.NoChange;
        }

        /// <summary>
        /// specific event that is used to oberserve changes of the percentage change,
        /// so that the sorting of the portfolio items can be adjusted.
        /// </summary>
        public event Action<float> PercentChanged;

        public string ChangePercentString => $"{ChangePercent.WithSign()} %";
        public string ChangeAbsoluteString => _changeAbsolute.WithSign();
        public string CurrentRateString => _currentRate.ToString("N2");
        public string ChangeAbsoluteAndPercentString => $"{ChangeAbsoluteString} ({ChangePercentString})";

        public RateChange ChangeOfCurrentRate
        {
            get { return _changeOfCurrentRate; }
            private set
            {
                if (value == _changeOfCurrentRate) return;
                _changeOfCurrentRate = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsActualDataAvailable
        {
            get { return _isActualDataAvailable; }
            private set
            {
                if (value == _isActualDataAvailable) return;
                _isActualDataAvailable = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
