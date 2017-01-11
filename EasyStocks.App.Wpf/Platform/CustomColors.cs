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
        public static readonly Color NeutralColor = Colors.Black;
        public static readonly Color OkColor = Color.FromArgb(0xFF, 0x00, 0x88, 0x00);
        public static readonly Color FailureColor = Color.FromArgb(0xFF, 0xCC, 0x00, 0x00);
        public static readonly Color OkHighlightColor = Color.FromArgb(0xFF, 0x00, 0xCC, 0x00);

        public static readonly SolidColorBrush NeutralBrush = new SolidColorBrush(NeutralColor);
        public static readonly SolidColorBrush OkBrush = new SolidColorBrush(OkColor);
        public static readonly SolidColorBrush FailureBrush = new SolidColorBrush(FailureColor);
    }
}
