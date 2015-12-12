using System;
using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Services;
using Autofac;
using Xamarin.Forms;

namespace ArtportalenApp.Factories
{
    public class PageFactory : IPageFactory
    {
        private readonly IComponentContext _context;

        public PageFactory(IComponentContext context)
        {
            _context = context;
        }

        public TPage CreatePage<TPage, TViewModel>(Action<TViewModel> setAction = null, Action<TViewModel> poppedAction = null) 
            where TPage : Page, IViewModelAware<TViewModel>, new()
            where TViewModel : class, IViewModel
        {
            var page = new TPage();
            var vm = _context.Resolve<TViewModel>();

            vm.Navigation = new NavigationService(page.Navigation, this);

            if (setAction != null)
            {
                vm.SetState(setAction);
            }

            if (poppedAction != null)
            {
                vm.Navigation.Pop += (sender, args) =>
                {
                    poppedAction(vm);
                };
            }

            page.BindingContext = page.ViewModel = vm;

            page.Appearing += (sender, args) =>
            {
                vm.Appearing();
            };
            page.Disappearing += (sender, args) =>
            {
                vm.Disappearing();
            };

            return page;
        }
    }
}
