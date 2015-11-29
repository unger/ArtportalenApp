using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArtportalenApp.ViewModels;

namespace ArtportalenApp.Models
{
    public class Report : NotifyPropertyChangedBase
    {
        public string Id { get; set; }

        public long? SightingId
        {
            get { return GetProperty<long?>(); }
            set { SetProperty(value); }
        }

        public Taxon Taxon
        {
            get { return GetProperty<Taxon>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Report, string>(x => x.TaxonName);
            }
        }

        public string Quantity
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public Site Site
        {
            get { return GetProperty<Site>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Report, string>(x => x.SiteName);
            }
        }

        public DateTime StartDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public DateTime EndDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public TimeSpan StartTime
        {
            get { return GetProperty<TimeSpan>(); }
            set { SetProperty(value); }
        }

        public TimeSpan EndTime
        {
            get { return GetProperty<TimeSpan>(); }
            set { SetProperty(value); }
        }

        public string Comment
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool Unsure
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool NotRecovered
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string TaxonName
        {
            get
            {
                return Taxon != null ? string.Format("{0}{1}", Taxon.Name, Unsure ? "?" : string.Empty) : null;
            }
        }

        public string SiteName
        {
            get
            {
                return Site != null ? string.Format("{0}, {1}, {2}", Site.SiteName, Site.Kommun, Site.Landskap) : null;
            }
        }

        public string SummaryRow
        {
            get { return string.Format("{0} {1} ({2})", Quantity, SiteName, Comment); }
        }

        public bool IsValid
        {
            get
            {
                var quantityValid = Quantity != null && Regex.IsMatch(Quantity, @"^\d*$");

                return quantityValid;
            }
        }
    }
}
