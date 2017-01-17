using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Caliburn.Micro;
using EasyStocks.App.Droid.Platform;
using EasyStocks.Setup;

namespace EasyStocks.App.Droid
{
    [Activity(Label = "EasyStocks.App", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var bootstrapper = IoC.Get<BootstrapperHelper>();
            LoadApplication(new App(bootstrapper));
        }
    }
}

