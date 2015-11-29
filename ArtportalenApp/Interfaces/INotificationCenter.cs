using System;

namespace ArtportalenApp.Interfaces
{
    public interface INotificationCenter
    {
        void Subscribe<TArg>(object subscriber, string key, Action<TArg> callback);
        void Unsubscribe<TArg>(object subscriber, string key);
        void Send<TArg>(string key, TArg argument);
    }
}
