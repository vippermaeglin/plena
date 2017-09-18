using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using M4.M4v2.Base;
using Telerik.WinControls.UI.Docking;
using System.IO;
using M4Core.Entities;
using M4Data.List;
using System.Linq;

namespace M4.M4v2.Portfolio
{
    public partial class PortfolioView1 : UserControl
    {
        public frmMain2 MFrmMain;
        private List<DataManager> DataManager { get; set; }
        public List<string> AllAssets;
        public List<string> AllAssetsComplete;
        public List<Portfolios> UserPortfolios;
        private List<Stock> _lista;
        private int _tabCount;
        public DateTime DiaryStart, DiaryEnd;
        public List<string> SymbolsDiary;
        public List<string> DiaryCurrentSymbols;

        public PortfolioView1(frmMain2 main)
        {
            MFrmMain = main;
            InitializeComponent();
            DataManager = new List<DataManager>();
            _tabCount = 0;

            LoadToolTipMenu();
            LoadStock();
            LoadPortfolios();
          //  LoadTradeDiaries();
        }

        public void LoadToolTipMenu()
        {
            int year, month;

            //Trade Diary Commands:
            TxtDiaryStart.Enabled = false;

            if (DateTime.Now.Month > 1)
            {
                month = DateTime.Now.Month - 1;
                year = DateTime.Now.Year;
            }
            else
            {
                month = 12;
                year = DateTime.Now.Year-1;
            }

            DiaryStart = new DateTime(year, month, DateTime.Now.Day > DateTime.DaysInMonth(year, month) ? DateTime.DaysInMonth(year, month) : DateTime.Now.Day);

            TxtDiaryStart.Text = DiaryStart.Day + "/" + DiaryStart.Month + "/" + DiaryStart.Year;

            TxtDiaryEnd.Enabled = false;

            DiaryEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            TxtDiaryEnd.Text = DiaryEnd.Day + "/" + DiaryEnd.Month + "/" + DiaryEnd.Year;
        }

        public void RefreshDdldiarysymbol()
        {
            DdlDiarySymbol.Items.Clear();

            foreach (string s in SymbolsDiary)
            {
                DdlDiarySymbol.Items.Add(s);
            }
        }

        private void LoadStock()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\Symbols.xml";

            List<Stock> list = ListStocks.Instance().LoadStocks(path);
            _lista = list != null ? list.OrderBy(r => r.Name).ToList() : new List<Stock>();

            AllAssetsComplete = new List<string>();

            foreach (var stock in _lista)
            {
                AllAssetsComplete.Add(stock.Code + " - " + stock.Name);
            }
        }

