using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EasyStocks.Platform
{
    public static class CustomColors
    {
        public static readonly SolidColorBrush NeutralBrush = Brushes.Black;
        public static readonly SolidColorBrush OkBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x88, 0x00));
        public static readonly SolidColorBrush FailureBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xCC, 0x00, 0x00));
    }
}
