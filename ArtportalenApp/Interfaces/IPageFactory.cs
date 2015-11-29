using System;
using Xamarin.Forms;

namespace ArtportalenApp.Interfaces
{
    public interface IPageFactory
    {
        TPage CreatePage<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel;
    }
}