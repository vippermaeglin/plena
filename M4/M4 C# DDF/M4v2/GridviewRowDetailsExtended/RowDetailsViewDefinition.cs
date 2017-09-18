using Telerik.WinControls.UI;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class RowDetailsViewDefinition : TableViewDefinition
    {
        public override IRowView CreateViewUIElement(GridViewInfo viewInfo)
        {
            GridTableElement tableElement = (GridTableElement)base.CreateViewUIElement(viewInfo);
            tableElement.ViewElement.RowLayout = CreateRowLayout();
            return tableElement;
        }

        public override IGridRowLayout CreateRowLayout()
        {
            return new RowDetailsLayout();
        }
    }
}
