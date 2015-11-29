using System;
using System.Collections.Generic;
using System.Text;
using Parse;

namespace ArtportalenApp.Model
{
    [ParseClassName("_Session")]
    public class ApParseSession : ParseSession
    {
        [ParseFieldName("user")]
        public ParseUser User
        {
            get { return GetProperty<ParseUser>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("installationId")]
        public Guid InstallationId
        {
            get { return GetProperty<Guid>(); }
        }

        [ParseFieldName("expiresAt")]
        public DateTime? ExpiresAt
        {
            get { return GetProperty<DateTime?>(); }
        }
    }
}
