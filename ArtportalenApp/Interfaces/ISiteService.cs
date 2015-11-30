using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface ISiteService
    {
        Task<IList<Site>> GetSites(string searchText = null);
    }
}