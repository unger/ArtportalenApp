using System;
using System.Threading.Tasks;
using ArtportalenApp.Services;

namespace ArtportalenApp.Interfaces
{
    public interface IViewModel
    {
        string Title { get; set; }

        bool IsDone { get; set; }

        void SetState<T>(Action<T> action) where T : class;

        void SetDoneAction(Func<Task> action);

        void SetCancelAction(Func<Task> action);

        Task DoneAction();

        Task CancelAction();

        INavigationService Navigation { get; set; }

        void Appearing();

        void Disappearing();
    }
}
