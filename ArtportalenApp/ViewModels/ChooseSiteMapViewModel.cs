using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Maps;
using ArtportalenApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSiteMapViewModel : ViewModelBase
    {
        private readonly ISiteService _siteService;
        private Command _doneCommand;
        private Command _refreshCommand;
        private Command _cancelCommand;
        private Plugin.Geolocator.Abstractions.Position firstLocation = null;

        public ChooseSiteMapViewModel(ISiteService siteService)
        {
            _siteService = siteService;
            Title = "Karta";
            VisibleRegion = MapSpan.FromCenterAndRadius(new Position(57.6, 11.9), new Distance(1500));
        }

        public override void Appearing()
        {
            base.Appearing();
            IsActive = true;
            Device.BeginInvokeOnMainThread(RefreshSites);
        }

        public override void Disappearing()
        {
            base.Disappearing();
            IsActive = false;
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

                    await DoneAction();
                }));
            }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshSites, () => !IsBusy)); }
        }

        public Command CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(async obj =>
                {
                    await CancelAction();
                }));
            }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsActive
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public Position MapCenter
        {
            get { return GetProperty<Position>(); }
            set
            {
                var oldCenter = MapCenter;
                var distance = MapHelper.CalculateDistance(oldCenter.Latitude, oldCenter.Longitude, value.Latitude, value.Longitude, 'K');

                if (distance > 1)
                {
                    SetProperty(value);
                    Device.BeginInvokeOnMainThread(RefreshSites);
                }
            }
        }

        public Distance Radius
        {
            get { return GetProperty<Distance>(); }
            set
            {
                if (value != Radius)
                {
                    SetProperty(value);
                    Device.BeginInvokeOnMainThread(RefreshSites);
                }
            }
        }

        public MapSpan VisibleRegion
        {
            get { return GetProperty<MapSpan>(); }
            set
            {
                SetProperty(value);
                if (value != null)
                {
                    MapCenter = value.Center;
                    Radius = value.Radius;
                }
            }
        }

        public ObservableCollection<ILocationViewModel> Pins
        {
            get { return GetProperty<ObservableCollection<ILocationViewModel>>(); }
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

        private async void RefreshSites()
        {
            if (IsBusy || !IsActive)
            {
                return;
            }

            IsBusy = true;

            if (firstLocation == null)
            {
                firstLocation = await _siteService.GetLocation();
                if (firstLocation != null)
                {
                    VisibleRegion = MapSpan.FromCenterAndRadius(new Position(firstLocation.Latitude, firstLocation.Longitude), VisibleRegion.Radius);
                    IsBusy = false;
                    return;
                }
            }

            try
            {
                var distanceRadians =
                    DegreesToRadians(Math.Max(VisibleRegion.LatitudeDegrees, VisibleRegion.LongitudeDegrees)/2);

                var sites = await _siteService.GetNearBySites(MapCenter.Latitude, MapCenter.Longitude, distanceRadians);
                Sites = new ObservableCollection<Site>(sites);

                var pins = new List<ILocationViewModel>();
                foreach (var site in sites)
                {
                    var localSite = site;
                    pins.Add(new LocationViewModel
                    {
                        Key = site.SiteId.ToString(),
                        Title = site.SiteName,
                        Latitude = site.Latitude,
                        Longitude = site.Longitude,
                        Description = string.Format("{0}", site.IsPublic.HasValue ? site.IsPublic.Value ? "Allmän" : "Privat" : string.Empty),
                        Command = new Command(() => DoneCommand.Execute(localSite))
                    });
                }

                //UpdatePins(pins);
                Pins = new ObservableCollection<ILocationViewModel>(pins);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
