using System;
using ArtportalenApp.Interfaces;

namespace ArtportalenApp.ViewModels
{
    public abstract class ViewModelBase : NotifyPropertyChangedBase, IViewModel
    {
        public string Title
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public void SetState<T>(Action<T> action) where T : class
        {
            action(this as T);
        }

        public INavigationService Navigation { get; set; }

        public virtual void Appearing()
        {
        }

        public virtual void Disappearing()
        {
        }
    }
}
