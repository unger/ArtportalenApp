using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class EditReportPage : ContentPage, IViewModelAware<EditReportViewModel>
    {
        public EditReportPage()
        {
            InitializeComponent();
        }

        public EditReportViewModel ViewModel { get; set; }
    }
}
