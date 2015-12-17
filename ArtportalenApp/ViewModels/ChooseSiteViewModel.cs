using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.ViewModels
{
    public class ChooseSiteViewModel : ViewModelBase
    {
        public Site SelectedSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }
    }
}
