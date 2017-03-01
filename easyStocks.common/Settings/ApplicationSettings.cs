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

        public virtual string DropBoxToken
        {
            get { return _settingService.GetValueOrDefault<string>(nameof(DropBoxToken)); }
            set { _settingService.SetValue(nameof(DropBoxToken),value);}
        }

        /// <summary>
        /// flag is used to determine whether the
        /// user has setup his storage already once after the application
        /// did startup
        /// </summary>
        public virtual bool StorageWasSetup
        {
            get { return _settingService.GetValueOrDefault<bool>(nameof(StorageWasSetup)); }
            set { _settingService.SetValue(nameof(StorageWasSetup),value);}
        }

        /// <summary>
        /// remember which type of storage is used for the portfolio.
        /// </summary>
        public virtual StorageType StorageType
        {
            get { return (StorageType)_settingService.GetValueOrDefault<int>(nameof(StorageType)); }
            set { _settingService.SetValue(nameof(StorageType), (int)value); }
        }

        /// <summary>
        /// this flag is set whenever data was stored in the backupstorage.
        /// When starting the next time and the flag is set, the data from the backupstorage
        /// needs to be retrieved and synced with the default storage. After that, the backupstorage
        /// can be erased.
        /// </summary>
        public virtual bool BackupStorageWasUsed
        {
            get { return _settingService.GetValueOrDefault<bool>(nameof(BackupStorageWasUsed)); }
            set { _settingService.SetValue(nameof(BackupStorageWasUsed), value); }
        }
    }
}