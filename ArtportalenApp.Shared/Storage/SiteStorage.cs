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
        public async Task<IList<Site>> GetNearbySites(double latitude, double longitude, string searchText = null)
        {
            var pos = new ParseGeoPoint(latitude, longitude);
            var sitesQuery = new ParseQuery<ApParseSite>()
                .Limit(25);

            if (!string.IsNullOrEmpty(searchText) && searchText.Length >= 3)
            {
                sitesQuery = sitesQuery.Where(s => s.SiteName.Contains(searchText));
            }
            else
            {
                sitesQuery = sitesQuery.WhereNear("location", pos);
            }

            return (await sitesQuery.FindAsync())
                .Select(s => ConvertToSite(s, pos))
                .ToList();
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
