using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class ChooseSingleTaxonPage : ContentPage, IPage<ChooseSingleTaxonViewModel>
    {
        public ChooseSingleTaxonPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            SearchBarElement.Focus();

            base.OnAppearing();
        }

        public ChooseSingleTaxonViewModel ViewModel { get; set; }
    }
}
