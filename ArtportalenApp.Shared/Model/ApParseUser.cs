using System;
using System.Collections.Generic;
using System.Text;
using Parse;

namespace ArtportalenApp.Model
{
    [ParseClassName("_User")]
    public class ApParseUser : ParseUser
    {
        [ParseFieldName("fullname")]
        public string Fullname
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
