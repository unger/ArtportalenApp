using System.Collections.Generic;
using System.Threading.Tasks;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IAccountStorage
    {
        Task SignUp(string fullname, string email, string password);
        Task<User> LogIn(string email, string password);
        Task RequestPasswordReset(string email);
        Task LogOut();
        Task<IList<Session>> GetSessions();
    }
}