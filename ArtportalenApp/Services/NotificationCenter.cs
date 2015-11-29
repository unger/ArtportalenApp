using System;
using ArtportalenApp.Interfaces;

namespace ArtportalenApp.Services
{
    public class NotificationCenter : INotificationCenter
    {
        private readonly object _obj = new object();

        public void Subscribe<TArg>(object subscriber, string key, Action<TArg> callback)
        {
            Xamarin.Forms.MessagingCenter.Subscribe<object, TArg>(subscriber, key, (o, a) => callback(a));
        }
        
        public void Send<TArg>(string key, TArg argument)
        {
            Xamarin.Forms.MessagingCenter.Send(_obj, key, argument);
        }

        public void Unsubscribe<TArg>(object subscriber, string key)
        {
            Xamarin.Forms.MessagingCenter.Unsubscribe<object, TArg>(subscriber, key);
        }
    }
}
