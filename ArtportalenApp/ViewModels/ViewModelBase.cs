﻿using System;
using System.Threading.Tasks;
using ArtportalenApp.Interfaces;

namespace ArtportalenApp.ViewModels
{
    public abstract class ViewModelBase : NotifyPropertyChangedBase, IViewModel
    {
        private Func<Task> doneAction; 

        public string Title
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool IsDone
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public void SetState<T>(Action<T> action) where T : class
        {
            action(this as T);
        }

        public void SetDoneAction(Func<Task> action)
        {
            doneAction = action;
        }

        public async Task DoneAction()
        {
            if (doneAction != null)
            {
                await doneAction();
            }
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
