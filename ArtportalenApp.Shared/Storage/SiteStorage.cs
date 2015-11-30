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

        public async Task<IList<Site>> GetSites(string searchText = null)
        {
            var sitesQuery = new ParseQuery<ApParseSite>()
                .OrderByDescending(x => x.UseCount)
                .Limit(25);

            if (!string.IsNullOrEmpty(searchText) && searchText.Length >= 3)
            {
                sitesQuery = sitesQuery.Where(s => s.SiteName.Contains(searchText));
            }
            else
            {
                sitesQuery = sitesQuery.WhereGreaterThan("UseCount", 1000);
            }

            return (await sitesQuery.FindAsync())
                .Select(s => ConvertToSite(s, new ParseGeoPoint(0, 0)))
                .ToList();
        }

        private Site ConvertToSite(ApParseSite s, ParseGeoPoint p)
        {
            return new Site
            {
                SiteId = s.SiteId,
                SiteName = s.SiteName,
                Forsamling = s.Forsamling,
                Socken = s.Socken,
                Lan = s.Lan,
                SiteYCoord = s.SiteYCoord,
                SiteXCoord = s.SiteXCoord,
                Kommun = s.Kommun,
                Landskap = s.Landskap,
                UseCount = s.UseCount,
                Latitude = s.Location.Latitude,
                Longitude = s.Location.Longitude,
                DistanceKm = (p.Latitude == 0 && p.Longitude == 0) ? 0 : s.Location.DistanceTo(p).Kilometers
            };
        }
    }
}