        public void LoadPortfolios()
        {
            //load User's Porfolios
            try
            {
                UserPortfolios = ListPortfolios.Instance().LoadListPortfolios();

                foreach (Portfolios p in UserPortfolios)
                {
                    bool port = UserPortfolios.Any(updatePortfolio => updatePortfolio.Equals(p.Name));

                    if (!port)
                        continue;

                    DataManager dataManager2 = new DataManager(p.Assets) { Dock = DockStyle.Fill, NamePort = p.Name };
                    DataManager.Add(dataManager2);
                    DocumentWindow tabPage = new DocumentWindow
                    {
                        Name = "tab" + p.Name.Trim(),
                        Text = p.Name
                    };
                    _tabCount++;

                    tabPage.Controls.Add(dataManager2);   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadPortifolios(): " + ex.Message);
            }
        }

        public void LoadPortfolios(List<string> updatePortfolios)
        {
            //load User's Porfolios
            try
            {
                UserPortfolios = ListPortfolios.Instance().LoadListPortfolios();

                foreach (Portfolios p in UserPortfolios)
                {
                    bool port = updatePortfolios.Any(r => r.Equals(p.Name));

                    if (!port)
                        continue;

                    DataManager dataManager = DataManager.Find(r => r.NamePort.Equals(p.Name));

                    if (dataManager == null) 
                        continue;

                    dataManager.AssetList = p.Assets;
                    dataManager.LoadAssets(p.Assets);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadPortifolios(): " + ex.Message);
            }
        }

        //private void LoadTradeDiaries()
        //{
        //    Diary = new TradeDiaryView(MFrmMain, this) { Dock = DockStyle.Fill };

        //    documentWindow2.Controls.Add(Diary);

        //    foreach (string s in SymbolsDiary)
        //    {
        //        DdlDiarySymbol.Items.Add(s);
        //    }
        //}

        public void CleanPortfolios()
        {
            try
            {
                while (_tabCount > 0)
                {
                    foreach (DocumentTabStrip dts in radDock1.Controls)
                    {
                        dts.Controls.RemoveByKey(Program.LanguageDefault.DictionaryTabAssets["newTab"] +
                                                  (_tabCount - 1));
                        
                    }

                    _tabCount--;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddNewPortfolio(string name, List<string> assets)
        {
            try
            {
                DataManager dataManager = new DataManager(assets) { Dock = DockStyle.Fill, NamePort = name};

                DataManager.Add(dataManager);

                DocumentWindow tabPage = new DocumentWindow
                {
                    Name = "tab" + name.Trim()
                };

                _tabCount++;

                tabPage.Text = name;
                tabPage.BringToFront();
                tabPage.Controls.Add(dataManager);

                Portfolios newPortfolio = new Portfolios { Assets = AllAssets, Name = name };

                UserPortfolios.Add(newPortfolio);

                ListPortfolios.Instance().UpdatePortfolios(UserPortfolios);

                MFrmMain.ReloadSelectTools();
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddNewPortifolios(): " + ex.Message);
            }
        }
        
        public void DeletePortfolio(string nameRemoved)
        {
            List<Portfolios> newUserPortfolios = new List<Portfolios>();

            List<string> portfolios = new List<string>();

            try
            {
                UserPortfolios = ListPortfolios.Instance().LoadListPortfolios();

                _tabCount = 0;

                foreach (Portfolios p in UserPortfolios)
                {
                    if (p.Name == nameRemoved)
                    {
                        MFrmMain.RemovePortfolio(nameRemoved);
                        continue;
                    }

                    newUserPortfolios.Add(p);
                    portfolios.Add(p.Name);
                    _tabCount++;
                }

                UserPortfolios = newUserPortfolios;

                ListPortfolios.Instance().UpdatePortfolios(UserPortfolios);

                LoadPortfolios();

                MFrmMain.ReloadSelectTools(portfolios);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DeletePortifolio(): " +ex.Message);
            }
        }

        //private void BtnDiaryStartClick(object sender, EventArgs e)
        //{
        //    int x = ((MouseEventArgs)e).X;
        //    int y = ((MouseEventArgs)e).Y;

        //    DateChoice _choice = new DateChoice(this, "start", DiaryStart)
        //    {
        //        StartPosition = FormStartPosition.Manual,
        //        Location = PointToScreen(new Point(x, y)),
        //        IsClick = true
        //    };
        //    _choice.ShowDialog();

        //    TxtDiaryStart.Text = DiaryStart.Day + "/" + DiaryStart.Month + "/" + DiaryStart.Year;
        //    Diary.FilterDiaries(DiaryCurrentSymbols, DiaryStart, DiaryEnd);
        //}

        //private void BtnDiaryEndClick(object sender, EventArgs e)
        //{
        //    DateChoice choice = new DateChoice(this, "end", DiaryStart)
        //    {
        //        StartPosition = FormStartPosition.Manual,
        //        Location = PointToScreen(new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y)),
        //        IsClick = true
        //    };

        //    choice.ShowDialog();

        //    TxtDiaryEnd.Text = DiaryEnd.Day + "/" + DiaryEnd.Month + "/" + DiaryEnd.Year;
        //    Diary.FilterDiaries(DiaryCurrentSymbols, DiaryStart, DiaryEnd);
        //}

        //private void BtnDiarySymbolClick(object sender, EventArgs e)
        //{
        //    EditDiarySymbols edit = new EditDiarySymbols(this);
        //    edit.ShowDialog();
        //}

        //private void DdlDiarySymbolSelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        //{
        //    if (DdlDiarySymbol.SelectedItem == null)
        //        return;

        //    DiaryCurrentSymbols = new List<string> {DdlDiarySymbol.SelectedItem.Text.Trim()};
        //    Diary.FilterDiaries(DiaryCurrentSymbols, DiaryStart, DiaryEnd);
        //}

        private void PortfolioView1Resize(object sender, EventArgs e)
        {
            commandBarStripElement2.Size = new Size(Width, commandBarStripElement2.Size.Height);
        }
    }
}
