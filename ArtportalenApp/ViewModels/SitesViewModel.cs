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
        private readonly ISiteStorage _siteStorage;
        private readonly IGeolocator _geolocator;
        private Command _refreshCommand;
        private Command _previewCommand;

        public SitesViewModel(ISiteStorage siteStorage, IGeolocator geolocator)
        {
            _siteStorage = siteStorage;
            _geolocator = geolocator;

            Title = "Närliggande lokaler";

            Device.BeginInvokeOnMainThread(RefreshSites);
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshSites, () => !IsBusy)); }
        }

        public Command PreviewCommand
        {
            get
            {
                return _previewCommand ?? (_previewCommand = new Command(async x =>
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


            var position = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);

            try
            {
                var sites = await _siteStorage.GetNearbySites(position.Latitude, position.Longitude);
                Sites = new ObservableCollection<Site>(sites);
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
                RefreshCommand.ChangeCanExecute();
            }
        }
    }
}
