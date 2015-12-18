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
    public class ChooseSiteMapViewModel : ViewModelBase
    {
        private readonly ISiteService _siteService;
        private Command _doneCommand;
        private Command _refreshCommand;

        public ChooseSiteMapViewModel(ISiteService siteService)
        {
            _siteService = siteService;
            Title = "Karta";
        }

        public override void Appearing()
        {
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

                    await DoneAction();
                }));
            }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshSites, () => !IsBusy)); }
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

        private async void RefreshSites()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var sites = await _siteService.GetNearBySites();
                Sites = new ObservableCollection<Site>(sites);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
