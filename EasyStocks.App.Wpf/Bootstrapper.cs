using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using EasyStocks.App.Wpf.Platform;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.Setup;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf
{
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// contains platform independent logic that can be
        /// reused during bootstrapping
        /// </summary>
        private BootstrapperHelper _bootstrapperLogic;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            SetupViewLocators();
            _bootstrapperLogic.SetupContainer();
            // show view models before loading model,
            // otherwise view models cannot register for model changes
            DisplayRootViewFor<MainViewModel>();
            await _bootstrapperLogic.LoadModelFromStorage();
            _bootstrapperLogic.StartNotification();
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

        protected override void Configure()
        {
            _bootstrapperLogic = new WpfBootstrapperHelper(new SimpleContainer());
            _bootstrapperLogic.Container.Singleton<IWindowManager, WindowManager>();
        }

        protected override object GetInstance(Type service, string key) => _bootstrapperLogic.Container.GetInstance(service, key);
        protected override IEnumerable<object> GetAllInstances(Type service) => _bootstrapperLogic.Container.GetAllInstances(service);
        protected override void BuildUp(object instance) => _bootstrapperLogic.Container.BuildUp(instance);
    }
}
