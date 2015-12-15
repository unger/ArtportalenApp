using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
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
            PushAsync(pageFactory.CreatePage<CreateReportStep1Page, CreateReportStep1ViewModel>()).Wait();
        }

        public CreateReportViewModel ViewModel { get; set; }
    }
}
