using System.Collections.Generic;
using Parse;

namespace ArtportalenApp
{
    [ParseClassName("Rule")]
    public class ApParseRule : ParseObject
    {
        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("prefix")]
        public int? Prefix
        {
            get { return GetProperty<int?>(); }
            set { SetProperty<int?>(value); }
        }

        [ParseFieldName("taxons")]
        public IList<string> Taxons
        {
            get { return GetProperty<IList<string>>(); }
            set { SetProperty<IList<string>>(value); }
        }

        [ParseFieldName("kommuner")]
        public IList<string> Kommuner
        {
            get { return GetProperty<IList<string>>(); }
            set { SetProperty<IList<string>>(value); }
        }

        [ParseFieldName("landskap")]
        public IList<string> Landskap
        {
            get { return GetProperty<IList<string>>(); }
            set { SetProperty<IList<string>>(value); }
        }

        [ParseFieldName("isActive")]
        public bool IsActive
        {
            get { return GetProperty<bool>(); }
            set { SetProperty<bool>(value); }
        }

        [ParseFieldName("user")]
        public ParseUser User
        {
            get { return GetProperty<ParseUser>(); }
            set { SetProperty(value); }
        }
    }
}
