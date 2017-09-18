using Telerik.WinControls.UI;
using System.Windows.Forms;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class RowDetailsBehavior : BaseGridBehavior
    {
        public override bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                GridViewElement.Template.SynchronizationService.BeginDispatch();
            }

            bool result = base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                GridViewElement.Template.SynchronizationService.EndDispatch(true);
            }

            return result;
        }
    }

}
