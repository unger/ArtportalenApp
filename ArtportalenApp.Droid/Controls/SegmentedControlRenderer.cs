using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using ArtportalenApp.Controls;
using ArtportalenApp.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer (typeof(ArtportalenApp.Controls.SegmentedControl), typeof(SegmentedControlRenderer))]
namespace ArtportalenApp.Droid.Controls
{
	public class SegmentedControlRenderer : ViewRenderer<ArtportalenApp.Controls.SegmentedControl, RadioGroup>
	{
	    private RadioGroup _radioGroup;

		protected override void OnElementChanged(ElementChangedEventArgs<ArtportalenApp.Controls.SegmentedControl> e)
		{
			base.OnElementChanged(e);

			var layoutInflater = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
            var valueDict = new Dictionary<string, string>();

			_radioGroup = new RadioGroup(Context);
			_radioGroup.Orientation = Orientation.Horizontal;
			_radioGroup.CheckedChange += (sender, eventArgs) => {
				var rg = (RadioGroup)sender;
				if (rg.CheckedRadioButtonId != -1) {
					var id = rg.CheckedRadioButtonId;
					var radioButton = rg.FindViewById (id);
					var radioId = rg.IndexOfChild (radioButton);
					var btn = (RadioButton)rg.GetChildAt (radioId);
					var selection = (String)btn.Text;

                    ((IElementController)Element).SetValueFromRenderer(SegmentedControl.SelectedValueProperty, valueDict[selection]);
				}
			};
            
			for (var i = 0; i < e.NewElement.Children.Count; i++) {
				var o = e.NewElement.Children[i];
                valueDict.Add(o.Text, o.Value);
                var v = (SegmentedControlButton)layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
				v.Text = o.Text;
			    if (i == 0)
			    {
                    v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
			    }
                else if (i == e.NewElement.Children.Count - 1)
                {
                    v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                }
				_radioGroup.AddView(v);
			}

			SetNativeControl(_radioGroup);

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
                        var radio = _radioGroup.GetChildAt(i);
                        if (_radioGroup.CheckedRadioButtonId != radio.Id)
                        {
                            _radioGroup.Check(radio.Id);
                        }
                    }
                }
                if (!found)
                {
                    if (_radioGroup.CheckedRadioButtonId != -1)
                    {
                        _radioGroup.ClearCheck();
                    }
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged: {0}", e.PropertyName);
            }
        }

	}

	public class SegmentedControlButton : RadioButton
	{
		private int lineHeightSelected;
		private int lineHeightUnselected;

		private Paint linePaint;

		public SegmentedControlButton (Context context) : this (context, null)
		{
		}

		public SegmentedControlButton (Context context, IAttributeSet attributes) : this (context, attributes, Resource.Attribute.segmentedControlOptionStyle)
		{
		}

		public SegmentedControlButton (Context context, IAttributeSet attributes, int defStyle) : base (context, attributes, defStyle)
		{
			Initialize (attributes, defStyle);
		}

		private void Initialize (IAttributeSet attributes, int defStyle)
		{
			var a = this.Context.ObtainStyledAttributes (attributes, Resource.Styleable.SegmentedControlOption, defStyle, Resource.Style.SegmentedControlOption);

			var lineColor = a.GetColor (Resource.Styleable.SegmentedControlOption_lineColor, 0);
			linePaint = new Paint ();
			linePaint.Color = lineColor;

			lineHeightUnselected = a.GetDimensionPixelSize (Resource.Styleable.SegmentedControlOption_lineHeightUnselected, 0);
			lineHeightSelected = a.GetDimensionPixelSize (Resource.Styleable.SegmentedControlOption_lineHeightSelected, 0);

			a.Recycle ();
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);

			if (linePaint.Color != 0 && (lineHeightSelected > 0 || lineHeightUnselected > 0)) {
				var lineHeight = Checked ? lineHeightSelected : lineHeightUnselected;

				if (lineHeight > 0) {
					var rect = new Rect (0, Height - lineHeight, Width, Height);
					canvas.DrawRect (rect, linePaint);
				}
			}
		}
	}
}