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
        private readonly IArtportalenService _artportalenService;
        private Command _sightingsCommand;
        private Command _rulesCommand;
        private Command _logOutCommand;
        private Command _sessionsCommand;
        private Command _myReportsCommand;
        private Command _sitesCommand;
        private Command _disconnectFromArtportalenCommand;
        private Command _connectToArtportalenCommand;
        private object _newReportCommand;

        public MenuViewModel(IAccountStorage accountStorage, ICurrentUser currentUser, IArtportalenService artportalenService)
        {
            _accountStorage = accountStorage;
            _artportalenService = artportalenService;
            CurrentUser = currentUser;
            Title = "Meny";

            IsConnectedArtportalen = _artportalenService.HasAccount;
            IsNotConnectedArtportalen = !_artportalenService.HasAccount;
        }

        public IMasterDetailNavigation MasterNavigation { get; set; }

        public ICurrentUser CurrentUser
        {
            get { return GetProperty<ICurrentUser>(); }
            set { SetProperty(value); }
        }

        public bool IsConnectedArtportalen
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsNotConnectedArtportalen
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

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

        public Command MyReportsCommand
        {
            get
            {
                return _myReportsCommand ?? (_myReportsCommand = new Command(() =>
                {
                    MasterNavigation.SetDetail<ReportsPage, ReportsViewModel>();
                }));
            }
        }

        public object NewReportCommand
        {
            get
            {
                return _newReportCommand ?? (_newReportCommand = new Command(async () =>
                {
                    await Navigation.PushModalAsync<CreateReportPage, CreateReportViewModel>(init: async vm =>
                    {
                    });
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

        public Command DisconnectFromArtportalenCommand
        {
            get
            {
                return _disconnectFromArtportalenCommand ?? (_disconnectFromArtportalenCommand = new Command(() =>
                {
                    _artportalenService.RemoveCredential();
                    IsConnectedArtportalen = _artportalenService.HasAccount;
                    IsNotConnectedArtportalen = !_artportalenService.HasAccount;
                }));
            }
        }

        public Command ConnectToArtportalenCommand
        {
            get
            {
                return _connectToArtportalenCommand ?? (_connectToArtportalenCommand = new Command(async () =>
                {
                    await Navigation.PushModalAsync<ArtportalenLogInPage, ArtportalenLogInViewModel>(done: async vm =>
                    {
                        await Navigation.PopModalAsync();

                        await _artportalenService.SaveCredential(vm.Username, vm.Password);
                        IsConnectedArtportalen = _artportalenService.HasAccount;
                        IsNotConnectedArtportalen = !_artportalenService.HasAccount;
                    });
                }));
            }
        }
    }
}
