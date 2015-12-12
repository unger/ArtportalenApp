using System.Linq;
using System.Net;
using ArtportalenApp.Interfaces;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ArtportalenApp.Storage
{
    public class ArtportalenAccountStorage : IArtportalenAccountStorage
    {
        private readonly ICurrentUser _currentUser;
        const string ServiceId = "Artportalen"; 

        Account _account;
        readonly AccountStore _accountStore;

        public ArtportalenAccountStorage(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
#if __ANDROID__
            _accountStore = AccountStore.Create(Forms.Context);
#endif
#if __IOS__
            _accountStore = AccountStore.Create();
#endif
            _account = _accountStore.FindAccountsForService(ServiceId).FirstOrDefault(a => a.Username == _currentUser.Id);
        }

        public CookieContainer Cookies
        {
            get
            {
                if (_account != null)
                {
                    return _account.Cookies;
                }

                return null;
            }
        }

        public void SaveCookies()
        {
            if (_account != null)
            {
                _accountStore.Save(_account, ServiceId);
            }
        }

        public void SaveCredential(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                if (_account == null)
                {
                    _account = new Account();
                }
                _account.Username = _currentUser.Id;
                _account.Properties["Username"] = username;
                _account.Properties["Password"] = password;

                _accountStore.Save(_account, ServiceId);
            }
        }

        public void RemoveCredential()
        {
            _accountStore.Delete(_account, ServiceId);
            _account = null;
        }

        public NetworkCredential GetCredential()
        {
            if (_account != null)
            {
                return new NetworkCredential(_account.Properties["Username"], _account.Properties["Password"]);
            }

            return null;
        }

        public bool HasCrendental
        {
            get { return _account != null; }
        }
    }
}
