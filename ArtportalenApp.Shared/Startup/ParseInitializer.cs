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

            ParseClient.Initialize(ConfigurationManager.AppSettings.ParseApplicationId, ConfigurationManager.AppSettings.ParseDotNetKey);
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
