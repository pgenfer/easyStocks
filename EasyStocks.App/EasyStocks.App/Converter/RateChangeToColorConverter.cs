using System;
using System.Globalization;
using EasyStocks.App.Platform;
using EasyStocks.ViewModel;
using Xamarin.Forms;

namespace EasyStocks.Converter
{
    public class RateChangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rateChange = (RateChange)value;
            return rateChange == RateChange.NoChange
                ? CustomColors.NeutralColor
                : rateChange == RateChange.Negative
                    ? CustomColors.FailureColor
                    : CustomColors.OkColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
