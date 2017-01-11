using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using EasyStocks.App.Wpf.Platform;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Wpf
{
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// container used for dependency injection
        /// </summary>
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            SetupModel();
            SetupViewModel();

            DisplayRootViewFor<MainViewModel>();
        }

        private async void SetupModel()
        {
            var stockTicker = new YahooFinanceStockTicker();
            _container.Instance<IStockTicker>(stockTicker);
            var portfolio = new Portfolio();
            _container.Instance(portfolio);

            var deserializer = new PortfolioDeserializer(stockTicker);
            await deserializer.LoadAsync(portfolio, new JsonStorage());

            var serializer = new PortfolioSerializer();
            // save the portfolio whenever a new item was added
            portfolio.AccountItemAdded += async _ => await serializer.SaveAsync(portfolio, new JsonStorage());
            portfolio.AccountDataChanged += async _ => await serializer.SaveAsync(portfolio, new JsonStorage());
            
            var portfolioUpdater = new PortfolioUpdater(portfolio, stockTicker);
            portfolioUpdater.StartUpdate();
        }

        private void SetupViewModel()
        {
            _container.PerRequest<MainViewModel>();
            _container.PerRequest<SearchShareViewModel>();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();
            _container.Singleton<IWindowManager, WindowManager>();

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

        protected override object GetInstance(Type service, string key) => _container.GetInstance(service, key);
        protected override IEnumerable<object> GetAllInstances(Type service) => _container.GetAllInstances(service);
        protected override void BuildUp(object instance) => _container.BuildUp(instance);
    }
}
