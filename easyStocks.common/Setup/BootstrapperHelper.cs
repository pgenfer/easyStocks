﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Dto;
using EasyStocks.Error;
using EasyStocks.Model;
using EasyStocks.Model.Account;
using EasyStocks.Network;
using EasyStocks.Settings;
using EasyStocks.Storage;
using EasyStocks.Storage.Dropbox;
using EasyStocks.ViewModel;
using EasyStocks.Model.StockTicker;

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
            var errorService = container.GetInstance<IErrorService>();
            // stock ticker for retrieving stock information
            //var stockTicker = new YahooFinanceStockTicker(errorService);
            var stockTicker = new YahooQueryTicker(errorService,container.GetInstance<StockNameRepository>());
            container.Instance<IStockTicker>(stockTicker);
            // portfolio where stocks are stored
            // must be registered for every interface
            var portfolio = new PortfolioRepository();
            container.Instance(portfolio);
            container.Instance<IPortfolioRepository>(portfolio);
            container.Instance<IPortfolioPersistentRepository>(portfolio);
        }

        private static void SetupViewModel(SimpleContainer container)
        {
            container.Singleton<MainViewModel>();
            container.Singleton<SearchShareViewModel>();
            // important: don't forget to register all view models
            // https://github.com/Caliburn-Micro/Caliburn.Micro/issues/182
            container.PerRequest<AccountItemCreateViewModel>();
            container.PerRequest<AccountItemEditViewModel>();
            container.Singleton<StorageSelectionViewModel>();
            container.Singleton<DropboxLoginViewModel>();
        }

        public void SetupContainer()
        {
            // register error service as soon as possible to track all problems
            // during setup
            Container.Instance(new StockNameRepository());
            Container.Instance<IErrorService>(new ErrorService());
            RegisterPlatformDependentServices(Container);
            SetupApplicationSettings(Container);
            SetupModel(Container);
            SetupViewModel(Container);
        }

        private static void SetupApplicationSettings(SimpleContainer container)
        {
            var settingsService = container.GetInstance<ISettingsService>();
            var settings = new ApplicationSettings(settingsService);
            container.Instance(settings);
        }

        public async Task StartApplication()
        {
            await SetupStorage(Container);
            await LoadModelFromStorage();
            StartNotification();
        }

        /// <summary>
        /// loads the portfolio data from the storage 
        /// and also tries to load stock data initially.
        /// </summary>
        /// <returns></returns>
        private async Task LoadModelFromStorage()
        {
            var storage = Container.GetInstance<IStorage>();
            var portfolio = Container.GetInstance<PortfolioRepository>();
            var deserializer = new PortfolioSerializer(storage);
            await deserializer.LoadAsync(portfolio);
            // register the names in the name
            var nameRepository = Container.GetInstance<StockNameRepository>();
            nameRepository.RegisterNames(portfolio.GetAllAccountItems());
            portfolio.FirePortfolioLoaded();
        }

        /// <summary>
        /// checks what storage to use for the portfolio.
        /// If no settings were stored so far, the user is ask
        /// </summary>
        private async Task CheckStorageForFirstTime()
        {
            // check if we need to ask the user for the storage first
            // or we do not have a valid token
            var settings = Container.GetInstance<ApplicationSettings>();
            if (!settings.StorageWasSetup || string.IsNullOrEmpty(settings.DropBoxToken))
            {
                var navigationService = Container.GetInstance<INavigationService>();
                await navigationService.NavigateToStorageSelection(settings);
                // now if the user choses the Dropbox storage, let him login
                if (settings.StorageType == StorageType.DropBox)
                {
                    await navigationService.NavigateToDropBoxLogin(settings);
                }
                settings.StorageWasSetup = true;
            }
        }

        /// <summary>
        /// setups the storage type that is used to load and store the portfolio.
        /// </summary>
        /// <param name="container"></param>
        private async Task SetupStorage(SimpleContainer container)
        {
            await CheckStorageForFirstTime();

            var settings = container.GetInstance<ApplicationSettings>();
            IStorage storageToUse;
            if (settings.StorageType == StorageType.Local)
            {
                storageToUse = container.GetInstance<IFileSystemStorage>();
            }
            else
            {
                // token provider is registered at location where assetmanager is available
                var fileStorage = container.GetInstance<IFileSystemStorage>();
                var errorService = container.GetInstance<IErrorService>();
                // use drop box as default storage and the file system as failover
#if DEBUG
                IStorage realStorage = new DebugDropBoxStorage(settings.DropBoxToken,new ThrowExceptionErrorService());
#else
                IStorage realStorage = new DropBoxStorage(settings.DropBoxToken,new ThrowExceptionErrorService());
#endif
                storageToUse =
                    new StorageWithBackupStrategy(
                        realStorage,
                        fileStorage,
                        errorService,
                        settings);
            }

            container.Instance(storageToUse);
        }

        /// <summary>
        /// starts notification for the portfolio
        /// and also hooks up the serializer to get notified whenever the portfolio
        /// changes.
        /// </summary>
        private void StartNotification()
        {
            // start with observing the network connection
            var connectivityManager = Container.GetInstance<IConnectivityService>();
            
            // save whenever the portfolio changes
            var storage = Container.GetInstance<IStorage>();
            var portfolio = Container.GetInstance<PortfolioRepository>();
            // serializer used to save portfolio data
            var serializer = new PortfolioSerializer(storage);
            // save the portfolio whenever it changes
            portfolio.RegisterSerializerForChanges(serializer);

            // start update notification process for the portfolio
            var stockTicker = Container.GetInstance<IStockTicker>();
            var portfolioUpdater = new PortfolioUpdater(portfolio, stockTicker, storage);
            connectivityManager.ConnectivityChanged += portfolioUpdater.ReactOnNetworkChanges;

            // start observing the network, this should also start the update process of the portfolio
            connectivityManager.StartObserving();
        }
    }
}
