using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace M4.M4v2.Scripts
{
    public partial class ScriptDialog : Telerik.WinControls.UI.RadForm
    {
        public ctlPalette parentForm;
        public string Script;
        public bool ERROR = true;
        public ScriptDialog(ctlPalette parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        void txtScript_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) 
            {
                case Keys.Enter:
                    btOk_Click(new object(), new EventArgs());
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }
        public ScriptDialog(ctlPalette parent, string script)
        {
            InitializeComponent();
            parentForm = parent;
            txtScript.Text = script;
            txtScript.Select(txtScript.TextLength, 0);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (parentForm.TestScripts(txtScript.Text.Trim()))
            {
                Script = txtScript.Text.Trim(new char[]{'\n','\r'});
                ERROR = false;
                Close();
            }
            else ERROR = true;
        }

        private void btTradescript_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.modulusfe.com/tradescript/TradeScript.pdf");
        }

    }
}
