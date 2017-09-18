/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Linq;
using System.Windows.Forms;
using M4.M4v2.Chart;
using Nevron.UI.WinForm.Controls;
using Telerik.WinControls.UI.Docking;

namespace M4
{
    public partial class frmPopoutChart : NForm
    {
        public Guid IdTab { get; set; }
        public Control _myParent;
        public CtlPainelChart MActiveChart { get; set; }

        public frmPopoutChart()
        {
            InitializeComponent();

            Focus();

        }

        // Pop the chart back into the tab
        private void frmPopoutChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmMain.GInstance.ListPopout.Any(cada => cada == this))
                frmMain.GInstance.ListPopout.Remove(this);
            if (_myParent != null && Controls[0] != null)
            {
                Controls[0].Parent = _myParent;
            }

        }



        private void frmPopoutChart_Activated(object sender, EventArgs e)
        {

            frmMain.GInstance.MActiveChart = null;
            frmMain.GInstance.MActiveChart = MActiveChart;

        }

        private void frmPopoutChart_Deactivate(object sender, EventArgs e)
        {

            frmMain.GInstance.TabActivate();
        }
    }
}
