using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Settings
{
    /// <summary>
    /// setting interface is needed so that
    /// we can use different implementations on 
    /// mobile devices and desktop
    /// </summary>
    public interface ISettingsService
    {
        T GetValueOrDefault<T>(string key,T @default = default(T));
        void SetValue<T>(string key, T value);
    }
}
