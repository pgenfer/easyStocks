using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.ViewModel;
using Xamarin.Forms;

namespace EasyStocks.View
{
    public partial class DropboxLoginView
    {
        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            var viewModel = (DropboxLoginViewModel) BindingContext;
            viewModel?.CheckLogin(e.Url);
        }
    }
}
