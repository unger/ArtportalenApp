using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Parse;

namespace ArtportalenApp.Storage
{
    public class ReportStorage : IReportStorage
    {
        public async Task<IList<Report>> GetReports()
        {
            var reports = await new ParseQuery<ApParseSighting>()
                .Where(x => x.User == ParseUser.CurrentUser)
                .Where(x => x.SightingId == null)
                .FindAsync();

            return reports.Select(ConvertToReport).ToList();
        }

        public Task DeleteReport(string id)
        {
            var parseReport = ParseObject.CreateWithoutData<ApParseSighting>(id);
            return parseReport.DeleteAsync();
        }

        public async Task SaveReport(Report report)
        {
            int quantity;
            var startDate = report.StartDate + report.StartTime;
            var endDate = report.EndDate + report.EndTime;

            var startTime = startDate.ToString("HH:mm:ss");
            if (startTime == "00:00:00")
            {
                startTime = null;
            }
            var endTime = endDate.ToString("HH:mm:ss");
            if (endTime == "00:00:00")
            {
                endTime = null;
            }

            ApParseSighting parseSighting;
            if (string.IsNullOrEmpty(report.Id))
            {
                parseSighting = new ApParseSighting();
            }
            else
            {
                parseSighting = ParseObject.CreateWithoutData<ApParseSighting>(report.Id);
            }

            parseSighting.SightingId = report.SightingId;
            parseSighting.TaxonSortOrder = report.Taxon.SortOrder;
            parseSighting.TaxonPrefix = report.Taxon.Prefix;
            parseSighting.TaxonId = report.Taxon.TaxonId;
            parseSighting.TaxonName = report.Taxon.Name;
            parseSighting.Unsure = report.Unsure;
            parseSighting.NotRecovered = report.NotRecovered;
            //Attribute = this.attributeCalculator.GetAttribute(report.Quantity, report.StageId, report.GenderId, report.ActivityId);
            parseSighting.Quantity = int.TryParse(report.Quantity, out quantity) ? (int?)quantity : null;
            parseSighting.StartDate = report.StartDate + report.StartTime;
            parseSighting.EndDate = report.EndDate + report.EndTime;
            parseSighting.StartTime = startTime;
            parseSighting.EndTime = endTime;
            parseSighting.SiteId = report.Site.SiteId;
            parseSighting.SiteName = report.Site.SiteName;
            parseSighting.Forsamling = report.Site.Forsamling;
            parseSighting.Kommun = report.Site.Kommun;
            parseSighting.Lan = report.Site.Lan;
            parseSighting.Landskap = report.Site.Landskap;
            parseSighting.Socken = report.Site.Socken;
            parseSighting.SiteXCoord = report.Site.SiteXCoord;
            parseSighting.SiteYCoord = report.Site.SiteYCoord;
            parseSighting.Location = new ParseGeoPoint(report.Site.Latitude, report.Site.Longitude);
            parseSighting.SightingObservers = ParseUser.CurrentUser["fullname"] as string;
            parseSighting.Comment = report.Comment;
            parseSighting.User = ParseUser.CurrentUser;

            await parseSighting.SaveAsync();
        }

        private Report ConvertToReport(ApParseSighting s)
        {
            return new Report
            {
                Id = s.ObjectId,
                SightingId = s.SightingId,
                Taxon = new Taxon
                {
                    TaxonId = s.TaxonId,
                    Name = s.TaxonName,
                    SortOrder = s.TaxonSortOrder,
                    Prefix = s.TaxonPrefix,
                },
                Quantity = s.Quantity.HasValue ? s.Quantity.ToString() : null,
                Unsure = s.Unsure,
                NotRecovered = s.NotRecovered,
                Site = new Site
                {
                    SiteId = s.SiteId,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    SiteName = s.SiteName,
                    Kommun = s.Kommun,
                    Landskap = s.Landskap,
                    Forsamling = s.Forsamling,
                    Lan = s.Lan,
                    Socken = s.Socken,
                    SiteYCoord = s.SiteYCoord,
                    SiteXCoord = s.SiteXCoord,
                },
                StartDate = s.StartDate.Date,
                EndDate = s.EndDate.Date,
                StartTime = s.StartDate.TimeOfDay,
                EndTime = s.EndDate.TimeOfDay,
                Comment = s.Comment,
            };
        }
    }
}
