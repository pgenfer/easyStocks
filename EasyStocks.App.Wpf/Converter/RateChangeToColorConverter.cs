using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using EasyStocks.Platform;
using EasyStocks.ViewModel;

namespace EasyStocks.Converter
{
    public class RateChangeToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rateChange = (RateChange)value;
            return rateChange == RateChange.NoChange
                ? Brushes.Transparent 
                : rateChange == RateChange.Negative
                    ? CustomColors.FailureBrush
                    : CustomColors.OkBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
