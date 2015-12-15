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
        private readonly IArtportalenService _artportalenService;

        public SiteService(IGeolocator geolocator, ISiteStorage siteStorage, IArtportalenService artportalenService)
        {
            _geolocator = geolocator;
            _siteStorage = siteStorage;
            _artportalenService = artportalenService;
        }

        public async Task<IList<Site>> GetSites(string searchText = null)
        {
            Position position;
            try
            {
                position = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            catch (Exception)
            {
                position = null;
            }

            if (position != null)
            {
                if (_artportalenService.HasAccount && string.IsNullOrEmpty(searchText))
                {
                    return await _artportalenService.GetNearbySites(position.Latitude, position.Longitude);
                }

                return await _siteStorage.GetNearbySites(position.Latitude, position.Longitude, searchText);
            }
            
            return await _siteStorage.GetSites(searchText);
        }
    }
}
