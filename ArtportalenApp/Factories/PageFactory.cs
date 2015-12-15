using System;
using System.Threading.Tasks;
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

        public TPage CreatePage<TPage, TViewModel>(Action<TViewModel> init = null, Action<TViewModel> done = null, Action<TViewModel> cancel = null) 
            where TPage : Page, IPage<TViewModel>, new()
            where TViewModel : class, IViewModel
        {
            var page = new TPage();
            var vm = _context.Resolve<TViewModel>();

            vm.Navigation = new NavigationService(page.Navigation, this);

            if (init != null)
            {
                vm.SetState(init);
            }

            vm.SetDoneAction(() =>
            {
                if (done != null)
                {
                    done(vm);
                }

                return Task.FromResult(0);
            });

            vm.Navigation.Pop += (sender, args) =>
            {
                if (cancel != null)
                {
                    if (!vm.IsDone)
                    {
                        cancel(vm);
                    }
                }
            };

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
