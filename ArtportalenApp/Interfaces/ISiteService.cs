using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;
using Plugin.Geolocator.Abstractions;

namespace ArtportalenApp.Interfaces
{
    public interface ISiteService
    {
        Task<IList<Site>> GetSites(string searchText = null);
        Task<IList<Site>> GetNearBySites();
        Task<IList<Site>> GetNearBySites(double latitude, double longitude, double distance = 0);
        Task<Position> GetLocation(bool fetch = false);
    }
}