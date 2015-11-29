using ArtportalenApp.iOS.Controls;
using ArtportalenApp.iOS.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(StandardViewCellRenderer))] 

namespace ArtportalenApp.iOS.Controls
{
    public class StandardViewCellRenderer : ViewCellRenderer
    {
        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell,
            UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.Accessory = CellAccessoryHelper.GetCellAccessory(item.StyleId);
            return cell;
        }
    }
}