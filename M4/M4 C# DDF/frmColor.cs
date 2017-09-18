/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Drawing;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmColor : NForm
  {
    private Color m_Color;

    public frmColor()
    {
      InitializeComponent();

      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      m_Color = NColorPane1.Color;
      DialogResult = DialogResult.OK;
      Close();
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    public Color GetColor(Color OriginalColor)
    {
      m_Color = OriginalColor;
      ShowDialog();
      return m_Color;
    }
  }
}
