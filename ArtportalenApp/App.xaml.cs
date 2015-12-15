using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using ArtportalenApp.Views;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp
{
    public partial class App : Application
    {
        public App()
        {
            var pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();
            var currentUser = AutofacContainer.Container.Resolve<ICurrentUser>();

            MainPage = new NavigationPage(pageFactory.CreatePage<LoadingPage, LoadingViewModel>());



            InitializeComponent();

            // The root page of your application
            if (!currentUser.IsAutenticated)
            {
                MainPage = new NavigationPage(pageFactory.CreatePage<LogInPage, LogInViewModel>());
            }
            else
            {
                MainPage = pageFactory.CreatePage<MainPage, MainViewModel>();
            }
        }
    }
}
