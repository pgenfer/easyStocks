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
        private float _changePercent;
        private float _changeAbsolute;
        private float _currentRate;
        private RateChange _changeOfCurrentRate = RateChange.NoChange;
        private bool _isActualDataAvailable;

        public DailyDataViewModel(ShareDailyInformation info)
        {
            Update(info);
        }

        public void Update(ShareDailyInformation info)
        {
            _currentRate = info.Rate.Value;
            _changeAbsolute = info.ChangeAbsolute.Value;
            _changePercent = info.ChangeInPercent.Value;

            // check if data is accurate
            IsActualDataAvailable = info.IsAccurate;

            NotifyOfPropertyChange(nameof(CurrentRateString));
            NotifyOfPropertyChange(nameof(ChangeAbsoluteString));
            NotifyOfPropertyChange(nameof(ChangePercentString));

            ChangeOfCurrentRate =
                info.ChangeInPercent.IsPositive
                    ? RateChange.Positive
                    : info.ChangeInPercent.IsNegative
                        ? RateChange.Negative
                        : RateChange.NoChange;
        }

        public string ChangePercentString => $"{_changePercent.WithSign()} %";
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
