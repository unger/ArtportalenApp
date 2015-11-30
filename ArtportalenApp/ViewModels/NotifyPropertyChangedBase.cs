using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ArtportalenApp.ViewModels
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged, IDisposable
    {
        private readonly object _lockObj = new object();
        private readonly Dictionary<string, object> _propertyFields = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected virtual bool SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                lock (_lockObj)
                {
                    var oldvalue = GetProperty<T>(propertyName);
                    if (object.Equals(oldvalue, value)) return false;

                    // Listen to childupdates, needs some more testing
                    var prefix = propertyName + ".";
                    PropertyChangedEventHandler handler = (sender, e) =>
                    {
                        RaisePropertyChanged(prefix + e.PropertyName);
                    };
                    if (oldvalue is INotifyPropertyChanged)
                    {
                        (oldvalue as INotifyPropertyChanged).PropertyChanged -= handler;
                    }
                    if (value is INotifyPropertyChanged)
                    {
                        (value as INotifyPropertyChanged).PropertyChanged += handler;
                    }

                    _propertyFields[propertyName] = value;
                    RaisePropertyChanged(propertyName);
                }
                return true;
            }

            return false;
        }

        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            return GetProperty<T>(default(T), propertyName);
        }

        protected T GetProperty<T>(T defaultValue, [CallerMemberName] string propertyName = null)
        {
            lock (_lockObj)
            {
                if (_propertyFields.ContainsKey(propertyName))
                {
                    return ConvertValue<T>(_propertyFields[propertyName], defaultValue);
                }
            }

            return defaultValue;
        }

        protected void RaisePropertyChanged<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof (TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            return member.Member.Name;
        }

        private T ConvertValue<T>(object value, T defaultValue)
        {
            try
            {
                return (T) value;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                lock (_lockObj)
                {
                    foreach (var key in _propertyFields.Keys.ToArray())
                    {
                        var obj = _propertyFields[key] as IDisposable;
                        if (obj != null)
                        {
                            obj.Dispose();
                        }
                        else
                        {
                            var list = _propertyFields[key] as IList;
                            if (list != null)
                            {
                                foreach (var item in list)
                                {
                                    var disposableItem = item as IDisposable;
                                    if (disposableItem != null)
                                    {
                                        disposableItem.Dispose();
                                    }
                                }
                                list.Clear();
                            }
                        }

                        _propertyFields.Remove(key);
                    }
                }
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~NotifyPropertyChangedBase()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
    }
}
