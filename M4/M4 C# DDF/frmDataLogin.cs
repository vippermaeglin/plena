/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

//Data feed API login form

namespace M4
{
  public partial class frmDataLogin : NForm
  {
    private readonly bool m_ExitOnCancel;
    private bool m_ok;

    public frmDataLogin(bool ExitOnCancel) //Flag to exit the application if the login fails
    {
      InitializeComponent();

      m_ExitOnCancel = ExitOnCancel;
      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;

      txtUsername.GotFocus += (sender,e) => txtUsername.SelectAll();
      txtPassword.GotFocus += (sender,e) => txtPassword.SelectAll();
    }

    private void frmDataLogin_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!m_ok) e.Cancel = true;
    }

    private void frmDataLogin_Load(object sender, EventArgs e)
    {
      txtUsername.Text = Properties.Settings.Default.Username;
      txtPassword.Text = Properties.Settings.Default.Password;      
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      this.TopMost = false;
      m_ok = true;
      Properties.Settings.Default.Username = txtUsername.Text;
      Properties.Settings.Default.Password = txtPassword.Text;
      DialogResult = DialogResult.OK;
      Close();
      Application.DoEvents();
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      m_ok = true;
      if (m_ExitOnCancel)
      {
        Environment.Exit(0);
        return;
      }
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.TopMost = true;      
      timer1.Enabled = false;
    }

    private void _lblCaption_0_Click(object sender, EventArgs e)
    {

    }

  }
}
