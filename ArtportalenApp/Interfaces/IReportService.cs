using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IReportService
    {
        Task<IList<Site>> GetLatestReportedSitesAsync();
    }
}