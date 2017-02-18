using EasyStocks.Storage;

namespace EasyStocks.Settings
{
    /// <summary>
    /// Access to the user settings. The class uses
    /// the ISettingsService for reading/writing the settings,
    /// in that way different implementations of the SettingsService
    /// can be used.
    /// </summary>
    public class ApplicationSettings
    {
        private readonly ISettingsService _settingService;

        public ApplicationSettings(ISettingsService settingService)
        {
            _settingService = settingService;
        }

        public string DropBoxToken
        {
            get { return _settingService.GetValueOrDefault<string>(nameof(DropBoxToken)); }
            set { _settingService.SetValue(nameof(DropBoxToken),value);}
        }

        /// <summary>
        /// flag is used to determine whether the
        /// user has setup his storage already once after the application
        /// did startup
        /// </summary>
        public bool StorageWasSetup
        {
            get { return _settingService.GetValueOrDefault<bool>(nameof(StorageWasSetup)); }
            set { _settingService.SetValue(nameof(StorageWasSetup),value);}
        }

        /// <summary>
        /// remember which type of storage is used for the portfolio.
        /// </summary>
        public StorageType StorageType
        {
            get { return (StorageType)_settingService.GetValueOrDefault<int>(nameof(StorageType)); }
            set { _settingService.SetValue(nameof(StorageType), (int)value); }
        }
    }
}