using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Network;

namespace EasyStocks.App.Wpf.Platform
{
    /// <summary>
    /// wpf connection service assumes that there is always a network connection on a desktop machine
    /// so no need to check for connection.
    /// </summary>
    public class WpfConnectivityService : IConnectivityService
    {
        public Connectivity CurrentConnectivity { get; private set; } = Connectivity.None;
        public event Action<Connectivity> ConnectivityChanged;
        public void StartObserving()
        {
            CurrentConnectivity = Connectivity.Wifi;
            ConnectivityChanged?.Invoke(CurrentConnectivity);
        }

        public void StopObserving()
        {
            CurrentConnectivity = Connectivity.None;
            ConnectivityChanged?.Invoke(CurrentConnectivity);
        }
    }
}
