using System;
using System.Collections.Generic;
using System.Windows.Forms;
using M4.M4v2.Base;
using Telerik.WinControls.UI;
using M4.DataServer.Interface;

namespace M4.M4v2.Portfolio
{
    public partial class RenameTab : RadForm
    {
        public string NameTab { get; set; }
        public List<SymbolGroup> UserPortfolios;

        public RenameTab()
        {
            InitializeComponent();

            TranslateText();

            radTextBox1.Focus();
        }

        private void TranslateText()
        {
            Text = Program.LanguageDefault.DictionaryPortfolio["FormRanameTab"];
        }

        private void RadButton1Click(object sender, EventArgs e)
        {
            bool exist = UserPortfolios.Exists(userPortfolio => radTextBox1.Text.ToUpper().Equals(userPortfolio.Name.ToUpper()));

            //if (exist)
            //{
            //    Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryPortfolio["portfolioRenameExist"]);
            //}
            //Validate name:
            /*else*/ if (radTextBox1.Text.Trim().Length<=0)
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryPortfolio["portfolioRenameInvalid"]);
            }
            else //OK
            {
                NameTab = radTextBox1.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void RadTextBox1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                RadButton1Click(null, null);    
            }
            else if (e.KeyCode.Equals(Keys.Escape))
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        

        private void RenameTab_Shown(object sender, EventArgs e)
        {
            radTextBox1.Focus();

        }
    }
}