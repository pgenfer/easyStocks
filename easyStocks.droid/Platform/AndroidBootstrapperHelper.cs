using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Net;
using Caliburn.Micro;
using EasyStocks.App.Platform;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Network;
using EasyStocks.Storage;
using EasyStocks.Storage.Dropbox;

namespace EasyStocks.App.Droid.Platform
{
    public class AndroidBootstrapperHelper : XamarinFormsBootstrapper
    {
        public AndroidBootstrapperHelper(SimpleContainer container) : base(container)
        {
        }

        protected override void RegisterPlatformDependentServices(SimpleContainer container)
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

            container.Instance<IConnectivityService>(new AndroidConnectivityService(connectivityManager));
            container.Instance<IFileSystemStorage>(new AndroidFileStorage(new ThrowExceptionErrorService()));
            base.RegisterPlatformDependentServices(container);
        }
    }
}
