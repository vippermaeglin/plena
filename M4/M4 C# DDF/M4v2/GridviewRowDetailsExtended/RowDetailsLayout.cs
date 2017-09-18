using System.Linq;
using Telerik.WinControls.UI;
using System.Drawing;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class RowDetailsLayout : TableViewRowLayout
    {
        private int _systemWidth;
        private SizeF _availableSize;

        public override void InvalidateRenderColumns()
        {
            base.InvalidateRenderColumns();

            _systemWidth = 0;

            foreach (GridViewColumn column in RenderColumns.Where(column => !(column is GridViewDataColumn)))
            {
                _systemWidth += column.Width + Owner.CellSpacing;
            }

            if (_systemWidth > 0)
            {
                _systemWidth -= Owner.CellSpacing;
            }
        }

        public override SizeF MeasureRow(SizeF availableSize)
        {
            SizeF desiredSize = base.MeasureRow(availableSize);
            _availableSize = availableSize;
            return desiredSize;
        }

        public override RectangleF ArrangeCell(RectangleF clientRect, GridCellElement cell)
        {
            GridViewRowDetailsExtended grid = (GridViewRowDetailsExtended)Owner.GridViewElement.GridControl;
            if (ReferenceEquals(cell.ColumnInfo, grid.DetailsColumn))
            {
                cell.StretchHorizontally = true;
                cell.BypassLayoutPolicies = true;

                if (cell.RowElement is RowDetailsRowElement)
                {

                    if (((GridViewRowDetailsExtended)cell.RowElement.GridControl).EnableAllExpand && ((GridViewRowDetailsExtended)cell.RowElement.GridControl).ShowRowDetails)
                    {
                        return new RectangleF(0, Owner.RowHeight, _availableSize.Width - _systemWidth, grid.DetailsRowHeight - Owner.RowHeight);
                    }
                    else if (((GridViewRowDetailsExtended)cell.RowElement.GridControl).ShowRowDetails)
                    {
                        return cell.RowElement.IsCurrent ? new RectangleF(0, Owner.RowHeight, _availableSize.Width - _systemWidth, grid.DetailsRowHeight - Owner.RowHeight) : RectangleF.Empty;
                    }
                    else
                    {
                        return RectangleF.Empty;
                    }
                }
                else
                {
                    return RectangleF.Empty;
                }
            }

            if (cell is GridDataCellElement)
            {
                clientRect.Height = Owner.RowHeight;
            }

            return base.ArrangeCell(clientRect, cell);
        }
    }
}
