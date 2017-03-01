using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Settings;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfSettingsService : ISettingsService
    {
        private readonly Properties.Settings _settings;

        public WpfSettingsService()
        {
            _settings = Properties.Settings.Default;
        }

        public T GetValueOrDefault<T>(string key, T @default = default(T))
        {
            try
            {
                var result = (T)_settings[key];
                return result;
            }
            catch (SettingsPropertyNotFoundException) // setting was not created yet
            {
                CreateSetting(key, @default);
                return @default;
            }
        }

        private void CreateSetting<T>(string key, T value)
        {
            var property = new SettingsProperty(key)
            {
                DefaultValue = value,
                IsReadOnly = false,
                PropertyType = typeof(T),
                Provider = _settings.Providers["LocalFileSettingsProvider"]
            };

            property.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());
            _settings.Properties.Add(property);
            _settings.Save();
        }

        public void SetValue<T>(string key, T value)
        {
            try
            {
                _settings[key] = value;
            }
            catch (SettingsPropertyNotFoundException) // setting was not created yet
            {
                CreateSetting(key, value);
            }
            finally
            {
                _settings.Save();
            }
        }
    }
}
