using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArtportalenApp.Controls;
using ArtportalenApp.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(ArtportalenApp.Controls.SegmentedControl), typeof(SegmentedControlRenderer))]
namespace ArtportalenApp.iOS.Controls
{
	public class SegmentedControlRenderer : ViewRenderer<ArtportalenApp.Controls.SegmentedControl, UISegmentedControl>
	{
	    private UISegmentedControl _segmentedControl;

		protected override void OnElementChanged (ElementChangedEventArgs<ArtportalenApp.Controls.SegmentedControl> e)
		{
			base.OnElementChanged (e);

            if (e.NewElement == null)
		    {
		        return;
		    }

			_segmentedControl = new UISegmentedControl();
		    var valueDict = new Dictionary<string, string>();

			for (var i = 0; i < e.NewElement.Children.Count; i++) {
                valueDict.Add(e.NewElement.Children[i].Text, e.NewElement.Children[i].Value);
				_segmentedControl.InsertSegment(e.NewElement.Children[i].Text, i, false);
			}

			_segmentedControl.ValueChanged += (sender, eventArgs) =>
			{
			    var title = _segmentedControl.TitleAt(_segmentedControl.SelectedSegment);

                e.NewElement.SelectedValue = valueDict[title];
			};

			SetNativeControl(_segmentedControl);

            if (e.NewElement.SelectedValue != null)
            {
                OnElementPropertyChanged(e.NewElement, new PropertyChangedEventArgs(SegmentedControl.SelectedValueProperty.PropertyName));
            }
		}

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == SegmentedControl.SelectedValueProperty.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("Changed SelectedValueProperty: {0}", Element.SelectedValue);

                var found = false;
                for (var i = 0; i < Element.Children.Count; i++)
                {
                    var option = Element.Children[i];
                    if (option.Value == Element.SelectedValue)
                    {
                        found = true;
                        if (_segmentedControl.SelectedSegment != i)
                        {
                            _segmentedControl.SelectedSegment = i;
                        }
                    }
                }
                if (!found)
                {
                    if (_segmentedControl.SelectedSegment != -1)
                    {
                        _segmentedControl.SelectedSegment = -1;
                    }
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged: {0}", e.PropertyName);
            }
        }
	}
}