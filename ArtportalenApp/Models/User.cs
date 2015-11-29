using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;

namespace ArtportalenApp.Models
{
    public class User : IUser
    {
        public string Id { get; set; }

        public string Username { get; private set; }

        public string Email { get; set; }

        public string Fullname { get; set; }
    }
}
