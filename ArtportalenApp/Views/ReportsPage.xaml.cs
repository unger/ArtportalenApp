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
    public partial class ReportsPage : ContentPage, IPage<ReportsViewModel>
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        public ReportsViewModel ViewModel { get; set; }
    }
}
