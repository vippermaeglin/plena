using System.Windows.Forms;

namespace M4.M4v2.FormDock
{
    public partial class StatusManager : UserControl
    {
        public StatusManager()
        {
            InitializeComponent();
        }

        public void SetMessage(string info, OutputWindowV2.OutputIcon icon)
        {
            outputWindowV21.DisplayConnectionStatus(info, icon);
        }
    }
}
