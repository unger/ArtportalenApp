using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Artportalen.Response.Web;
using ArtportalenApp.Configuration;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;

namespace ArtportalenApp.Services
{
    public class SiteService : ISiteService
    {
        private readonly IGeolocator _geolocator;
        private readonly ISiteStorage _siteStorage;
        private readonly IArtportalenService _artportalenService;
        private Position _lastPosition;

        public SiteService(IGeolocator geolocator, ISiteStorage siteStorage, IArtportalenService artportalenService)
        {
            _geolocator = geolocator;
            _siteStorage = siteStorage;
            _artportalenService = artportalenService;
        }

        public async Task<IList<Site>> GetSites(string searchText = null)
        {
            return await _siteStorage.GetSites(searchText);
        }

        public async Task<Position> GetLocation(bool fetch = false)
        {
            if (_lastPosition == null || fetch)
            {
                return await FetchLocation();
            }

            return _lastPosition;
        }

        private async Task<Position> FetchLocation()
        {
            try
            {
                _lastPosition = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            catch (Exception)
            {
            }

            return _lastPosition;
        }

        public async Task<IList<Site>> GetNearBySites()
        {
            var position = await FetchLocation();

            if (position != null)
            {
                return await GetNearBySites(position.Latitude, position.Longitude);
            }

            return new List<Site>();
        }

        public async Task<IList<Site>> GetNearBySites(double latitude, double longitude, double distanceRadians = 0)
        {
            if (_artportalenService.HasAccount)
            {
                return await _artportalenService.GetNearbySites(latitude, longitude, distanceRadians);
            }

            return await _siteStorage.GetNearbySites(latitude, longitude, distanceRadians);
        }
    }
}
