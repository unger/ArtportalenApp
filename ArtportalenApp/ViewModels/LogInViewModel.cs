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
    public class LogInViewModel : ViewModelBase
    {
        private readonly IAccountStorage _accountStorage;
        private Command _logInCommand;
        private Command _signUpCommand;

        public LogInViewModel(IAccountStorage accountStorage)
        {
            _accountStorage = accountStorage;
        }

        public string Email
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
                        var user = await _accountStorage.LogIn(Email, Password);

                        Navigation.ResetMainPage<MainPage, MainViewModel>(false);
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                    }
                }));
            }
        }

        public Command SignUpCommand
        {
            get
            {
                return _signUpCommand ?? (_signUpCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<SignUpPage, SignUpViewModel>();
                }));
            }
        }
    }
}
