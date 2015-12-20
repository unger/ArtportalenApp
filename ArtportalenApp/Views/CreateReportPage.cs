using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public class CreateReportPage : NavigationPage, IPage<CreateReportViewModel>
    {
        public CreateReportPage()
        {
            var pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();

            PushAsync(pageFactory.CreatePage<ChooseSitePage, ChooseSiteViewModel>(
                done: async vm =>
                {
                    await PushAsync(pageFactory.CreatePage<EditReportPage, EditReportViewModel>(
                        init: editVm =>
                        {
                            editVm.CurrentReport = new Report {Site = vm.SelectedSite};
                        },
                        cancel: async editVm =>
                        {
                            await Navigation.PopModalAsync();
                        },
                        done: async editVm =>
                        {
                            await ViewModel.DoneAction();
                            await Navigation.PopModalAsync();
                        }));
                },
                cancel: async editVm =>
                {
                    await Navigation.PopModalAsync();
                })).Wait();
        }

        public CreateReportViewModel ViewModel { get; set; }
    }
}
