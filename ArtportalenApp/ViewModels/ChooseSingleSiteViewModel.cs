using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSingleSiteViewModel : ViewModelBase
    {
        private readonly IGeolocator _geolocator;
        private readonly ISiteStorage _siteStorage;
        private Command _doneCommand;

        public ChooseSingleSiteViewModel(IGeolocator geolocator, ISiteStorage siteStorage)
        {
            _geolocator = geolocator;
            _siteStorage = siteStorage;
            Title = "Välj lokal";

            Device.BeginInvokeOnMainThread(RefreshSites);
        }

        public Command DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(async (t) =>
                {
                    var site = t as Site;
                    if (site != null)
                    {
                        SelectedSite = site;
                    }

                    await Navigation.PopAsync();
                }));
            }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public Site SelectedSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }

        public IList<Site> Sites
        {
            get { return GetProperty<IList<Site>>(); }
            set
            {
                SetProperty(value);
            }
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

        private async void RefreshSites()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;


            var position = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);

            try
            {
                var sites = await _siteStorage.GetNearbySites(position.Latitude, position.Longitude, SearchText);
                Sites = new ObservableCollection<Site>(sites);
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
