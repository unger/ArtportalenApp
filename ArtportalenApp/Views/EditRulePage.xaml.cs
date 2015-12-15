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
    public partial class EditRulePage : ContentPage, IPage<EditRuleViewModel>
    {
        public EditRulePage()
        {
            InitializeComponent();
        }

        public EditRuleViewModel ViewModel { get; set; }
    }
}
