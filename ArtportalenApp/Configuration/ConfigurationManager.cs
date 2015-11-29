namespace ArtportalenApp.Configuration
{
    public class ConfigurationManager
    {
        public static AppSettings AppSettings = ConfigSettingsFactory.Load<AppSettings>();
    }
}
