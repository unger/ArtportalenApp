using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public class SiteDetailPage : TabbedPage, IPage<SiteDetailViewModel>
    {
        private SiteDetailViewModel _viewModel;
        private SiteDetailInfoPage infoPage;

        public SiteDetailPage()
        {
            var pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();
            infoPage = pageFactory.CreatePage<SiteDetailInfoPage, SiteDetailInfoViewModel>();

            Children.Add(infoPage);

            /*
            Children.Add(pageFactory.CreatePage<ChooseSiteNearbyPage, ChooseSiteNearbyViewModel>(
                done: async vm =>
                {
                    ViewModel.SelectedSite = vm.SelectedSite;
                    await ViewModel.DoneAction();
                },
                cancel: async vm =>
                {
                    await ViewModel.CancelAction();
                }));
            Children.Add(pageFactory.CreatePage<ChooseSiteSearchPage, ChooseSiteSearchViewModel>(
                done: async vm =>
                {
                    ViewModel.SelectedSite = vm.SelectedSite;
                    await ViewModel.DoneAction();
                },
                cancel: async vm =>
                {
                    await ViewModel.CancelAction();
                }));
            Children.Add(pageFactory.CreatePage<ChooseSiteMapPage, ChooseSiteMapViewModel>(
                done: async vm =>
                {
                    ViewModel.SelectedSite = vm.SelectedSite;
                    await ViewModel.DoneAction();
                },
                cancel: async vm =>
                {
                    await ViewModel.CancelAction();
                }));*/
        }

        public SiteDetailViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                if (value != null)
                {
                    if (value.SelectedSite != null)
                    {
                        infoPage.ViewModel.SelectedSite = value.SelectedSite;
                    }
                }
            }
        }
    }
}
