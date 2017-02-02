using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Dto;
using EasyStocks.Model;
using EasyStocks.Model.Account;
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
            // must be registered for every interface
            var portfolio = new PortfolioRepository();
            container.Instance(portfolio);
            container.Instance<IPortfolioRepository>(portfolio);
            container.Instance<IPortfolioPersistentRepository>(portfolio);
            container.Instance<IPortfolioUpdateRepository>(portfolio);

            var storage = container.GetInstance<IStorage>();
            // serializer used to save portfolio data
            var serializer = new PortfolioSerializer(storage);

            // save the portfolio whenever it changes
            portfolio.AccountItemAdded += async _ => await serializer.SaveAsync(portfolio);
            portfolio.AccountItemRemoved += async _ => await serializer.SaveAsync(portfolio);
            portfolio.AccountItemsUpdated += async _ => await serializer.SaveAsync(portfolio);
        }

        private static void SetupViewModel(SimpleContainer container)
        {
            container.Singleton<MainViewModel>();
            container.Singleton<SearchShareViewModel>();
            // important: don't forget to register all view models
            // https://github.com/Caliburn-Micro/Caliburn.Micro/issues/182
            container.PerRequest<AccountItemCreateViewModel>();
            container.PerRequest<AccountItemEditViewModel>();
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
            var portfolio = Container.GetInstance<PortfolioRepository>();
            var stockTicker = Container.GetInstance<IStockTicker>();

            var deserializer = new PortfolioSerializer(storage);
            await deserializer.LoadAsync(portfolio);
            await portfolio.CheckForUpdatesAsync(stockTicker);
            portfolio.FirePortfolioLoaded();
        }

        public void StartNotification()
        {
            var portfolio = Container.GetInstance<IPortfolioUpdateRepository>();
            var stockTicker = Container.GetInstance<IStockTicker>();
            var portfolioUpdater = new PortfolioUpdater(portfolio, stockTicker);
            portfolioUpdater.StartUpdate();
        }
    }
}
