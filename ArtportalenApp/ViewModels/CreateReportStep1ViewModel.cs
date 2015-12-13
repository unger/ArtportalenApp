using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;

namespace ArtportalenApp.ViewModels
{
    public class CreateReportStep1ViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;

        public CreateReportStep1ViewModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override void Appearing()
        {
            base.Appearing();

            _reportService.GetLatestReportedSitesAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {

                }
                else
                {
                    var sites = t.Result;
                    for (int i = 0; i < 5; i++)
                    {
                        switch (i + 1)
                        {
                            case 1:
                                FirstSite = sites[i];
                                break;
                            case 2:
                                SecondSite = sites[i];
                                break;
                            case 3:
                                ThirdSite = sites[i];
                                break;
                            case 4:
                                FourthSite = sites[i];
                                break;
                            case 5:
                                FifthSite = sites[i];
                                break;
                        }
                    }
                }
            });
        }

        public Site FirstSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }

        public Site SecondSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }

        public Site ThirdSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }

        public Site FourthSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }

        public Site FifthSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }
    }
}
