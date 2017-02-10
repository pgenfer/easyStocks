using Android.Content.Res;
using Caliburn.Micro;
using EasyStocks.App.Platform;
using EasyStocks.Dto;
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
            base.RegisterPlatformDependentServices(container);
            // token provider is registered at location where assetmanager is available
            var tokenProvider = container.GetInstance<ITokenProvider>();
            // TODO: if file storage has a separate interface, it could be registered here
            // and the combined storage could be composed by the container
            container.Instance<IStorage>(new CombinedStorage(new AndroidFileStorage(), new DropBoxStorage(tokenProvider)));
        }
    }
}
