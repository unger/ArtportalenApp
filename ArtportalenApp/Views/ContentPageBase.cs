using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.ViewModels;
using Xamarin.Forms;

namespace ArtportalenApp.Views
{
    public class ContentPageBase<T> : ContentPage where T : ViewModelBase
    {
        public new T BindingContext
        {
            get { return base.BindingContext as T; }
            set { base.BindingContext = value; }
        }

        public T DataContext
        {
            get { return BindingContext; }
            set { base.BindingContext = value; }
        }
    }
}
