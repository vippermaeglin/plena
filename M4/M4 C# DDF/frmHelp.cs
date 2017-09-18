/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

namespace M4
{
    public partial class frmHelp : NForm
    {
        //TODO: Set your website URL here:
        private const string DomainURL = "http://www.plena-tp.com.br";

        public frmHelp()
        {
            InitializeComponent();

            if (frmMain.NevronPalette != null)
                Palette = frmMain.NevronPalette;

            Label2.Text = Program.LanguageDefault.DictionaryPlena["about"];
            LinkLabel1.Text = DomainURL;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(DomainURL);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
