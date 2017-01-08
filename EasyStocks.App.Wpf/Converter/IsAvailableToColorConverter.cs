using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace EasyStocks.Converter
{
    public class IsAvailableToColorConverter : MarkupExtension, IValueConverter
    {
        private readonly SolidColorBrush _isEnabledBrush = Brushes.Black;
        private readonly SolidColorBrush _isDisabledBrush = Brushes.LightGray;


        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool) value ? _isEnabledBrush : _isDisabledBrush;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
