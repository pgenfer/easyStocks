using Android.Content.Res;
using Caliburn.Micro;
using EasyStocks.App.Platform;
using EasyStocks.Dto;
using EasyStocks.Error;
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
            container.Instance<IFileSystemStorage>(new AndroidFileStorage(new ThrowExceptionErrorService()));
            base.RegisterPlatformDependentServices(container);
        }
    }
}
