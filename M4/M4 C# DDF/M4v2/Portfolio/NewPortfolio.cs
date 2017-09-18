using System;
using System.Collections.Generic;
using System.Windows.Forms;
using M4.DataServer.Interface;
using M4.M4v2.GridviewRowDetailsExtended;
using System.Data.SqlClient;

namespace M4.M4v2.Portfolio
{
    public partial class NewPortfolio : Telerik.WinControls.UI.RadForm
    {
        public string PortFolio { get; set; }

        public SelectView1 ParentControl;

        public List<string> UpdatePortfolios { get; set; }

        public NewPortfolio()
        {
            InitializeComponent();

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            lblPortfolio.Text = Program.LanguageDefault.DictionaryPortfolio["lblPortfolio"];
            Text = Program.LanguageDefault.DictionaryPortfolio["FormPortfolio"];
            btnOk.Text = Program.LanguageDefault.DictionaryPortfolio["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionaryPortfolio["btnCancel"];
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            try
            {
                SaveChanges();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        public void SaveChanges()
        {
            if (String.IsNullOrEmpty(txtPortfolio.Text))
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryPortfolio["fieldEmptyPortfolio"]);
                return;
            }

            SymbolGroup portChanged = new SymbolGroup()
            {
                Name = txtPortfolio.Text,
                Type = (int)GroupType.Portfolio,
                Index = ParentControl.GetPortfolioNewIndex(),
                Symbols = ""
            };

            UpdatePortfolios = new List<string> { txtPortfolio.Text };

            SqlConnection _connection = DBlocalSQL.Connect();

            List<SymbolGroup> listpoPortfolios = DBlocalSQL.LoadGroups(GroupType.Index,_connection);
            listpoPortfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Portfolio,_connection));
            DBlocalSQL.Disconnect(_connection);

            foreach (var listpoPortfolio in listpoPortfolios)
            {
                if (listpoPortfolio.Name.ToUpper() == portChanged.Name.ToUpper() || Program.LanguageDefault.DictionaryTabAssets["tabAll"] == portChanged.Name.ToUpper())
                {
                    Telerik.WinControls.RadMessageBox.Show(String.Format(Program.LanguageDefault.DictionaryPortfolio["portfolioExisted"], txtPortfolio.Text));
                    return;
                }

                UpdatePortfolios.Add(listpoPortfolio.Name);
            }

            listpoPortfolios.Add(portChanged);

            SqlConnection _connection2 = DBlocalSQL.Connect();
            DBlocalSQL.SaveGroups(listpoPortfolios,_connection2);
            DBlocalSQL.Disconnect(_connection2);

            PortFolio = txtPortfolio.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void NewPortfolio_Load(object sender, EventArgs e)
        {
            txtPortfolio.Select();
        }
    }
}
