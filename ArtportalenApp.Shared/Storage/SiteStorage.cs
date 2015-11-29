using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Parse;

namespace ArtportalenApp.Storage
{
    public class SiteStorage : ISiteStorage
    {
        public async Task<IList<Site>> GetNearbySites(double latitude, double longitude)
        {
            var pos = new ParseGeoPoint(latitude, longitude);
            var sites = await new ParseQuery<ApParseSite>()
                .WhereNear("location", pos)
                .Limit(25)
                .FindAsync();


            return sites.Select(s => ConvertToSite(s, pos)).ToList();
        }

        private Site ConvertToSite(ApParseSite s, ParseGeoPoint p)
        {
            return new Site
            {
                SiteId = s.SiteId,
                SiteName = s.SiteName,
                Forsamling = s.Forsamling,
                Kommun = s.Kommun,
                Landskap = s.Landskap,
                UseCount = s.UseCount,
                DistanceKm = s.Location.DistanceTo(p).Kilometers

            };
        }
    }
}
