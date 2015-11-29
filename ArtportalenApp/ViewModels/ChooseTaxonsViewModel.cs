using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Services;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseTaxonsViewModel : ViewModelBase
    {
        private Command _doneCommand;

        private string[] selectedTaxons = new string[] {};

        public ChooseTaxonsViewModel(ITaxonService taxonService)
        {
            TaxonFilter = "";
            Title = "Välj arter";

            Device.BeginInvokeOnMainThread(async () =>
            {
                var taxons = await taxonService.GetSpecies();
                Taxons = new ObservableCollection<TaxonItemModel>(taxons.Select(x => new TaxonItemModel { Name = x.Name, Prefix = x.Prefix }).ToList());

                if (Taxons != null)
                {
                    foreach (var taxon in Taxons)
                    {
                        taxon.Selected = selectedTaxons.Any(t => t == taxon.Name);
                    }

                    foreach (var taxon in selectedTaxons)
                    {
                        if (!Taxons.Any(t => t.Name == taxon))
                        {
                            Taxons.Add(new TaxonItemModel { Name = taxon, Selected = true });
                        }
                    }
                    RaisePropertyChanged<ChooseTaxonsViewModel, ObservableCollection<TaxonItemModel>>(x => x.Taxons);
                }
            });
        }

        public Command DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(async () =>
                {
                    await Navigation.PopAsync();
                }));
            }
        }

        public void SetSelectedTaxons(string[] taxons)
        {
            selectedTaxons = taxons ?? new string[] {};
        }

        public ObservableCollection<TaxonItemModel> Taxons
        {
            get { return GetProperty<ObservableCollection<TaxonItemModel>>(); }
            set
            {
                SetProperty(value);
                FilterTaxons();
            }
        }

        public ObservableCollection<TaxonItemModel> FilteredTaxons
        {
            get { return GetProperty<ObservableCollection<TaxonItemModel>>(); }
            set { SetProperty(value); }
        }

        public string SearchText
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                FilterTaxons();
            }
        }

        public string TaxonFilter
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                FilterTaxons();
            }
        }

        private void FilterTaxons()
        {
            if (Taxons != null)
            {
                bool? selectedFilter = null;
                var prefixFilter = "";
                if (TaxonFilter == "valda")
                {
                    selectedFilter = true;
                }
                else if (TaxonFilter == "ejvalda")
                {
                    selectedFilter = false;
                }
                else
                {
                    prefixFilter = TaxonFilter;
                }

                FilteredTaxons = new ObservableCollection<TaxonItemModel>(
                    Taxons.Where(x =>
                        (!selectedFilter.HasValue || x.Selected == selectedFilter.Value)
                        &&
                        (string.IsNullOrEmpty(prefixFilter) || x.PrefixFilter == prefixFilter)
                        &&
                        (string.IsNullOrEmpty(SearchText) || x.Name.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) != -1)
                        ));
            }
            else
            {
                FilteredTaxons = null;
            }
        }

        public class TaxonItemModel
        {
            public string Name { get; set; }
            public bool Selected { get; set; }
            public int? Prefix { get; set; }

            public string PrefixFilter
            {
                get
                {
                    if (Prefix.HasValue)
                    {
                        switch (Prefix.Value)
                        {
                            case 0:
                            case 1:
                                return "0";
                            case 2:
                            case 3:
                                return "1";
                            case 4:
                            case 5:
                                return "2";
                            case 6:
                            case 7:
                                return "3";
                            case 8:
                            case 9:
                                return "4";
                        }
                    }

                    return "";
                }
            }
        } 
    }
}
