using EasyStocks.Styles;
using Xamarin.Forms;

namespace EasyStocks.App.Platform
{
    public static class CustomColors
    {
        public static readonly Color NeutralColor = Color.Transparent;
        public static readonly Color OkColor = Color.FromRgb(ColorValues.Ok.Item1, ColorValues.Ok.Item2, ColorValues.Ok.Item3);
        public static readonly Color FailureColor = Color.FromRgb(ColorValues.Failure.Item1, ColorValues.Failure.Item2, ColorValues.Failure.Item3);
        public static readonly Color PrimaryColor = Color.FromHex(ColorValues.PrimaryColor);
        public static readonly Color AccentColor = Color.FromHex(ColorValues.AccentColor);
        public static readonly Color PrimaryTextColor = Color.FromHex(ColorValues.PrimaryTextColor);
        public static readonly Color SecondaryTextColor = Color.FromHex(ColorValues.SecondaryTextColor);
        public static readonly Color HintTextColor = Color.FromHex(ColorValues.HintColor);
        public static readonly Color PrimaryTextLightColor = Color.FromHex(ColorValues.PrimaryTextLightColor);
        public static readonly Color SecondaryTextLightColor = Color.FromHex(ColorValues.SecondaryTextLightColor);
        public static readonly Color DividerColor = Color.FromHex(ColorValues.DividerColor);
    }
}
