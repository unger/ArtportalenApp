using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Plugin.Geolocator.Abstractions;

namespace ArtportalenApp.Services
{
    public class SiteService : ISiteService
    {
        private readonly IGeolocator _geolocator;
        private readonly ISiteStorage _siteStorage;

        public SiteService(IGeolocator geolocator, ISiteStorage siteStorage)
        {
            _geolocator = geolocator;
            _siteStorage = siteStorage;
        }

        public async Task<IList<Site>> GetSites(string searchText = null)
        {
            Position position;
            try
            {
                position = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            catch (Exception e)
            {
                position = null;
            }

            if (position != null)
            {
                return await _siteStorage.GetNearbySites(position.Latitude, position.Longitude, searchText);
            }
            
            return await _siteStorage.GetSites(searchText);
        }
    }
}
