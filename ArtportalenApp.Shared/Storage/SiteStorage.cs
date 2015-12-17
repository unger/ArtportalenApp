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
            var sitesQuery = new ParseQuery<ApParseSite>()
                .WhereNear("location", pos)
                .Limit(25);

            return (await sitesQuery.FindAsync())
                .Select(s => ConvertToSite(s, pos))
                .OrderByDescending(s => s.DistanceKm)
                .ToList();
        }

        public async Task<IList<Site>> GetSites(string searchText, double latitude = 0, double longitude = 0)
        {
            var lowerSearchText = searchText.ToLower();
            var sitesQuery = new ParseQuery<ApParseSite>()
                .OrderByDescending(x => x.UseCount)
                .Limit(25);

            if (!string.IsNullOrEmpty(searchText))
            {
                sitesQuery = sitesQuery.Where(s => s.Search.Contains(lowerSearchText));
            }
            else
            {
                sitesQuery = sitesQuery.WhereGreaterThan("UseCount", 1000);
            }

            var allSites = (await sitesQuery.FindAsync())
                .Select(s => ConvertToSite(s, new ParseGeoPoint(latitude, longitude)));

            // Remove duplicates
            var siteIds = new HashSet<long>();
            var sites = new List<Site>();
            foreach (var site in allSites)
            {
                if (site.SiteId.HasValue)
                {
                    if (!siteIds.Contains(site.SiteId.Value))
                    {
                        siteIds.Add(site.SiteId.Value);
                        sites.Add(site);
                    }
                }
            }

            return sites
                .OrderByDescending(s => s.DistanceKm)
                .ThenBy(s => s.UseCount)
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
                Accuracy = s.Accuracy,
                ParentId = s.ParentId,
                IsPublic = s.IsPublic,
                DistanceKm = (p.Latitude == 0 && p.Longitude == 0) ? 0 : s.Location.DistanceTo(p).Kilometers
            };
        }
    }
}
