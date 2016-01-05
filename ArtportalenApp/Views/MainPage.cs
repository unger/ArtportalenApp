using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public class MainPage : MasterDetailPage, IPage<MainViewModel>, IMasterDetailNavigation
    {
        private IPageFactory _pageFactory;

        public MainPage()
        {
            _pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();

            Master = new NavigationPage(_pageFactory.CreatePage<MenuPage, MenuViewModel>(init: vm => { vm.MasterNavigation = this; })) { Title = "Menu" };
            Detail = new NavigationPage(_pageFactory.CreatePage<SightingsPage, SightingsViewModel>(init: vm => { vm.Title = "Obsar"; }));
        }


        public MainViewModel ViewModel { get; set; }

        public void SetDetail<TPage, TViewModel>(Action<TViewModel> init = null, Action<TViewModel> done = null, Action<TViewModel> cancel = null) 
            where TPage : Page, IPage<TViewModel>, new() 
            where TViewModel : class, IViewModel
        {
            var page = _pageFactory.CreatePage<TPage, TViewModel>(init, done, cancel);
            if (page is NavigationPage)
            {
                Detail = page;
            }
            else
            {
                Detail = new NavigationPage(page);
            }
            IsPresented = false;
        }
    }
}
