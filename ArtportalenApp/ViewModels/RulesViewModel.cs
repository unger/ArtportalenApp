using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArtportalenApp.Constants;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class RulesViewModel : ViewModelBase
    {
        private readonly IRuleStorage _ruleStorage;
        private Command _refreshCommand;
        private Command _previewCommand;
        private Command _editCommand;
        private Command _deleteCommand;
        private Command _addCommand;

        public RulesViewModel(IRuleStorage ruleStorage, INotificationCenter notificationCenter)
        {
            _ruleStorage = ruleStorage;

            Title = "Regler";

            Device.BeginInvokeOnMainThread(RefreshRules);

            notificationCenter.Subscribe<User>(this, NotificationKeys.CurrentUserChanged, u =>
            {
                Device.BeginInvokeOnMainThread(RefreshRules);
            });
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool HasRules
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool HasNoRules
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(RefreshRules, () => !IsBusy)); }
        }

        public Command PreviewCommand
        {
            get
            {
                return _previewCommand ?? (_previewCommand = new Command(async x =>
                {
                    var rule = x as Rule;
                    if (rule != null)
                    {
                        await Navigation.PushAsync<SightingsPage, SightingsViewModel>(
                            init: vm =>
                            {
                                vm.Title = rule.Name;
                                vm.RuleId = rule.Id;
                            });
                    }
                }));
            }
        }

        public Command EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new Command(async x =>
                {
                    var rule = x as Rule;
                    if (rule != null)
                    {
                        await Navigation.PushAsync<EditRulePage, EditRuleViewModel>(
                            init: vm =>
                            {
                                vm.CurrentRule = rule;
                            },
                            done: vm =>
                            {
                                RefreshRules();
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
                    var rule = x as Rule;
                    if (rule != null)
                    {
                        await _ruleStorage.DeleteRule(rule.Id);
                        RefreshRules();
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
                    await Navigation.PushAsync<EditRulePage, EditRuleViewModel>(
                        init: vm =>
                        {
                            vm.CurrentRule = new Rule();
                        },
                        done: vm =>
                        {
                            RefreshRules();
                        });
                }));
            }
        }

        public ObservableCollection<Rule> Rules
        {
            get { return GetProperty<ObservableCollection<Rule>>(); }
            set
            {
                SetProperty(value);

                HasRules = value != null && value.Count > 0;
                HasNoRules = !HasRules;
            }
        }

        private async void RefreshRules()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            RefreshCommand.ChangeCanExecute();

            try
            {
                var rules = await _ruleStorage.GetRules();
                Rules = new ObservableCollection<Rule>(rules);
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
