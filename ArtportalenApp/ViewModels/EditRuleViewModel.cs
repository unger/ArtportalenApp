using System;
using System.ComponentModel;
using System.Linq;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.Views;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class EditRuleViewModel : ViewModelBase
    {
        private readonly IRuleStorage _ruleStorage;
        private readonly IProvinceService _provinceService;
        private readonly IMunicipalityService _municipalityService;
        private Command _saveCommand;
        private Command _changeTaxonsCommand;
        private Command _changeMunicipalityCommand;
        private Command _changeProvinceCommand;

        public EditRuleViewModel(IRuleStorage ruleStorage, IProvinceService provinceService, IMunicipalityService municipalityService)
        {
            _ruleStorage = ruleStorage;
            _provinceService = provinceService;
            _municipalityService = municipalityService;

            PropertyChanged += EditRuleViewModel_PropertyChanged;
        }

        void EditRuleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new Command(async () =>
                {
                    IsBusy = true;
                    SaveCommand.ChangeCanExecute();
                    try
                    {
                        await _ruleStorage.SaveRule(CurrentRule);
                        await Navigation.PopAsync();
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                    }
                    finally
                    {
                        IsBusy = false;
                        SaveCommand.ChangeCanExecute();
                    }
                },
                () => !IsBusy && IsValid));
            }
        }

        public Command ChangeTaxonsCommand
        {
            get
            {
                return _changeTaxonsCommand ?? (_changeTaxonsCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<ChooseTaxonsPage, ChooseTaxonsViewModel>(
                        init: vm =>
                        {
                            vm.SetSelectedTaxons(CurrentRule.Taxons);
                        },
                        done: async vm =>
                        {
                            var taxons = vm.Taxons.Where(t => t.Selected).Select(t => t.Name);
                            CurrentRule.Taxons = taxons.ToArray();

                            await vm.Navigation.PopAsync();
                        });
                }));
            }
        }

        public Command ChangeMunicipalityCommand
        {
            get
            {
                return _changeMunicipalityCommand ?? (_changeMunicipalityCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<ChooseValuesPage, ChooseValuesViewModel>(
                        init: vm =>
                        {
                            vm.SetValues(_municipalityService.GetAll().Select(x => x.Name).ToArray(), CurrentRule.Kommuner);
                        },
                        done: async vm =>
                        {
                            CurrentRule.Kommuner = vm.AllValues.Where(t => t.Selected).Select(t => t.Name).ToArray();
                            await vm.Navigation.PopAsync();
                        });
                }));
            }
        }

        public Command ChangeProvinceCommand
        {
            get
            {
                return _changeProvinceCommand ?? (_changeProvinceCommand = new Command(async () =>
                {
                    await Navigation.PushAsync<ChooseValuesPage, ChooseValuesViewModel>(
                        init: vm =>
                        {
                            vm.SetValues(_provinceService.GetAll().Select(x => x.Name).ToArray(), CurrentRule.Landskap);
                        },
                        done: async vm =>
                        {
                            CurrentRule.Landskap = vm.AllValues.Where(t => t.Selected).Select(t => t.Name).ToArray();
                            await vm.Navigation.PopAsync();
                        });
                }));
            }
        }

        public string ErrorMessage
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool IsBusy
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IsValid
        {
            get { return CurrentRule != null && CurrentRule.IsValid; }
        }

        public Rule CurrentRule
        {
            get { return GetProperty<Rule>(); }
            set { SetProperty(value); }
        }
    }
}
