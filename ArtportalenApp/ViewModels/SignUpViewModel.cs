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
        private readonly ICurrentUser _currentUser;
        private Command _signUpCommand;

        public SignUpViewModel(IAccountStorage accountStorage, INotificationCenter notificationCenter,
            ICurrentUser currentUser)
        {
            _accountStorage = accountStorage;
            _notificationCenter = notificationCenter;
            _currentUser = currentUser;
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
            get
            {
                return _signUpCommand ?? (_signUpCommand = new Command(async () =>
                {
                    ErrorMessage = null;
                    try
                    {
                        await _accountStorage.SignUp(Fullname, Email, Password);

                        if (_currentUser.IsAutenticated)
                        {
                            Navigation.ResetMainPage<MainPage, MainViewModel>(false);
                        }
                        else
                        {
                            ErrorMessage = "Något gick fel försök igen";
                        }
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
