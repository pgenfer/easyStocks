using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using EasyStocks.Platform;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf.Converter
{
    public class RateChangeToTextColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rateChange = (RateChange)value;
            return rateChange == RateChange.NoChange
                ? CustomColors.PrimaryTextColorBrush
                : CustomColors.PrimaryTextLightColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
