using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class SightingsPage : ContentPage, IViewModelAware<SightingsViewModel>
    {
        public SightingsPage()
        {
            InitializeComponent();

            SightingsListView.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }

        public SightingsViewModel ViewModel { get; set; }
    }
}
