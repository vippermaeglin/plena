/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System.Windows.Forms;

namespace M4.M4v2.Scanner
{
  public partial class frmScannerScript : Form
  {
    private readonly ctlScanner m_owner;

    public frmScannerScript(ctlScanner owner)
    {
      InitializeComponent();

      m_owner = owner;

    }

    public string HeaderText
    {
      get { return NGrouper1.Text; }
      set { NGrouper1.Text = value; }
    }

    public string Script
    {
      get { return txtScript.Text; }
      set { txtScript.Text = value; }
    }

    private void OK_Button_Click(object sender, System.EventArgs e)
    {
      m_owner.TempScript = txtScript.Text;
      DialogResult = DialogResult.OK;
      Close();
    }

    private void Cancel_button_Click(object sender, System.EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void cmdDocumentation_Click(object sender, System.EventArgs e)
    {
      DialogResult = DialogResult.Abort;
      Close();
    }

    private void frmScannerScript_Load(object sender, System.EventArgs e)
    {
      txtScript.Text = m_owner.TempScript;
    }
  }
}
