using System;
using System.Collections.Generic;
using System.Text;
using ArtportalenApp.DependencyInjection;
using ArtportalenApp.Interfaces;
using ArtportalenApp.ViewModels;
using ArtportalenApp.Views;
using Autofac;
using Parse;
using Xamarin.Forms;

namespace ArtportalenApp
{
    public class ParseErrorHandler
    {
        public static void HandleParseError(ParseException e)
        {
            switch (e.Code)
            {
                case ParseException.ErrorCode.InvalidSessionToken:
                    HandleInvalidSessionToken();
                    break;
            }
        }

        private static void HandleInvalidSessionToken()
        {
            ParseUser.LogOut();

            Device.BeginInvokeOnMainThread(() =>
            {
                var pageFactory = AutofacContainer.Container.Resolve<IPageFactory>();
                Xamarin.Forms.Application.Current.MainPage = pageFactory.CreatePage<LogInPage, LogInViewModel>();
            });
        }
    }

}
