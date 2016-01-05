using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ArtportalenApp.Configuration;
using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Factories;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Model;
using ArtportalenApp.Services;
using ArtportalenApp.Storage;
using ArtportalenApp.ViewModels;
using Autofac;
using Parse;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin;

namespace ArtportalenApp
{
    public class Appstart
    {
        public static void Initialize()
        {
            AutofacContainer.Container = Buildcontainer();
        }

        private static IContainer Buildcontainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MenuViewModel>();

            builder.RegisterType<LogInViewModel>();
            builder.RegisterType<SignUpViewModel>();
            builder.RegisterType<RulesViewModel>();
            builder.RegisterType<SightingsViewModel>();
            builder.RegisterType<EditRuleViewModel>();
            builder.RegisterType<ChooseTaxonsViewModel>();
            builder.RegisterType<ChooseSingleTaxonViewModel>();
            builder.RegisterType<ChooseValuesViewModel>();

            builder.RegisterType<ChooseSiteViewModel>();
            builder.RegisterType<ChooseSiteSearchViewModel>();
            builder.RegisterType<ChooseSiteNearbyViewModel>();
            builder.RegisterType<ChooseSiteLatestViewModel>();
            builder.RegisterType<ChooseSiteMapViewModel>();
            
            builder.RegisterType<SessionsViewModel>();
            builder.RegisterType<SitesViewModel>();
            builder.RegisterType<ReportsViewModel>();
            builder.RegisterType<EditReportViewModel>();
            builder.RegisterType<ArtportalenLogInViewModel>();

            builder.RegisterType<SiteDetailViewModel>();
            builder.RegisterType<SiteDetailInfoViewModel>();

            builder.RegisterType<CreateReportViewModel>();
            builder.RegisterType<LoadingViewModel>();

            builder.Register(x => CrossDeviceInfo.Current).As<IDeviceInfo>();
            builder.Register(x => CrossGeolocator.Current).As<IGeolocator>();
            

            builder.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<TaxonService>().As<ITaxonService>();
            builder.RegisterType<ProvinceService>().As<IProvinceService>();
            builder.RegisterType<MuncipalityService>().As<IMunicipalityService>();
            builder.RegisterType<SiteService>().As<ISiteService>();
            builder.RegisterType<ArtportalenService>().As<IArtportalenService>();
            builder.RegisterType<ReportService>().As<IReportService>();
            
            
            
            builder.RegisterType<ApParseCurrentUser>().As<ICurrentUser>().SingleInstance();            

            builder.RegisterType<SightingStorage>().As<ISightingStorage>();
            builder.RegisterType<RuleStorage>().As<IRuleStorage>();
            builder.RegisterType<TaxonStorage>().As<ITaxonStorage>();
            builder.RegisterType<AccountStorage>().As<IAccountStorage>();
            builder.RegisterType<SiteStorage>().As<ISiteStorage>();
            builder.RegisterType<ReportStorage>().As<IReportStorage>();
            builder.RegisterType<ArtportalenAccountStorage>().As<IArtportalenAccountStorage>();

            
            builder.RegisterType<Services.NotificationCenter>().As<INotificationCenter>();

            return builder.Build();            
        }
    }
}
