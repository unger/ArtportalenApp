using System;
using System.Threading.Tasks;
using ArtportalenApp.Services;
using Xamarin.Forms;

namespace ArtportalenApp.Interfaces
{
    public interface INavigationService
    {
        Task PushAsync<TPage, TViewModel>(Action<TViewModel> init = null, Action<TViewModel> done = null)
            where TPage : Page, IPage<TViewModel>, new()
            where TViewModel : class, IViewModel;

        Task PushModalAsync<TPage, TViewModel>(Action<TViewModel> init = null, Action<TViewModel> done = null)
            where TPage : Page, IPage<TViewModel>, new()
            where TViewModel : class, IViewModel;

        Task<Page> PopAsync();
        Task<Page> PopModalAsync();
        Task PopToRootAsync();
        
        void ResetMainPage<TPage, TViewModel>(bool wrapWithNavigationPage, Action<TViewModel> init = null)
            where TPage : Page, IPage<TViewModel>, new()
            where TViewModel : class, IViewModel;

        event PopEventHandler Pop;
    }
}