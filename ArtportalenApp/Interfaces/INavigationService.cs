using System;
using System.Threading.Tasks;
using ArtportalenApp.Services;
using Xamarin.Forms;

namespace ArtportalenApp.Interfaces
{
    public interface INavigationService
    {
        Task PushAsync<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel;

        Task PushModalAsync<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel;

        Task<Page> PopAsync();
        Task<Page> PopModalAsync();
        Task PopToRootAsync();
        
        void ResetMainPage<TPage, TViewModel>(bool wrapWithNavigationPage, Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel;

        event PopEventHandler Pop;
    }
}