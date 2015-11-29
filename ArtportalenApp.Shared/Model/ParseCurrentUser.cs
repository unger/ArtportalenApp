using System;
using System.Collections.Generic;
using System.Text;
using ArtportalenApp.Interfaces;
using Parse;

namespace ArtportalenApp.Model
{
    public class ApParseCurrentUser : ICurrentUser
    {
        public string Id
        {
            get
            {
                return IsAutenticated
                    ? ParseUser.CurrentUser.ObjectId
                    : null;
            }
        }

        public string Username
        {
            get
            {
                return IsAutenticated
                    ? ParseUser.CurrentUser.Username
                    : null;
            }
        }

        public string Email
        {
            get
            {
                return IsAutenticated
                    ? ParseUser.CurrentUser.Email
                    : null;
            }
        }

        public string Fullname
        {
            get
            {
                return IsAutenticated
                    ? ParseUser.CurrentUser["fullname"] as string
                    : null;
            }
        }

        public bool IsAutenticated
        {
            get { return ParseUser.CurrentUser != null && ParseUser.CurrentUser.IsAuthenticated; }
        }
    }
}
