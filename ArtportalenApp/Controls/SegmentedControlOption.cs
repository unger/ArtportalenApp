using Xamarin.Forms;

namespace ArtportalenApp.Controls
{
    public class SegmentedControlOption: View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create<SegmentedControlOption, string>(p => p.Text, "");

        public static readonly BindableProperty ValueProperty = BindableProperty.Create<SegmentedControlOption, string>(p => p.Value, "");

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty) ?? Text; }
            set { SetValue(ValueProperty, value); }
        }
    }
}