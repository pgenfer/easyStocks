using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.App.Platform;
using Xamarin.Forms;

namespace EasyStocks.App.Converter
{
    /// <summary>
    /// converter changes the color of a label depending whether the stop rate
    /// of the account item was reached. Another converter is responsible for changing
    /// the background color. User can define the color when creating the converter.
    /// </summary>
    public class ColorToStopRateReachedConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty StopRateReachedColorProperty =
           BindableProperty.Create("StopRateReachedColor", typeof(Color), typeof(ColorToStopRateReachedConverter), Color.White);
        public static readonly BindableProperty StopRateNotReachedColorProperty =
            BindableProperty.Create("StopRateNotReachedColor", typeof(Color), typeof(RateChangeToTextColorConverter), Color.Black);

        public Color StopRateReachedColor
        {
            get { return (Color)GetValue(StopRateReachedColorProperty); }
            set { SetValue(StopRateReachedColorProperty, value); }
        }

        public Color StopRateNotReachedColor
        {
            get { return (Color)GetValue(StopRateNotReachedColorProperty); }
            set { SetValue(StopRateNotReachedColorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? StopRateReachedColor : StopRateNotReachedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
