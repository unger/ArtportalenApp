using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ArtportalenApp.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(TextCell), typeof(CustomTextCellRenderer))] 

namespace ArtportalenApp.Droid.Controls
{
    public class CustomTextCellRenderer : TextCellRenderer
    {
        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);
        }

        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            return base.GetCellCore(item, convertView, parent, context);
        }
    }
}