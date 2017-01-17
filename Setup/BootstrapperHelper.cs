using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.ViewModel;

namespace EasyStocks.Setup
{
    /// <summary>
    /// class that supports bootstrapping operations
    /// </summary>
    public abstract class BootstrapperHelper
    {
        public SimpleContainer Container { get; }

        protected BootstrapperHelper(SimpleContainer container)
        {
            Container = container;
        }
        
        protected abstract void RegisterPlatformDependentServices(SimpleContainer container);

        private static void SetupModel(SimpleContainer container)
        {
            // stock ticker for retrieving stock information
            var stockTicker = new YahooFinanceStockTicker();
            container.Instance<IStockTicker>(stockTicker);
            // portfolio where stocks are stored
            var portfolio = new Portfolio();
            container.Instance(portfolio);
            // serializer used to save portfolio data
            var serializer = new PortfolioSerializer();
            var storage = container.GetInstance<IStorage>();
            // save the portfolio whenever a new item was added
            portfolio.AccountItemAdded += async _ => await serializer.SaveAsync(portfolio, storage);
            portfolio.AccountItemRemoved += async _ => await serializer.SaveAsync(portfolio, storage);
            portfolio.AccountDataChanged += async _ => await serializer.SaveAsync(portfolio, storage);
        }

        private static void SetupViewModel(SimpleContainer container)
        {
            container.Singleton<MainViewModel>();
            container.Singleton<SearchShareViewModel>();
            // important: don't forget to register all view models
            // https://github.com/Caliburn-Micro/Caliburn.Micro/issues/182
            container.PerRequest<AccountItemCreateViewModel>();
        }

        public void SetupContainer()
        {
            RegisterPlatformDependentServices(Container);
            SetupModel(Container);
            SetupViewModel(Container);
        }

        public async Task LoadModelFromStorage()
        {
            var storage = Container.GetInstance<IStorage>();
            var portfolio = Container.GetInstance<Portfolio>();
            var stockTicker = Container.GetInstance<IStockTicker>();

            var deserializer = new PortfolioDeserializer(stockTicker);
            await deserializer.LoadAsync(portfolio, storage);
        }

        public void StartNotification()
        {
            var portfolio = Container.GetInstance<Portfolio>();
            var stockTicker = Container.GetInstance<IStockTicker>();
            var portfolioUpdater = new PortfolioUpdater(portfolio, stockTicker);
            portfolioUpdater.StartUpdate();
        }
    }
}
