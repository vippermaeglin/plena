using System;
using Telerik.WinControls.UI;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class RowDetailsRowElement : GridDataRowElement
    {
        protected override Type ThemeEffectiveType
        {
            get { return typeof(GridDataRowElement); }
        }

        public override void UpdateInfo()
        {
            GridViewRowDetailsExtended grid = ((GridViewRowDetailsExtended)GridControl);

            if (grid != null)
            {
                if (grid.EnableAllExpand)
                {
                    RowInfo.Height = grid.ShowRowDetails ? grid.DetailsRowHeight : TableElement.RowHeight;
                }
                else
                {
                    if (grid.ShowRowDetails)
                    {
                        RowInfo.Height = !RowInfo.IsCurrent ? TableElement.RowHeight : ((GridViewRowDetailsExtended)GridControl).DetailsRowHeight;
                    }
                    else
                    {
                        RowInfo.Height = TableElement.RowHeight;
                    }

                }
            }
            base.UpdateInfo();

        }
    }

}
