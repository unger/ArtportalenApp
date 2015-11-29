using Parse;

namespace ArtportalenApp.Model
{
    [ParseClassName("_Installation")]
    public class ApParseInstallation : ParseInstallation
    {
        [ParseFieldName("user")]
        public ParseUser User
        {
            get { return GetProperty<ParseUser>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("deviceInfo")]
        public string DeviceInfo
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
