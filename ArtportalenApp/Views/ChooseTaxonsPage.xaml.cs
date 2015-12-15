using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class ChooseTaxonsPage : ContentPage, IPage<ChooseTaxonsViewModel>
    {
        public ChooseTaxonsPage()
        {
            InitializeComponent();
        }

        public ChooseTaxonsViewModel ViewModel { get; set; }
    }
}
