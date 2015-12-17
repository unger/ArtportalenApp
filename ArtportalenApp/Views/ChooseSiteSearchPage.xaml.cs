using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public partial class ChooseSiteSearchPage : ContentPage<ChooseSiteSearchViewModel>
    {
        public ChooseSiteSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            SearchBarElement.Focus();

            base.OnAppearing();
        }
    }
}
