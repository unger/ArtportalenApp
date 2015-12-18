using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public class ChooseSitePage : TabbedPage, IPage<ChooseSiteViewModel>
    {
        public ChooseSitePage()
        {
            var pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();

            Children.Add(pageFactory.CreatePage<ChooseSiteLatestPage, ChooseSiteLatestViewModel>(done: async vm =>
            {
                ViewModel.SelectedSite = vm.SelectedSite;
                await ViewModel.DoneAction();
            }));
            Children.Add(pageFactory.CreatePage<ChooseSiteNearbyPage, ChooseSiteNearbyViewModel>(done: async vm =>
            {
                ViewModel.SelectedSite = vm.SelectedSite;
                await ViewModel.DoneAction();
            }));
            Children.Add(pageFactory.CreatePage<ChooseSiteSearchPage, ChooseSiteSearchViewModel>(done: async vm =>
            {
                ViewModel.SelectedSite = vm.SelectedSite;
                await ViewModel.DoneAction();
            }));
            Children.Add(pageFactory.CreatePage<ChooseSiteMapPage, ChooseSiteMapViewModel>(done: async vm =>
            {
                ViewModel.SelectedSite = vm.SelectedSite;
                await ViewModel.DoneAction();
            }));
        }

        public ChooseSiteViewModel ViewModel { get; set; }
    }
}
