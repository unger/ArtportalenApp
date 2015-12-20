using System;
using System.Collections.Generic;
using System.Linq;
using ArtportalenApp.Constants;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IReportStorage _reportStorage;
        private Command _refreshCommand;
        private Command _editCommand;
        private Command _deleteCommand;
        private Command _addCommand;

        public ReportsViewModel(IReportStorage reportStorage, INotificationCenter notificationCenter)
        {
            _reportStorage = reportStorage;

            Title = "Mina rapporter";

            notificationCenter.Subscribe<User>(this, NotificationKeys.CurrentUserChanged, u =>
            {
                Device.BeginInvokeOnMainThread(RefreshReports);
            });
        }

        public override void Appearing()
        {
            IsLoaded = true;
            Device.BeginInvokeOnMainThread(RefreshReports);
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

        public Command RefreshCommand
        {
            get {  return _refreshCommand ?? (_refreshCommand = new Command(RefreshReports, () => !IsBusy)); }
        }

        public Command EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new Command(async x =>
                {
                    var report = x as Report;
                    if (report != null)
                    {
                        await Navigation.PushAsync<EditReportPage, EditReportViewModel>(
                            init: vm =>
                            {
                                vm.CurrentReport = report;
                            },
                            done: async vm =>
                            {
                                await vm.Navigation.PopAsync();
                                RefreshReports();
                            });
                    }
                }));
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new Command(async x =>
                {
                    var report = x as Report;
                    if (report != null)
                    {
                        await _reportStorage.DeleteReport(report.Id);
                        RefreshReports();
                    }
                }));
            }
        }

        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<EditReportPage, EditReportViewModel>(
                        init: vm =>
                        {
                            vm.CurrentReport = new Report();
                        },
                        done: async vm =>
                        {
                            await vm.Navigation.PopAsync();
                            RefreshReports();
                        });
                }));
            }
        }

        public List<IGrouping<string, Report>> GroupedReports
        {
            get { return GetProperty<List<IGrouping<string, Report>>>(); }
            set { SetProperty(value); }
        }

        public List<Report> Reports
        {
            get { return GetProperty<List<Report>>(); }
            set
            {
                if (value != null)
                {
                    SetProperty(value.OrderByDescending(s => s.EndDate).ToList());
                        GroupedReports = value
                            .OrderByDescending(s => s.EndDate)
                            .GroupBy(x => GetDateGroupingHeader(x.EndDate))
                            .ToList();
                }
                else
                {
                    SetProperty((List<Report>)null);
                    GroupedReports = null;
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

        private async void RefreshReports()
        {
            if (IsBusy || !IsLoaded)
            {
                return;
            }

            IsBusy = true;
            RefreshCommand.ChangeCanExecute();

            try
            {
                Reports = null;
                var reports = await _reportStorage.GetReports();
                Reports = new List<Report>(reports);
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