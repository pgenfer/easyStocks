using Caliburn.Micro;
using EasyStocks.App.Platform;
using EasyStocks.Dto;

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
            container.Instance<IStorage>(new AndroidFileStorage());
        }
    }
}
