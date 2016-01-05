using System;
using Xamarin.Forms;

namespace ArtportalenApp.Interfaces
{
    public interface IMasterDetailNavigation
    {
        void SetDetail<TPage, TViewModel>(Action<TViewModel> init = null, Action<TViewModel> done = null, Action<TViewModel> cancel = null)
            where TPage : Page, IPage<TViewModel>, new()
            where TViewModel : class, IViewModel;
    }
}
