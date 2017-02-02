using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.ViewModel;
using Xamarin.Forms;

namespace EasyStocks.App.Converter
{
    /// <summary>
    /// provides the correct color depending on the color of the rate change,
    /// if rate change is negative/positive, a light color will be returned,
    /// if no change occured a dark color will be returned. A corresponding background
    /// color (red/green) can be choosen depending on the rate change.
    /// </summary>
    public class RateChangeToTextColorConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty LightColorProperty =
            BindableProperty.Create("LightColor", typeof(Color), typeof(RateChangeToTextColorConverter),Color.White);
        public static readonly BindableProperty DarkColorProperty =
            BindableProperty.Create("DarkColor", typeof(Color), typeof(RateChangeToTextColorConverter),Color.Black);

        public Color LightColor
        {
            get { return (Color)GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }

        public Color DarkColor
        {
            get { return (Color)GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var change = (RateChange) value;
            return change == RateChange.NoChange ? DarkColor : LightColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
