using System.Net;

namespace ArtportalenApp.Interfaces
{
    public interface IArtportalenAccountStorage
    {
        bool HasCrendental { get; }

        NetworkCredential GetCredential();
        void SaveCredential(string username, string password);
        void RemoveCredential();

        CookieContainer Cookies { get; }
        void SaveCookies();
    }
}
