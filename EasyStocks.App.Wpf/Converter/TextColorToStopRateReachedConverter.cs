using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using EasyStocks.App.Wpf.Converter;
using EasyStocks.Platform;

namespace EasyStocks.App.Converter
{
    /// <summary>
    /// converter changes the color of a label depending whether the stop rate
    /// of the account item was reached. Another converter is responsible for changing
    /// the background color. User can define the color when creating the converter.
    /// </summary>
    public class ColorToStopRateReachedConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? CustomColors.FailureBrush : CustomColors.PrimaryTextColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
