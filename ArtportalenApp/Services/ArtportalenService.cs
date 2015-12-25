using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Artportalen;
using Artportalen.Response.Web;
using ArtportalenApp.Configuration;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Newtonsoft.Json;
using SwedishCoordinates;
using SwedishCoordinates.Positions;

namespace ArtportalenApp.Services
{
    public class ArtportalenService : IArtportalenService
    {
        private readonly IArtportalenAccountStorage _artportalenAccountStorage;

        private Ap2Client _ap2Client;

        private Ap2WebClient _ap2WebClient;

        public ArtportalenService(IArtportalenAccountStorage artportalenAccountStorage)
        {
            if (artportalenAccountStorage.HasCrendental)
            {
                var account = artportalenAccountStorage.GetCredential();
                _ap2WebClient = new Ap2WebClient(new HttpClientHandler {CookieContainer = artportalenAccountStorage.Cookies, UseCookies = true });
                _ap2WebClient.SetCredentials(account.UserName, account.Password);
            }

            _ap2Client = new Ap2Client(ConfigurationManager.AppSettings.ArtportalenAccessKey);

            _artportalenAccountStorage = artportalenAccountStorage;
        }

        public bool HasAccount
        {
            get { return _artportalenAccountStorage.HasCrendental; }
        }

        public async Task SaveCredential(string username, string password)
        {
            var token = _ap2Client.Authorize(username, password);
            
            _artportalenAccountStorage.SaveCredential(username, password);
        }

        public NetworkCredential GetCredential()
        {
            return _artportalenAccountStorage.GetCredential();
        }

        public async Task<IList<Site>> GetNearbySites(double latitude, double longitude, double distanceRadians)
        {
            var distance = distanceRadians*100000;
            if (distance == 0) distance = 5000;
            var pos = new WGS84Position(latitude, longitude).ToWebMercator();
            var sw = new WebMercatorPosition(pos.Latitude - distance, pos.Longitude - distance);
            var ne = new WebMercatorPosition(pos.Latitude + distance, pos.Longitude + distance);

            var sites = await _ap2WebClient.GetSitesWithinBoundsAsync(sw, ne);
            _artportalenAccountStorage.SaveCookies();

            // Run this in background for non blocking of UI
            Task.Run(async () =>
            {
                await UpdateSites(sites).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        t.Exception.Handle(ex => true);
                    }

                    return t.Result;
                });
            });

            var webMercCalc = new WebMercatorCalculator();

            return sites.Select(s =>
                new Site
                {
                    SiteId = s.SiteId,
                    SiteName = s.SiteName,
                    SiteYCoord = s.SiteYCoord,
                    SiteXCoord = s.SiteXCoord,
                    Latitude = webMercCalc.YToLatitude(s.SiteYCoord),
                    Longitude = webMercCalc.XToLongitude(s.SiteXCoord),
                    Kommun = s.Kommun,
                    DistanceKm = GetDistanceKm(pos, new WebMercatorPosition(s.SiteYCoord, s.SiteXCoord)),
                }
            ).OrderBy(x => x.DistanceKm).ToList();
        }

        public void RemoveCredential()
        {
            _artportalenAccountStorage.RemoveCredential();
        }

        private double GetDistanceKm(WebMercatorPosition pos1, WebMercatorPosition pos2)
        {
            var dLat = pos1.Latitude - pos2.Latitude;
            var dLng = pos1.Longitude - pos2.Longitude;

            return Math.Sqrt(dLat * dLat + dLng * dLng) / 1000;
        }

        public Task<HttpResponseMessage> UpdateSites(IList<SiteResponse> sites)
        {
            var url = new Uri(ConfigurationManager.AppSettings.UpdateSitesUrl);
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(new { sites = sites });
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            return client.PostAsync(url, httpContent);
        }
    }
}
