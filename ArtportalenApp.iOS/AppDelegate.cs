using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ArtportalenApp.Configuration;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Startup;
using Foundation;
using Parse;
using TestFairyLib;
using UIKit;
using Xamarin;

namespace ArtportalenApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Xamarin insights
            Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
            {
                if (isStartupCrash)
                {
                    Insights.PurgePendingCrashReports().Wait();
                }
            };
            Insights.Initialize(ConfigurationManager.AppSettings.XamarinInsightsApiKey);

            TestFairy.Begin(ConfigurationManager.AppSettings.TestFairyAppToken);

            // Initialize 
            Appstart.Initialize();
            ParseInitializer.Initialize();
            ParsePushInitialize();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var installation = ParseInstallation.CurrentInstallation;
            installation.SetDeviceTokenFromData(deviceToken);
            installation.SaveAsync();
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (application.ApplicationState == UIApplicationState.Active)
            {
                // Application in Foregroud, popup message
                if (userInfo.ContainsKey(NSObject.FromObject("aps")))
                {
                    var aps = userInfo["aps"] as NSMutableDictionary;
                    if (aps != null)
                    {
                        if (aps.ContainsKey(NSObject.FromObject("alert")))
                        {
                            var alert = (aps["alert"] ?? NSObject.FromObject("")).ToString();
                            new UIAlertView("Artportalen", alert, null, "Ok", null).Show();
                        }
                    }
                }
            }

            ParsePush.HandlePush(userInfo);
            completionHandler(UIBackgroundFetchResult.NoData);
        }
        
        private void ParsePushInitialize()
        {
            // Register for Push Notitications
            UIUserNotificationType notificationTypes = (UIUserNotificationType.Alert |
                                                        UIUserNotificationType.Badge |
                                                        UIUserNotificationType.Sound);
            var settings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes,
                                                                          new NSSet(new string[] { }));
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            ParsePush.ParsePushNotificationReceived += (object sender, ParsePushNotificationEventArgs args) =>
            {
                if (args.Payload.ContainsKey("alert"))
                {
                    //args.Payload["alert"].ToString();
                }
            };
        }
    }
}
