using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Storage.Dropbox;

namespace EasyStocks.App.Wpf.Platform
{
    public class WpfTokenProvider : ITokenProvider
    {
        public string Token { get; }

        public WpfTokenProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            using (var streamReader = new StreamReader(assembly.GetManifestResourceStream("EasyStocks.App.Wpf.Resources.dropbox.token")))
            {
                Token = streamReader.ReadToEnd();
            }
        }
    }
}
