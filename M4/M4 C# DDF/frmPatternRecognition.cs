/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmPatternRecognition : NForm
  {
    private string m_PatternFile;

    public frmPatternRecognition()
    {
      InitializeComponent();

      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    //Returns a pattern definition file.
    public string GetPatternDefinitionFile()
    {
      m_PatternFile = "";
      ShowDialog();
      return m_PatternFile;
    }

    private void LoadPatterns()
    {
      cboPattern.Items.Clear();
      cboPattern.Items.Add("<launch pattern designer...>");
      cboPattern.Items[0].Tag = "Launch Designer";
      string FileTitle = "";
      string path = Application.ExecutablePath;
      short found = (short)path.LastIndexOf(@"\");

      if (found > -1)
      {
        path = path.Substring(0, found);
      }
      try
      {
        foreach (string File in Directory.GetFiles(path + @"\Res\Patterns\", "*.apr.xml"))
        {
          found = (short)File.LastIndexOf(@"\");
          if (found > -1)
          {
            FileTitle = (File.Substring(found + 1).Replace(".apr.xml", ""));
          }
          cboPattern.Items.Add(FileTitle);
          cboPattern.Items[cboPattern.Items.Count - 1].Tag = File;
        }
      }
      catch (Exception)
      {
        Directory.CreateDirectory(path + @"\Res\Patterns\");
      }
    }

    private void frmPatternRecognition_Load(object sender, EventArgs e)
    {
      LoadPatterns();
    }

    private void cboPattern_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cboPattern.SelectedItem.ToString() == "Launch Designer")
      {
        string path = Application.ExecutablePath;
        short found = (short)path.LastIndexOf(@"\");
        if (found > -1)
        {
          path = path.Substring(0, found);
        }
        path = path + @"\Res\Patterns\APRDesigner.exe";
        if (!File.Exists(path))
        {
          MessageBox.Show("Pattern Designer is not installed on this system!", "Error:",
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          ProcessStartInfo startInfo = new ProcessStartInfo
                                         {
                                           FileName = path,
                                           WindowStyle = ProcessWindowStyle.Normal
                                         };
          Process.Start(startInfo);
          m_PatternFile = "";
          DialogResult = DialogResult.OK;
          Close();
        }
      }
      else
      {
        m_PatternFile = Convert.ToString(cboPattern.SelectedItem);
      }
    }
  }
}
