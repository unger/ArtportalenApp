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

        public async Task<IList<Site>> GetNearBySites()
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
                if (_artportalenService.HasAccount)
                {
                    return await _artportalenService.GetNearbySites(position.Latitude, position.Longitude);
                }

                return await _siteStorage.GetNearbySites(position.Latitude, position.Longitude);
            }

            return new List<Site>();
        }
    }
}
