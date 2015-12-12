namespace ArtportalenApp.Models
{
    public class Site
    {
        public long? SiteId { get; set; }
        public string SiteName { get; set; }
        public string Forsamling { get; set; }
        public string Kommun { get; set; }
        public string Landskap { get; set; }
        public string Lan { get; set; }
        public string Socken { get; set; }
        public int UseCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int SiteYCoord { get; set; }
        public int SiteXCoord { get; set; }
        public double DistanceKm { get; set; }

        public string TitleText
        {
            get { return string.Format("{0}, {1}", SiteName, Kommun); }
        }

        public string DetailText
        {
            get { return string.Format("{0}, {1}, {2}", Forsamling, Kommun, Landskap); }
        }

        public string DistanceText
        {
            get
            {
                if (DistanceKm < 1)
                {
                    return string.Format("{0:0}m", DistanceKm*1000);
                }
                else
                {
                    return string.Format("{0:0.0}km", DistanceKm);
                }
            }
        }
    }
}
