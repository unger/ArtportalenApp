using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ArtportalenApp.Controls
{
	public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
	{
        //public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create<SegmentedControl, string>(p => p.SelectedValue, "");

        public static BindableProperty SelectedValueProperty =
            BindableProperty.Create<SegmentedControl, string>(ctrl => ctrl.SelectedValue,
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (SegmentedControl)bindable;
                    if (oldValue != newValue)
                    {
                        ctrl.SelectedValue = newValue;
                    }
                });


        public SegmentedControl()
        {
            Children = new List<SegmentedControlOption>();
        }

        public IList<SegmentedControlOption> Children { get; set; }

		public event ValueChangedEventHandler ValueChanged;

		public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public string SelectedValue
        {
            get { return (string)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
    }
}