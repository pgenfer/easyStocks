using System;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using EasyStocks.Setup;
using EasyStocks.View;
using EasyStocks.ViewModel;
using Xamarin.Forms;
using INavigationService = Caliburn.Micro.Xamarin.Forms.INavigationService;

namespace EasyStocks.App
{
    public partial class App
    {
        private readonly BootstrapperHelper _bootstrapper;

        public App(BootstrapperHelper bootstrapper)
        {
            InitializeComponent();

            _bootstrapper = bootstrapper;
            Initialize();
        }

        private void SetupViewLocators()
        {
            // mapping viewmodel and views from different assemblies
            // can be difficult, so configure the mapping explicitly:
            // http://www.jerriepelser.com/blog/split-views-and-viewmodels-in-caliburn-micro/
            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViews = nameof(View),
                DefaultSubNamespaceForViewModels = nameof(ViewModel)
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);
        }

        protected override async void OnStart()
        {
            SetupViewLocators();
            // this is a bit weird, but in order to have a working navigation, you have
            // to start with a View and not with a ViewModel. See here:
            // https://github.com/Caliburn-Micro/Caliburn.Micro/issues/71
            try
            {
                DisplayRootView<MainView>();
            }
            catch (Exception ex)
            {

            }
            
            await _bootstrapper.LoadModelFromStorage();
            _bootstrapper.StartNotification();
        }

        protected override void PrepareViewFirst(NavigationPage navigationPage)
        {
            _bootstrapper.Container.Instance<INavigationService>(new NavigationPageAdapter(navigationPage));
        }
    }
}
