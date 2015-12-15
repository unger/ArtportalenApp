using System;
using System.Collections.Generic;
using System.Linq;
using ArtportalenApp.Models;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSingleTaxonViewModel : ViewModelBase
    {
        private Command _doneCommand;

        public ChooseSingleTaxonViewModel()
        {
            TaxonFilter = "Art";
            Title = "Välj art";
        }

        public Command DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(async (t) =>
                {
                    var taxon = t as Taxon;
                    if (taxon != null)
                    {
                        SelectedTaxon = taxon;
                    }

                    await DoneAction();
                }));
            }
        }

        public Taxon SelectedTaxon
        {
            get { return GetProperty<Taxon>(); }
            set { SetProperty(value); }
        }

        public IList<Taxon> Taxons
        {
            get { return GetProperty<IList<Taxon>>(); }
            set
            {
                SetProperty(value);
                FilterTaxons();
            }
        }

        public IList<Taxon> FilteredTaxons
        {
            get { return GetProperty<IList<Taxon>>(); }
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
                FilteredTaxons = new List<Taxon>(
                    Taxons.Where(x =>
                        (string.IsNullOrEmpty(TaxonFilter) || x.Type == TaxonFilter)
                        &&
                        (string.IsNullOrEmpty(SearchText) || x.Name.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) != -1)
                        ));
            }
            else
            {
                FilteredTaxons = null;
            }
        }
    }
}
