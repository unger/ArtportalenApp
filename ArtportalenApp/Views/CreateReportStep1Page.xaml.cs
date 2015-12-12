using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class CreateReportStep1Page : ContentPage, IViewModelAware<CreateReportStep1ViewModel>
    {
        public CreateReportStep1Page()
        {
            InitializeComponent();
        }

        public CreateReportStep1ViewModel ViewModel { get; set; }
    }
}
