using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Storage;
using Xamarin.Forms;

namespace EasyStocks.App.Converter
{
    public class StorageTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var storageType = (StorageType)value;
            switch (storageType)
            {
                case StorageType.DropBox:
                    return new ImageSourceConverter().ConvertFromInvariantString("dropbox_logo.png");
                case StorageType.Local:
                    return new ImageSourceConverter().ConvertFromInvariantString("android_logo.png");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
