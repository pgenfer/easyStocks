using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Settings;
using EasyStocks.Setup;
using Xamarin.Forms;

namespace EasyStocks.App.Platform
{
    public class XamarinFormsBootstrapper : BootstrapperHelper
    {
        public XamarinFormsBootstrapper(SimpleContainer container) : base(container)
        {
        }

        protected override void RegisterPlatformDependentServices(SimpleContainer container)
        {
            container.Singleton<ViewModel.INavigationService, XamarinNavigationServiceAdapter>();
            container.Singleton<ISettingsService, XamarinSettingsService>();
        }
    }
}
