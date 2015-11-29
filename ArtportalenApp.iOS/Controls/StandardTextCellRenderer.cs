using System;
using System.Collections.Generic;
using System.Text;
using ArtportalenApp.iOS.Controls;
using ArtportalenApp.iOS.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TextCell), typeof(StandardTextCellRenderer))] 
namespace ArtportalenApp.iOS.Controls
{
    public class StandardTextCellRenderer : TextCellRenderer
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
