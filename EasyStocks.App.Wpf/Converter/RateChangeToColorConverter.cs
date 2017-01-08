using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using EasyStocks.ViewModel;

namespace EasyStocks.Converter
{
    public class RateChangeToColorConverter : MarkupExtension, IValueConverter
    {
        private readonly SolidColorBrush _noChangeBrush = Brushes.Black;
        private readonly SolidColorBrush _positiveBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x88, 0x00));
        private readonly SolidColorBrush _negativeBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xCC, 0x00, 0x00));

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rateChange = (RateChange)value;
            return rateChange == RateChange.NoChange
                ? _noChangeBrush
                : rateChange == RateChange.Negative
                    ? _negativeBrush
                    : _positiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
