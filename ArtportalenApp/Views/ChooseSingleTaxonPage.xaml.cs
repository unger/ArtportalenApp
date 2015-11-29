using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class ChooseSingleTaxonPage : ContentPage, IViewModelAware<ChooseSingleTaxonViewModel>
    {
        public ChooseSingleTaxonPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            // This disables some databinding at least on Android, investigate workaround
            //searchBar.Focus();

            base.OnAppearing();
        }

        public ChooseSingleTaxonViewModel ViewModel { get; set; }
    }
}
