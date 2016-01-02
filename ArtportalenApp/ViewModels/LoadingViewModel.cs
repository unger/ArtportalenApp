using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class LoadingViewModel : ViewModelBase
    {
        private Command _logInCommand;
        private Command _signUpCommand;

        public Command LogInCommand
        {
            get
            {
                return _logInCommand ?? (_logInCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<LogInPage, LogInViewModel>();
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
