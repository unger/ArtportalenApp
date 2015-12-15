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
    public class CreateReportStep1ViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private Command _nextCommand;
        private Command _cancelCommand;

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
                    Sites = t.Result;
                }
            });
        }

        public IList<Site> Sites
        {
            get { return GetProperty<List<Site>>(); }
            set { SetProperty(value); }
        }

        public Command NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new Command(async obj =>
                {
                    var site = obj as Site;

                    await Navigation.PushAsync<EditReportPage, EditReportViewModel>(
                        init: vm =>
                        {
                            vm.CurrentReport = new Report { Site = site };
                        },
                        done: vm =>
                        {
                            vm.Navigation.PopModalAsync();
                        });
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
