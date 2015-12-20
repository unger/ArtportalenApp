using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IArtportalenService
    {
        bool HasAccount { get; }
        Task SaveCredential(string username, string password);
        void RemoveCredential();
        NetworkCredential GetCredential();
        Task<IList<Site>> GetNearbySites(double latitude, double longitude, double distanceRadians = 0);
    }
}
