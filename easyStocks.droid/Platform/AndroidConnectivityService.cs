using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EasyStocks.Network;

namespace EasyStocks.App.Droid.Platform
{
    public class AndroidConnectivityService : BroadcastReceiver, IConnectivityService
    {
        private readonly ConnectivityManager _androidConnectivityManager;
        public Connectivity CurrentConnectivity { get; private set; } = Connectivity.None;

        public event Action<Connectivity> ConnectivityChanged;
        public void StartObserving() => Application.Context.RegisterReceiver(this, new IntentFilter(ConnectivityManager.ConnectivityAction));
        public void StopObserving() => Application.Context.UnregisterReceiver(this);

        public AndroidConnectivityService(ConnectivityManager androidConnectivityManager)
        {
            _androidConnectivityManager = androidConnectivityManager;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var networkInfo = _androidConnectivityManager.ActiveNetworkInfo;
            if(networkInfo == null)
                CurrentConnectivity = Connectivity.None;
            else if(!networkInfo.IsConnected)
                CurrentConnectivity = Connectivity.None;
            else if(networkInfo.Type == ConnectivityType.Wifi)
                CurrentConnectivity = Connectivity.Wifi;
            else
                CurrentConnectivity = Connectivity.Other;
            ConnectivityChanged?.Invoke(CurrentConnectivity);
        }
    }
}