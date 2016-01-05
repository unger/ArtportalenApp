using ArtportalenApp.Models;

namespace ArtportalenApp.ViewModels
{
    public class SiteDetailInfoViewModel : ViewModelBase
    {
        public Site SelectedSite
        {
            get { return GetProperty<Site>(); }
            set { SetProperty(value); }
        }
    }
}
