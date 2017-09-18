using System;
using System.Collections.Generic;
using System.Windows.Forms;
using M4.M4v2.Chart;
using M4.Properties;
using M4Utils.Language;
using Nevron.UI.WinForm.Controls;

namespace M4.M4v2.FormDock
{
    public partial class Data : UserControl
    {
        public frmMain MFrmMain;
        public List<DataManager> DataManager { get; set; }
        public List<NTabPage> NTabPages { get; set; }

        public Data()
        {
            InitializeComponent();

            DataManager = new List<DataManager>();
            NTabPages = new List<NTabPage>();

            LoadToolTipMenu();
            LoadDataManager();
        }

        private void LoadToolTipMenu()
        {
            btnNewWallet.ToolTipText = Program.LanguageDefault.DictionaryMenuAssets["btnNewWallet"];
            btnDeleteWallet.ToolTipText = Program.LanguageDefault.DictionaryMenuAssets["btnDeleteWallet"];
        }

        public void LoadDataManager()
        {
            DataManager dataManager = new DataManager { Dock = DockStyle.Fill };
            DataManager.Add(dataManager);

            NTabPage tabAll = new NTabPage
                                  {
                                      Location = new System.Drawing.Point(4, 4),
                                      Name = "tabAll",
                                      Size = new System.Drawing.Size(Width, Height),
                                      TabIndex = 1,
                                      Text = Program.LanguageDefault.DictionaryTabAssets["tabAll"]
                                  };
            tabAll.Controls.Add(dataManager);
            nTabControl.Controls.Add(tabAll);
        }

        private void BtnNewWalletClick(object sender, EventArgs e)
        {
            DataManager dataManager = new DataManager { Dock = DockStyle.Fill };
            DataManager.Add(dataManager);

            NTabPage nTabPage = new NTabPage
                                  {
                                      Location = new System.Drawing.Point(4, 4),
                                      Name = "newTab",
                                      Size = new System.Drawing.Size(Width, Height),
                                      TabIndex = 1,
                                      Text = Program.LanguageDefault.DictionaryTabAssets["newTab"] + " " + nTabControl.TabPages.Count
                                  };

            nTabPage.Controls.Add(dataManager);
            nTabControl.Controls.Add(nTabPage);
            nTabControl.SelectedIndex = nTabControl.TabPages.Count - 1;
        }

        private void BtnDeleteWalletClick(object sender, EventArgs e)
        {
            int tabSelected = nTabControl.SelectedIndex;
            nTabControl.Controls.Remove(nTabControl.SelectedTab);
            if (tabSelected > 0)
                nTabControl.SelectedIndex = tabSelected - 1;
        }

        private void NTabControlSelectedTabChanged(object sender, EventArgs e)
        {
            txtNameTab.Text = nTabControl.SelectedTab.Text;
        }

        private void TxtNameTabTextChanging(object sender, Telerik.WinControls.TextChangingEventArgs e)
        {
            nTabControl.SelectedTab.Text = txtNameTab.Text;
        }

        private void BtnGraphicsClick(object sender, EventArgs e)
        {
            ChartSelection selection = (new FrmSelectChart()).GetChartSelection();

            if (selection == null)
                return;

            MFrmMain.CreateNewCtlPainel(selection);
            //MFrmMain.CreateNewChart(selection);
        }
    }
}
