using System;
using System.Collections.Generic;
using System.Linq;
using ArtportalenApp.Constants;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class SightingsViewModel : ViewModelBase
    {
        private readonly ISightingStorage _sightingStorage;
        private Command _refreshCommand;

        public SightingsViewModel(ISightingStorage sightingStorage, INotificationCenter notificationCenter)
        {
            _sightingStorage = sightingStorage;

            Title = "Observationer";
            DaysFilter = "";

            notificationCenter.Subscribe<User>(this, NotificationKeys.CurrentUserChanged, u =>
            {
                Device.BeginInvokeOnMainThread(RefreshSightings);
            });
        }

        public override void Appearing()
        {
            IsLoaded = true;
            Device.BeginInvokeOnMainThread(RefreshSightings);
        }

        public bool IsLoaded
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool HasSightings
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool HasNoSightings
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public Command RefreshCommand
        {
            get {  return _refreshCommand ?? (_refreshCommand = new Command(RefreshSightings, () => !IsBusy)); }
        }

        public string DaysFilter
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                Device.BeginInvokeOnMainThread(RefreshSightings);
            }
        }

        public bool OrderByLatest
        {
            get { return DaysFilter == ""; }
        }

        public bool OrderByTaxon
        {
            get { return DaysFilter != ""; }
        }

        public string RuleId
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                Device.BeginInvokeOnMainThread(RefreshSightings);
            }
        }

        public List<IGrouping<string, Sighting>> GroupedSightings
        {
            get { return GetProperty<List<IGrouping<string, Sighting>>>(); }
            set { SetProperty(value); }
        }

        public List<Sighting> Sightings
        {
            get { return GetProperty<List<Sighting>>(); }
            set
            {
                if (value != null)
                {
                    SetProperty(value.OrderByDescending(s => s.CreatedAt).ToList());
                    if (OrderByLatest)
                    {
                        GroupedSightings = value
                            .OrderByDescending(s => s.CreatedAt)
                            .GroupBy(x => GetDateGroupingHeader(x.CreatedAt))
                            .ToList();
                    }
                    else
                    {
                        GroupedSightings = value
                            .OrderBy(s => s.TaxonName)
                            .ThenBy(s => s.Landskap)
                            .GroupBy(x => x.TaxonName + ", " + x.Landskap)
                            .ToList();
                    }

                    HasSightings = Sightings.Count > 0;
                    HasNoSightings = Sightings.Count == 0;
                }
                else
                {
                    HasSightings = false;
                    HasNoSightings = false;
                    SetProperty((List<Sighting>)null);
                    GroupedSightings = null;
                }
            }
        }

        private string GetDateGroupingHeader(DateTime? createdAt)
        {
            if (createdAt.HasValue)
            {
                return createdAt.Value.ToString("ddd d MMMM yyyy");
            }

            return "datum saknas";
        }

        private async void RefreshSightings()
        {
            if (IsBusy || !IsLoaded)
            {
                return;
            }

            IsBusy = true;
            RefreshCommand.ChangeCanExecute();

            try
            {
                Sightings = null;
                var sightings = await _sightingStorage.Match(DaysFilter, RuleId);
                Sightings = new List<Sighting>(sightings);
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                IsBusy = false;
                RefreshCommand.ChangeCanExecute();
            }
        }
    }
}
