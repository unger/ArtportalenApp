using System;
using System.Collections.Generic;
using System.Text;
using ArtportalenApp.Configuration;
using ArtportalenApp.Model;
using Parse;

namespace ArtportalenApp.Startup
{
    public class ParseInitializer
    {
        public static void Initialize()
        {
            ParseObject.RegisterSubclass<ApParseUser>();
            ParseObject.RegisterSubclass<ApParseInstallation>();
            ParseObject.RegisterSubclass<ApParseSession>();

            ParseObject.RegisterSubclass<ApParseSighting>();
            ParseObject.RegisterSubclass<ApParseSite>();
            ParseObject.RegisterSubclass<ApParseRule>();
            ParseObject.RegisterSubclass<ApParseTaxon>();

            var config = new ParseClient.Configuration
            {
                ApplicationId = ConfigurationManager.AppSettings.ParseApplicationId,
                WindowsKey = ConfigurationManager.AppSettings.ParseDotNetKey,
                Server = ConfigurationManager.AppSettings.ParseServerUrl,
            };

            ParseClient.Initialize(config);
            ParseAnalytics.TrackAppOpenedAsync();

#if __ANDROID__
            ParsePush.ParsePushNotificationReceived += ParsePush.DefaultParsePushNotificationReceivedHandler;
            if (ParseUser.CurrentUser != null)
            {
                ParseInstallation.CurrentInstallation["user"] = ParseUser.CurrentUser;
                ParseInstallation.CurrentInstallation.SaveAsync();
            }
#endif

        }
    }
}
