using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.App.Platform;
using EasyStocks.Error;
using Xamarin.Forms;

namespace EasyStocks.App.Converter
{
    public class SeverityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Severity) value) == Severity.Critical ? CustomColors.FailureColor : CustomColors.WarningColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
