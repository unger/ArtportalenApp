using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IReportStorage
    {
        Task<IList<Report>> GetReports();
        Task DeleteReport(string id);
        Task SaveReport(Report currentReport);
    }
}