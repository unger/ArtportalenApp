using System;

namespace ArtportalenApp.Models
{
    public class Sighting
    {
        public string Id { get; set; }
        public long? SightingId { get; set; }
        public int? TaxonSortOrder { get; set; }
        public int? TaxonPrefix { get; set; }
        public int TaxonId { get; set; }
        public string TaxonName { get; set; }
        public bool Unsure { get; set; }
        public bool NotRecovered { get; set; }
        public string Attribute { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public long? SiteId { get; set; }
        public string SiteName { get; set; }
        public string Forsamling { get; set; }
        public string Kommun { get; set; }
        public string Landskap { get; set; }
        public string Lan { get; set; }
        public string Socken { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SightingObservers { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool HasComment
        {
            get { return !string.IsNullOrWhiteSpace(Comment); }
        }

        public string SummaryRow
        {
            get
            {
                return string.Format("{0}{1}{2} {3}, {4}, {5} {6} {7} ({8})", 
                    Attribute,
                    Unsure ? " (ej säkert bestämd)" : string.Empty,
                    NotRecovered ? " (ej återfunnen)" : string.Empty,
                    SiteName, 
                    Kommun,
                    GetLandskapShortCode(Landskap),
                    DateString,
                    TimeString,
                    SightingObservers
                    );
            }
        }

        public string GetLandskapShortCode(string landskap)
        {
            switch (landskap)
            {
                case "Skåne": return "Sk";
                case "Blekinge": return "Bl";
                case "Småland": return "Sm";
                case "Öland": return "Öl";
                default: return landskap;
            }
        }

        public string DateString
        {
            get
            {
                if (StartDate.Year == EndDate.Year)
                {
                    if (StartDate.Month == EndDate.Month)
                    {
                        if (StartDate.Day == EndDate.Day)
                        {
                            return StartDate.ToString("d'/'M-yy");
                        }
                        return StartDate.ToString("d-") + EndDate.ToString("d'/'M-yy");
                    }
                    return StartDate.ToString("d'/'M-") + EndDate.ToString("d'/'M-yy");
                }
                return StartDate.ToString("d'/'M-yy") + EndDate.ToString("d'/'M-yy");
            }
        }

        public string TimeString
        {
            get
            {
                if (StartTime == EndTime)
                {
                    if (StartTime != null && StartTime.Length > 6)
                    {
                        return StartTime.Substring(0, 5);
                    }
                }
                if (StartTime != null && StartTime.Length > 6 && EndTime != null && EndTime.Length > 6)
                {
                    return string.Format("{0}-{1}", (StartTime ?? string.Empty).Substring(0, 5), (EndTime ?? string.Empty).Substring(0, 5));
                }

                return null;
            }
        }

        public string CreatedAtTimeString
        {
            get
            {
                if (CreatedAt.HasValue)
                {
                    return CreatedAt.Value.ToString("HH:mm");
                }
                return string.Empty;
            }
        }

        public string CreatedAtString
        {
            get
            {
                if (CreatedAt.HasValue)
                {
                    return CreatedAt.Value.ToString("d'/'M HH:mm");
                }
                return string.Empty;
            }
        }
    }
}
