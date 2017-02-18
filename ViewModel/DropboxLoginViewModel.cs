using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dropbox.Api;
using EasyStocks.Settings;

namespace EasyStocks.ViewModel
{
    public class DropboxLoginViewModel : AwaitableViewModel
    {
        private readonly INavigationService _navigationService;
        private ApplicationSettings _parameter;
        private Uri _loginPage;
        private const string ClientId = "nbj6k8wisphhuej";
        private const string RedirectUri = "https://www.dropbox.com/1/oauth2/redirect_receiver";
        private readonly string _authorizationState = Guid.NewGuid().ToString("N");

        public DropboxLoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ApplicationSettings Parameter
        {
            set { Setup(value); }
        }

        private void Setup(ApplicationSettings parameter)
        {
            _parameter = parameter;
            DisplayName = EasyStocksStrings.LoginToDropbox;

            LoginPage = DropboxOAuth2Helper.GetAuthorizeUri(
                OAuthResponseType.Token,
                ClientId,
                RedirectUri,
                _authorizationState,
                true);
        }

        public void CheckLogin(string url)
        {
            if (!url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                // navigation did not go the redirect uri, so ignore it
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(url));

                if (result.State != _authorizationState)
                {
                    // login process was not initiated by us, so ignore it
                    return;
                }

                var token = result.AccessToken;
                _parameter.DropBoxToken = token;
                // login successful, proceed with storage configuration
                TryClose();
                _navigationService.NavigateToPortfolio();
            }
            catch (ArgumentException ex)
            {
                // There was an error in the URI passed to ParseTokenFragment
                // do we need to show an error message here?

                // It is also strange that the WPF web browser sends the redirect URI
                // twice, first time with the token, second time without
                // for the moment we will ignore this here, but later we could fix it
                // TODO: on WPF, stop navigation processing after token was received.
                // TODO: show error in case something went wrong here
            }
        }

        public Uri LoginPage
        {
            get { return _loginPage; }
            private set
            {
                if (value == _loginPage) return;
                _loginPage = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
