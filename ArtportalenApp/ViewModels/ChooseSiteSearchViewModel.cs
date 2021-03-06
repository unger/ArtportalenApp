﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSiteSearchViewModel : ViewModelBase
    {
        private readonly ISiteService _siteService;
        private Command _doneCommand;
        private Command _refreshCommand;
        private Command _cancelCommand;

        public ChooseSiteSearchViewModel(ISiteService siteService)
        {
            _siteService = siteService;
            Title = "Sök";
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
                if (!string.IsNullOrEmpty(lastSearch))
                {
                    var sites = await _siteService.GetSites(lastSearch);
                    Sites = new ObservableCollection<Site>(sites);
                }
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
