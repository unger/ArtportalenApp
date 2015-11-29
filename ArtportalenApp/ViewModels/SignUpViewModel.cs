using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IAccountStorage _accountStorage;
        private readonly INotificationCenter _notificationCenter;
        private Command _signUpCommand;

        public SignUpViewModel(IAccountStorage accountStorage, INotificationCenter notificationCenter)
        {
            _accountStorage = accountStorage;
            _notificationCenter = notificationCenter;
        }

        public string Fullname
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
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

        public Command SignUpCommand
        {
            get { return _signUpCommand ?? (_signUpCommand = new Command(async () =>
            {
                ErrorMessage = null;
                try
                {
                    await _accountStorage.SignUp(Fullname, Email, Password);

                    Navigation.ResetMainPage<MainPage, MainViewModel>(false);
                }
                catch (Exception e)
                {
                    ErrorMessage = e.Message;
                }
            })); }   
        }
    }
}
