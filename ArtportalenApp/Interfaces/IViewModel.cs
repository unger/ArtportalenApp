using System;
using ArtportalenApp.Services;

namespace ArtportalenApp.Interfaces
{
    public interface IViewModel
    {
        string Title { get; set; }

        void SetState<T>(Action<T> action) where T : class;

        INavigationService Navigation { get; set; }

        void Appearing();

        void Disappearing();
    }
}
