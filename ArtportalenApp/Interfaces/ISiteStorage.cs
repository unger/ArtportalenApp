using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface ISiteStorage
    {
        Task<IList<Site>> GetNearbySites(double latitude, double longitude, double distanceRadians = 0);

        Task<IList<Site>> GetSites(string searchText, double latitude = 0, double longitude = 0);
    }
}