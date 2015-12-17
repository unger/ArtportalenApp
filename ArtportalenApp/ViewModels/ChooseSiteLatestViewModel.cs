using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSiteLatestViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private Command _cancelCommand;
        private Command _doneCommand;
        private Command _refreshCommand;

        public ChooseSiteLatestViewModel(IReportService reportService)
        {
            _reportService = reportService;
            Title = "Senaste";
        }

        public override void Appearing()
        {
            Device.BeginInvokeOnMainThread(RefreshSites);
        }

        public IList<Site> Sites
        {
            get { return GetProperty<List<Site>>(); }
            set { SetProperty(value); }
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


        private async void RefreshSites()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var sites = await _reportService.GetLatestReportedSitesAsync();
                Sites = sites;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshSites, () => !IsBusy)); }
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

        public Command CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(async obj =>
                {
                    await Navigation.PopModalAsync();
                }));
            }
        }
    }
}
