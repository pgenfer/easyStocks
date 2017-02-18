using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using EasyStocks.ViewModel;

namespace EasyStocks.View
{
    partial class DropboxLoginView
    {
        public DropboxLoginView()
        {
            InitializeComponent();
            // source of web browser is not bindable
            // so we have to set it explicitly
            DataContextChanged += (s, e) =>
            {
                var viewModel = e.NewValue as DropboxLoginViewModel;
                if (viewModel != null)
                {
                    var loginPage = viewModel.LoginPage;
                    LoginWebBrowser.Source = loginPage;
                }
            };
        }

        private void WebBrowserOnNavigating(object sender, NavigationEventArgs e)
        {
            var viewModel = DataContext as DropboxLoginViewModel;
            viewModel?.CheckLogin(e.Uri.AbsoluteUri);
        }
    }
}
