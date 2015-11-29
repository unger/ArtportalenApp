using System;
using System.Text;
using ArtportalenApp.Extensions;
using ArtportalenApp.ViewModels;

namespace ArtportalenApp.Models
{
    public class Rule : NotifyPropertyChangedBase
    {
        public string Id
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public int? Prefix
        {
            get { return GetProperty<int?>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Rule, int>(x => x.PrefixIndex);
            }
        }

        public string[] Taxons
        {
            get { return GetProperty<string[]>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Rule, string>(x => x.TaxonSummary);
                RaisePropertyChanged<Rule, string>(x => x.TaxonCount);
            }
        }

        public string[] Kommuner
        {
            get { return GetProperty<string[]>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Rule, string>(x => x.MunicipalitySummary);
                RaisePropertyChanged<Rule, string>(x => x.MunicipalityCount);
            }
        }

        public string[] Landskap
        {
            get { return GetProperty<string[]>(); }
            set
            {
                SetProperty(value);
                RaisePropertyChanged<Rule, string>(x => x.ProvinceSummary);
                RaisePropertyChanged<Rule, string>(x => x.ProvinceCount);
            }
        }

        public bool IsActive
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string IsActiveText
        {
            get { return IsActive ? "Aktiv" : null; }
        }

        public int PrefixIndex
        {
            get
            {
                if (Prefix.HasValue)
                {
                    return Prefix.Value;
                }
                return 9;
            }
            set
            {
                if (value >= 0 && value <= 9)
                {
                    Prefix = value;
                }
                else
                {
                    Prefix = 9;
                }
            }
        }

        public string TaxonSummary
        {
            get { return GetChoosenString(Taxons); }
        }

        public string TaxonCount
        {
            get { return GetCountString(Taxons); }
        }

        public string MunicipalitySummary
        {
            get { return GetChoosenString(Kommuner); }
        }

        public string MunicipalityCount
        {
            get { return GetCountString(Kommuner); }
        }

        public string ProvinceSummary
        {
            get { return GetChoosenString(Landskap); }
        }

        public string ProvinceCount
        {
            get { return GetCountString(Landskap); }
        }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return false;
                }


                if (Prefix == null && Taxons.IsNullOrEmpty() && Kommuner.IsNullOrEmpty() && Landskap.IsNullOrEmpty())
                {
                    return false;
                }

                return true;
            }
        }

        private string GetCountString(string[] input)
        {
            if (input != null)
            {
                if (input.Length > 0)
                {
                    return input.Length.ToString();
                }
            }

            return "0";
        }

        private string GetChoosenString(string[] input)
        {
            var max = 10;
            if (input != null)
            {
                if (input.Length > 0)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < Math.Min(input.Length, max); i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(input[i]);
                    }
                    if (input.Length > max)
                    {
                        sb.Append(", ...");
                    }

                    return sb.ToString();
                }    
            }

            return "Alla";
        }
    }
}