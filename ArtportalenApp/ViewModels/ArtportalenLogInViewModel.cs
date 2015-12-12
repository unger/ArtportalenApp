using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Views;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ArtportalenLogInViewModel : ViewModelBase
    {
        private readonly IArtportalenService _artportalenService;
        private Command _logInCommand;

        public ArtportalenLogInViewModel(IArtportalenService artportalenService)
        {
            _artportalenService = artportalenService;
        }

        public string Username
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Password
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string ErrorMessage
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public Command LogInCommand
        {
            get
            {
                return _logInCommand ?? (_logInCommand = new Command(async () =>
                {
                    ErrorMessage = null;
                    try
                    {
                        await _artportalenService.SaveCredential(Username, Password);

                        await Navigation.PopModalAsync();
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                    }
                }));
            }
        }
    }
}
