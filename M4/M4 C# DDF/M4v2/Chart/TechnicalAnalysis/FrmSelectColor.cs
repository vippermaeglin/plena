using System.Drawing;
using System.Windows.Forms;

namespace M4.M4v2.Chart.TechnicalAnalysis
{
    public partial class FrmSelectColor : Telerik.WinControls.UI.RadForm
    {
        public Color SelectedColor { get; set; }

        public void SetSelectedColor()
        {
            cslColor.SelectedColor = SelectedColor;
        }

        public FrmSelectColor()
        {
            InitializeComponent();

            cslColor.ActiveMode = Telerik.WinControls.ColorPickerActiveMode.Basic;
        }

        private void RadColorSelector1CancelButtonClicked1(object sender, Telerik.WinControls.ColorChangedEventArgs args)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void RadColorSelector1OkButtonClicked(object sender, Telerik.WinControls.ColorChangedEventArgs args)
        {
            SelectedColor = cslColor.SelectedColor;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
