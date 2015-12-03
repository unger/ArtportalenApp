using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Constants;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class SitesViewModel : ViewModelBase
    {
        private readonly ISiteService _siteService;
        private Command _refreshCommand;
        private Command _previewCommand;

        public SitesViewModel(ISiteService siteService)
        {
            _siteService = siteService;
            Title = "Närliggande lokaler";

            Device.BeginInvokeOnMainThread(RefreshSites);
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string SearchText
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                RefreshSites();
            }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshSites, () => !IsBusy)); }
        }

        public Command PreviewCommand
        {
            get
            {
                return _previewCommand ?? (_previewCommand = new Command(x =>
                {
                    var site = x as Site;
                    if (site != null)
                    {
                        /*
                        await Navigation.PushAsync<SightingsPage, SightingsViewModel>(
                            setAction: vm =>
                            {
                                vm.Title = site.Name;
                                vm.RuleId = site.Id;
                            });*/
                    }
                }));
            }
        }

        public ObservableCollection<Site> Sites
        {
            get { return GetProperty<ObservableCollection<Site>>(); }
            set
            {
                SetProperty(value);
            }
        }

        private async void RefreshSites()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            RefreshCommand.ChangeCanExecute();
            var lastSearch = SearchText;

            try
            {
                var sites = await _siteService.GetSites(lastSearch);
                Sites = new ObservableCollection<Site>(sites);
            }
            finally
            {
                IsBusy = false;
                RefreshCommand.ChangeCanExecute();
            }

            if (lastSearch != SearchText)
            {
                RefreshSites();
            }
        }
    }
}
