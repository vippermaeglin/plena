/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;
using M4.modulusfe.platform;

namespace M4
{
  public partial class frmActivate : Form
  {
    private bool m_ok;

    public frmActivate()
    {
      InitializeComponent();
    }

    //Calls an ASP page located on an authentication server, which checks the user's license key
    private void cmdActivate_Click(object sender, EventArgs e)
    {
      Service auth = new Service();
      try
      {
        //if (auth.ActivateLicenseKey(frmMain.ClientId, frmMain.ClientPassword, txtActivate.Text))
        //{
        //  m_ok = true;
        //  Properties.Settings.Default.LSTR = txtActivate.Text;
        //}
        //else
        //{
        //  m_ok = false;
        //  Properties.Settings.Default.LSTR = txtActivate.Text;
        //}
      }
      catch (System.Web.Services.Protocols.SoapException se)
      {
        Telerik.WinControls.RadMessageBox.Show(se.Message, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
        m_ok = false;
        Properties.Settings.Default.LSTR = "";
      }
    }

    private void frmActivate_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!m_ok)
        cmdCancel.PerformClick();
    }
  }
}