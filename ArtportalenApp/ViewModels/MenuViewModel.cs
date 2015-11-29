using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IAccountStorage _accountStorage;
        private Command _sightingsCommand;
        private Command _rulesCommand;
        private Command _logOutCommand;
        private Command _sessionsCommand;
        private Command _reportsCommand;
        private Command _sitesCommand;

        public MenuViewModel(IAccountStorage accountStorage)
        {
            _accountStorage = accountStorage;
            Title = "Meny";
        }

        public IMasterDetailNavigation MasterNavigation { get; set; }

        public Command SightingsCommand
        {
            get
            {
                return _sightingsCommand ?? (_sightingsCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<SightingsPage, SightingsViewModel>();
                }));
            }
        }

        public Command RulesCommand
        {
            get
            {
                return _rulesCommand ?? (_rulesCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<RulesPage, RulesViewModel>();
                }));
            }
        }

        public Command SessionsCommand
        {
            get
            {
                return _sessionsCommand ?? (_sessionsCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<SessionsPage, SessionsViewModel>();
                }));
            }
        }

        public Command LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new Command(async () =>
                {
                    await _accountStorage.LogOut();
                    Navigation.ResetMainPage<LogInPage, LogInViewModel>(true);
                }));
            }
        }

        public Command ReportsCommand
        {
            get
            {
                return _reportsCommand ?? (_reportsCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<ReportsPage, ReportsViewModel>();
                }));
            }
        }

        public Command SitesCommand
        {
            get
            {
                return _sitesCommand ?? (_sitesCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<SitesPage, SitesViewModel>();
                }));
            }
        }
    }
}
