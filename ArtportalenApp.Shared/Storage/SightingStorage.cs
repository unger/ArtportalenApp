using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Parse;

namespace ArtportalenApp.Storage
{
    public class SightingStorage : ISightingStorage
    {
        public async Task<IList<Sighting>> Match(string daysFilter, string ruleId)
        {
            IList<ApParseSighting> sightings = null;
            try
            {
                var parameters = new Dictionary<string, object>();
                if (!String.IsNullOrEmpty(daysFilter))
                {
                    parameters.Add("date", daysFilter);
                }
                if (!String.IsNullOrEmpty(ruleId))
                {
                    parameters.Add("rule", ruleId);
                }
                sightings = await ParseCloud.CallFunctionAsync<IList<ApParseSighting>>("match", parameters);
            }
            catch (Exception e)
            {
                sightings = new List<ApParseSighting>();
            }
            return sightings.Select(ConvertToSighting).ToList();
        }

        private Sighting ConvertToSighting(ApParseSighting s)
        {
            return new Sighting
            {
                Id = s.ObjectId,
                SightingId = s.SightingId,
                TaxonSortOrder = s.TaxonSortOrder,
                TaxonId = s.TaxonId,
                TaxonName = s.TaxonName,
                Attribute = s.Attribute,
                Unsure = s.Unsure,
                NotRecovered = s.NotRecovered,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                SiteId = s.SiteId,
                SiteName = s.SiteName,
                Kommun = s.Kommun,
                Landskap = s.Landskap,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                SightingObservers = s.SightingObservers,
                Comment = s.Comment,
                CreatedAt = s.CreatedAt.HasValue ? s.CreatedAt.Value.ToLocalTime() : s.CreatedAt,
                UpdatedAt = s.UpdatedAt.HasValue ? s.UpdatedAt.Value.ToLocalTime() : s.UpdatedAt,
            };
        }
    }
}
