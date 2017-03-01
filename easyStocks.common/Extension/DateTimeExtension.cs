using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Extension
{
    public static class DateTimeExtension
    {
        public static string ToDailyString(this DateTime dateTime)
        {
            if (dateTime.Year == DateTime.Now.Year)
            {
                var dayOfYear = dateTime.DayOfYear;
                if (dayOfYear == DateTime.Now.DayOfYear)
                    return EasyStocksStrings.Today;
                if (dayOfYear == DateTime.Now.DayOfYear - 1)
                    return EasyStocksStrings.Yesterday;
            }
            return dateTime.ToString("dddd");
        }
    }
}
