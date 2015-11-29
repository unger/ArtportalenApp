using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class ChooseValuesPage : ContentPage, IViewModelAware<ChooseValuesViewModel>
    {
        public ChooseValuesPage()
        {
            InitializeComponent();
        }

        public ChooseValuesViewModel ViewModel { get; set; }
    }
}
