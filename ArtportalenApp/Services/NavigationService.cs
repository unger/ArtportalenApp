using System;
using System.Threading.Tasks;
using ArtportalenApp.Factories;
using ArtportalenApp.Interfaces;
using Xamarin.Forms;

namespace ArtportalenApp.Services
{
    public delegate void PopEventHandler(object sender, EventArgs e);

    public class NavigationService : INavigationService
    {
        private readonly INavigation _navigation;
        private readonly IPageFactory _pageFactory;

        public NavigationService(INavigation navigation, IPageFactory pageFactory)
        {
            _navigation = navigation;
            _pageFactory = pageFactory;
        }

        public event PopEventHandler Pop;

        public Task PushAsync<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel
        {
            var page = _pageFactory.CreatePage<TPage, TViewModel>(setAction, poppedAction);

            return _navigation.PushAsync(page);
        }

        public Task PushModalAsync<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel
        {
            var page = _pageFactory.CreatePage<TPage, TViewModel>(setAction, poppedAction);

            if (page is NavigationPage)
            {
                return _navigation.PushModalAsync(page);
            }
            else
            {
                return _navigation.PushModalAsync(new NavigationPage(page));
            }
        }

        public async Task<Page> PopAsync()
        {
            var page = await _navigation.PopAsync();
            OnPop(EventArgs.Empty);
            return page;
        }

        public async Task<Page> PopModalAsync()
        {
            var page = await _navigation.PopModalAsync();
            OnPop(EventArgs.Empty);
            return page;
        }

        public async Task PopToRootAsync()
        {
            await _navigation.PopToRootAsync();
            OnPop(EventArgs.Empty);
        }

        public void ResetMainPage<TPage, TViewModel>(bool wrapWithNavigationPage, Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null)
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var page = _pageFactory.CreatePage<TPage, TViewModel>(setAction, poppedAction);
                if (wrapWithNavigationPage)
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
                else
                {
                    Application.Current.MainPage = page;
                }
            });
        }

        protected virtual void OnPop(EventArgs e)
        {
            if (Pop != null)
                Pop(this, e);
        }
    }
}
