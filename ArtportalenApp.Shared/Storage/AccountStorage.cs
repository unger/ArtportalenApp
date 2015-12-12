using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Constants;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Model;
using ArtportalenApp.Models;
using Parse;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;

namespace ArtportalenApp.Storage
{
    public class AccountStorage : IAccountStorage
    {
        private readonly INotificationCenter _notificationCenter;
        private readonly ICurrentUser _currentUser;
        private readonly IDeviceInfo _deviceInfo;

        public AccountStorage(INotificationCenter notificationCenter, ICurrentUser currentUser, IDeviceInfo deviceInfo)
        {
            _notificationCenter = notificationCenter;
            _currentUser = currentUser;
            _deviceInfo = deviceInfo;
        }

        public Task SignUp(string fullname, string email, string password)
        {
            var user = new ApParseUser
            {
                Email = email,
                Username = email,
                Password = password,
                Fullname = fullname,
            };

            return user.SignUpAsync()
                .ContinueWith(async t =>
                {
                    if (t.IsCanceled)
                    {
                        throw new TaskCanceledException();
                    }
                    if (t.IsFaulted)
                    {
                        throw t.Exception;
                    }
                    _notificationCenter.Send(NotificationKeys.CurrentUserChanged, _currentUser);
                    await AssociateInstallationWithUser();
                });
        }

        public async Task<User> LogIn(string email, string password)
        {
            var parseUser = await ParseUser.LogInAsync(email, password)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var handled = ParseErrorHandler.HandleParseError(t.Exception.InnerException as ParseException);
                        if (!handled)
                        {
                            throw t.Exception.InnerException;
                        }
                    }
                    _notificationCenter.Send(NotificationKeys.CurrentUserChanged, _currentUser);

                    return t.Result;
                });

            await AssociateInstallationWithUser();

            return new User
            {
                Id = parseUser.ObjectId,
                Fullname = parseUser["fullname"] as string,
                Email = parseUser.Username,
            };
        }

        public Task RequestPasswordReset(string email)
        {
            return ParseUser.RequestPasswordResetAsync(email);            
        }

        public async Task LogOut()
        {
            await DisconnectUserFromInstallation();
            await ParseUser.LogOutAsync()
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        t.Exception.Handle(ex => true);
                        //ParseErrorHandler.HandleParseError(t.Exception.InnerException as ParseException);
                    }
                    _notificationCenter.Send(NotificationKeys.CurrentUserChanged, _currentUser);
                });
        }

        public async Task<IList<Session>> GetSessions()
        {
            var sessions = (await ParseSession.Query
                .WhereEqualTo("user", ParseUser.CurrentUser)
                .FindAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        ParseErrorHandler.HandleParseError(t.Exception.InnerException as ParseException);
                    }

                    return t.Result;
                })).ToList();


            var installations = (await ParseCloud.CallFunctionAsync<IList<ApParseInstallation>>("installations", null))
                .ToDictionary(x => x.InstallationId.ToString());

            var resultSessions = new List<Session>();

            foreach (var parseSession in sessions)
            {
                var expiresAt = parseSession["expiresAt"] as DateTime?;
                var installationId = parseSession["installationId"] as string;
                var session = new Session
                {
                    Id = parseSession.ObjectId,
                    CreatedAt = parseSession.CreatedAt.HasValue ? parseSession.CreatedAt.Value.ToLocalTime() : parseSession.CreatedAt,
                    ExpiresAt = expiresAt.HasValue ? expiresAt.Value.ToLocalTime() : (DateTime?) null,
                };

                if (!string.IsNullOrEmpty(installationId) && installations.ContainsKey(installationId))
                {
                    var installation = installations[installationId];
                    session.DeviceInfo = installation.DeviceInfo;
                }

                resultSessions.Add(session);
            }

            return resultSessions;
        }

        private Task AssociateInstallationWithUser()
        {
            // Associate the device with a user
            var installation = ParseInstallation.CurrentInstallation;
            var parseInstallation = installation as ApParseInstallation;
            if (parseInstallation != null)
            {
                parseInstallation.User = ParseUser.CurrentUser;
                parseInstallation.DeviceInfo = GetDeviceString();
            }

            return installation.SaveAsync();
        }

        private Task DisconnectUserFromInstallation()
        {
            // Disconnect the device from user
            ParseInstallation.CurrentInstallation.Remove("user");
            return ParseInstallation.CurrentInstallation.SaveAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    t.Exception.Handle(ex => true);
                }

            });
        }

        public string GetDeviceString()
        {
            return string.Format("{0} {1} {2}", _deviceInfo.Platform, _deviceInfo.Version, _deviceInfo.Model);
        }
    }
}
