using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class SessionsViewModel : ViewModelBase
    {
        private readonly IAccountStorage _accountStorage;

        public SessionsViewModel(IAccountStorage accountStorage)
        {
            _accountStorage = accountStorage;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var sessions = await _accountStorage.GetSessions();
                Sessions = new ObservableCollection<Session>(sessions);
            });
        }

        public ObservableCollection<Session> Sessions
        {
            get { return GetProperty<ObservableCollection<Session>>(); }
            set { SetProperty(value); }
        }
    }
}
