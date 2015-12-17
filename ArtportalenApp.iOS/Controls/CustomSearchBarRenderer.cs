using System.Drawing;
using ArtportalenApp.iOS.Controls;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]


namespace ArtportalenApp.iOS.Controls
{

    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            this.AddDoneButton();
        }

        /// <summary>
        /// Add toolbar with Done button
        /// </summary>
        protected void AddDoneButton()
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
                this.Control
                    .ResignFirstResponder();
            });

            toolbar.Items = new UIBarButtonItem[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };

            Control.InputAccessoryView = toolbar;
            /*
            var protocol = Runtime.GetProtocol("UITextField");

            UITextField searchBarField;
            foreach (UIView subView in Control.Subviews)
            {
                if (subView.ConformsToProtocol(protocol))
                {
                    searchBarField = (UITextField) subView;
                    searchBarField.InputAccessoryView = toolbar;
                }
            }*/
        }
    }

}