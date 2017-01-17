using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using EasyStocks.Styles;

namespace EasyStocks.Platform
{
    public static class CustomColors
    {
        public static readonly Color NeutralColor = Color.FromRgb((byte)ColorValues.Neutral.Item1, (byte)ColorValues.Neutral.Item2, (byte)ColorValues.Neutral.Item3);
        public static readonly Color OkColor = Color.FromRgb((byte)ColorValues.Ok.Item1, (byte)ColorValues.Ok.Item2, (byte)ColorValues.Ok.Item3);
        public static readonly Color FailureColor = Color.FromRgb((byte)ColorValues.Failure.Item1, (byte)ColorValues.Failure.Item2, (byte)ColorValues.Failure.Item3);

        public static readonly SolidColorBrush NeutralBrush = new SolidColorBrush(NeutralColor);
        public static readonly SolidColorBrush OkBrush = new SolidColorBrush(OkColor);
        public static readonly SolidColorBrush FailureBrush = new SolidColorBrush(FailureColor);
    }
}
