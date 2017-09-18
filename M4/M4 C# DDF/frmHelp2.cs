using System;
using System.Windows.Forms;

namespace M4
{
    public partial class frmHelp2 : Telerik.WinControls.UI.RadForm
    {
        //TODO: Set your website URL here:
        private const string DomainURL = "http://www.plena-tp.com.br";
        public string button = "Fechar";
        public frmHelp2()
        {
            InitializeComponent();
            this.Text = "Sobre PLENA";
            Label2.Text = "PLENA Trading Platform - Versão " + Program.VERSION + Program.LanguageDefault.DictionaryPlena["about"];
            LinkLabel1.Text = DomainURL;
            radButton1.Text = button;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(DomainURL);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
