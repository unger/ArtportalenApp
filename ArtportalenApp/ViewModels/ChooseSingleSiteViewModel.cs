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
        private readonly ISiteService _siteService;
        private Command _doneCommand;

        public ChooseSingleSiteViewModel(ISiteService siteService)
        {
            _siteService = siteService;
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
            var lastSearch = SearchText;

            try
            {
                var sites = await _siteService.GetSites(lastSearch);
                Sites = new ObservableCollection<Site>(sites);
            }
            finally
            {
                IsBusy = false;
            }

            if (lastSearch != SearchText)
            {
                RefreshSites();
            }
        }
    }
}
