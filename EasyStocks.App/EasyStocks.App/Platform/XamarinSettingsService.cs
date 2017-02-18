using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Settings;
using Plugin.Settings;

namespace EasyStocks.App.Platform
{
    public class XamarinSettingsService : ISettingsService
    {
        public T GetValueOrDefault<T>(string key, T @default = default(T)) => CrossSettings.Current.GetValueOrDefault(key, @default);
        public void SetValue<T>(string key, T value) => CrossSettings.Current.AddOrUpdateValue(key, value);
    }
}
