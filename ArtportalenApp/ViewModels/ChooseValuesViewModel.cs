using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace ArtportalenApp.ViewModels
{
    public class ChooseValuesViewModel : ViewModelBase
    {
        private Command _doneCommand;

        private string[] selectedValues;

        public Command DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(async () =>
                {
                    await Navigation.PopAsync();
                }));
            }
        }

        public void SetValues(string[] allValues, string[] selectedValues)
        {
            var list = new List<ValueItemModel>();

            allValues = allValues ?? new string[] { };
            selectedValues = selectedValues ?? new string[] { };

            foreach (var value in allValues)
            {
                list.Add(new ValueItemModel
                {
                    Name = value, 
                    Selected = selectedValues.Any(t => t == value)
                });
            }

            foreach (var value in selectedValues)
            {
                if (!list.Any(t => t.Name == value))
                {
                    list.Add(new ValueItemModel { Name = value, Selected = true });
                }
            }

            AllValues = new ObservableCollection<ValueItemModel>(list);
        }

        public ObservableCollection<ValueItemModel> AllValues
        {
            get { return GetProperty<ObservableCollection<ValueItemModel>>(); }
            set
            {
                SetProperty(value);
                FilterValues();
            }
        }

        public ObservableCollection<ValueItemModel> FilteredValues
        {
            get { return GetProperty<ObservableCollection<ValueItemModel>>(); }
            set { SetProperty(value); }
        }

        public string SearchText
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                FilterValues();
            }
        }

        private void FilterValues()
        {
            if (AllValues != null)
            {
                FilteredValues = new ObservableCollection<ValueItemModel>(
                    AllValues.Where(x =>                         
                        string.IsNullOrEmpty(SearchText) || x.Name.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) != -1)
                        );
            }
            else
            {
                FilteredValues = null;
            }
        }

        public class ValueItemModel
        {
            public string Name { get; set; }
            public bool Selected { get; set; }
        } 
    }
}
