using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EasyStocks.Storage.Dropbox;
using static Android.InputMethodServices.InputMethodService;

namespace EasyStocks.App.Droid.Platform
{
    public class AndroidTokenProvider : ITokenProvider
    {
        public AndroidTokenProvider(AssetManager assetManager)
        {
            using (var streamReader = new StreamReader(assetManager.Open("dropbox.token")))
            {
                Token = streamReader.ReadToEnd();
            }
        }

        public string Token { get; }
    }
}