using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Setup;
using EasyStocks.Storage;
using EasyStocks.Storage.Dropbox;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfBootstrapperHelper : BootstrapperHelper
    {
        public WpfBootstrapperHelper(SimpleContainer container) : base(container)
        {
            container.Instance(container);
        }

        protected override void SetupStorage(SimpleContainer container)
        {
            var tokenProvider = container.GetInstance<ITokenProvider>();
            var errorService = container.GetInstance<IErrorService>();
            // use drop box as default storage and the file system as failover
            container.Instance<IStorage>(new DropBoxStorage(tokenProvider, errorService));
        }

        protected override void RegisterPlatformDependentServices(SimpleContainer container)
        {
            container.Singleton<INavigationService, WpfNavigationService>();
            container.Instance<ITokenProvider>(new WpfTokenProvider());
        }
    }
}
