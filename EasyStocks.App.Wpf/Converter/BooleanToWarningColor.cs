using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using EasyStocks.Platform;

namespace EasyStocks.Converter
{
    public class BooleanToWarningColor : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool) value ? CustomColors.FailureBrush : CustomColors.NeutralBrush;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}