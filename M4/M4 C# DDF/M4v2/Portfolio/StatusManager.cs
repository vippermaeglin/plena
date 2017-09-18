using System.Windows.Forms;

namespace M4.M4v2.Portfolio
{
    public partial class StatusManager : UserControl
    {
        public StatusManager()
        {
            InitializeComponent();

            outputWindowV21.AllowDrop = true;
            outputWindowV21.DragEnter += ctlWeb_DragEnter;
            outputWindowV21.DragDrop += ctlWeb_DragDrop;
        }

        public void SetMessage(string info, OutputWindowV2.OutputIcon icon)
        {
            outputWindowV21.DisplayConnectionStatus(info, icon);
        }

        void ctlWeb_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        void ctlWeb_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show(e.Data.GetData(DataFormats.Text).ToString());
        }
    }
}
