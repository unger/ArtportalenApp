using System;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class EditReportViewModel : ViewModelBase
    {
        private readonly IReportStorage _reportStorage;
        private readonly ITaxonService _taxonService;
        private Command _saveCommand;
        private Command _changeTaxonCommand;
        private Command _changeSiteCommand;

        public EditReportViewModel(IReportStorage reportStorage, ITaxonService taxonService)
        {
            _reportStorage = reportStorage;
            _taxonService = taxonService;

            PropertyChanged += EditReportViewModel_PropertyChanged;
        }

        void EditReportViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                SaveCommand.ChangeCanExecute();
            }
        }

        public Report CurrentReport
        {
            get { return GetProperty<Report>(); }
            set { SetProperty(value); }
        }

        public string ErrorMessage
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsValid
        {
            get { return CurrentReport != null && CurrentReport.IsValid; }
        }

        public Command SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new Command(async () =>
                {
                    IsBusy = true;
                    SaveCommand.ChangeCanExecute();
                    try
                    {
                        await _reportStorage.SaveReport(CurrentReport);
                        await Navigation.PopAsync();
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                    }
                    finally
                    {
                        IsBusy = false;
                        SaveCommand.ChangeCanExecute();
                    }
                },
                () => !IsBusy && IsValid));
            }
        }

        public Command ChangeTaxonCommand
        {
            get
            {
                return _changeTaxonCommand ?? (_changeTaxonCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<ChooseSingleTaxonPage, ChooseSingleTaxonViewModel>(
                        setAction: async vm =>
                        {
                            vm.Taxons = await _taxonService.GetTaxons();
                            vm.SelectedTaxon = CurrentReport.Taxon;
                        },
                        poppedAction: vm =>
                        {
                            CurrentReport.Taxon = vm.SelectedTaxon;
                        });
                }));
            }
        }

        public Command ChangeSiteCommand
        {
            get
            {
                return _changeSiteCommand ?? (_changeSiteCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<ChooseSingleSitePage, ChooseSingleSiteViewModel>(
                        setAction: async vm =>
                        {
                            vm.SelectedSite = CurrentReport.Site;
                        },
                        poppedAction: vm =>
                        {
                            CurrentReport.Site = vm.SelectedSite;
                        });
                }));
            }
        }
    }
}