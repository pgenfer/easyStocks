using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Network
{
    /// <summary>
    /// type of connectivity the application has
    /// with the network
    /// </summary>
    public enum Connectivity
    {
        /// <summary>
        /// there is no network connectivity at all
        /// </summary>
        None,
        /// <summary>
        /// application is connected via 
        /// WIFI or LAN connection
        /// </summary>
        Wifi,
        /// <summary>
        /// application is connected via other connection type.
        /// Although this might be blue tooth or something else,
        /// we should reduce the update interval here
        /// as it might cost the user additional traffic fees.
        /// </summary>
        Other
    }


    /// <summary>
    /// service used to control the current connectivity
    /// </summary>
    public interface IConnectivityService
    {
        /// <summary>
        /// the current connectivity mode of the device
        /// </summary>
        Connectivity CurrentConnectivity { get; }
        /// <summary>
        /// fired whenever the connectivity changes
        /// </summary>
        event Action<Connectivity> ConnectivityChanged;
        /// <summary>
        /// Connectivity service must be enabled explicitly
        /// </summary>
        void StartObserving();
        /// <summary>
        /// connectivity observation should be stopped if not required (e.g. if application is not active)
        /// </summary>
        void StopObserving();
    }
}
