using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;

namespace ArtportalenApp.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportStorage _reportStorage;

        public ReportService(IReportStorage reportStorage)
        {
            _reportStorage = reportStorage;
        }

        public async Task<IList<Site>> GetLatestReportedSitesAsync()
        {
            var sites = new List<Site>();
            var reports = await _reportStorage.GetReports();

            foreach (var report in reports)
            {
                var site = report.Site;
                if (!sites.Exists(
                        s =>
                            s.SiteName == site.SiteName && 
                            s.SiteXCoord == site.SiteXCoord &&
                            s.SiteYCoord == site.SiteYCoord))
                {
                    sites.Add(site);
                }

                if (sites.Count == 5) break;
            }

            return sites;
        }
    }
}
