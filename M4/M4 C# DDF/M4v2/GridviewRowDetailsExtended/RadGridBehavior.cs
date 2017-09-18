using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class RadGridBehavior : BaseGridBehavior
    {
        public override bool ProcessKeyDown(KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 13: //Enter
                case 37: //Left
                //case 38: //Up
                case 39: //Right
                    //case 40: //Down
                    return false;
            }

            return base.ProcessKeyDown(e);
        }
    }
}