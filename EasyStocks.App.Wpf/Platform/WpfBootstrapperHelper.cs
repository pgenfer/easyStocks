using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Dto;
using EasyStocks.Setup;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfBootstrapperHelper : BootstrapperHelper
    {
        public WpfBootstrapperHelper(SimpleContainer container) : base(container)
        {
            container.Instance(container);
        }

        protected override void RegisterPlatformDependentServices(SimpleContainer container)
        {
            container.Singleton<IStorage, JsonStorage>();
            container.Singleton<INavigationService, WpfNavigationService>();
        }
    }
}
