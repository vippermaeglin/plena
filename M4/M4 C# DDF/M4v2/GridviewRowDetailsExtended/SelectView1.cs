using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using M4.DataServer.Interface;
using M4.DataServer.Interface.ProtocolStructs;
using M4.M4v2.GridviewRowDetailsExtended.GridViewRT;
using Telerik.WinControls.UI.Docking;
using Telerik.WinControls.UI;
using M4Core.Entities;
using System.Drawing;
using M4.M4v2.Portfolio;
using System.Diagnostics;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public partial class SelectView1 : UserControl
    {
        public frmMain2 MFrmMain2;
        public List<string> AllAssets;
        public List<SymbolGroup> UserPortfolios;
        private List<Assets> _allAssets = new List<Assets>();
        public List<GridDataAssetsRT> gridsDataAssets;
        public string DecimalsFormating;

        public string TabSelected
        {
            get { return documentTabStrip1.SelectedTab != null ? documentTabStrip1.SelectedTab.TabStripItem.Text : ""; }
        }

        public int TabSelectedIndex
        {
            get { return documentTabStrip1.SelectedTab != null ? documentTabStrip1.SelectedTab.TabIndex : -1; }
        }

        public SelectView1()
        {
            InitializeComponent();

            DragDropService service = radDock1.GetService<DragDropService>();
            service.Enabled = false;

            ContextMenuService menuService = radDock1.GetService<ContextMenuService>();
            menuService.ContextMenuDisplaying += MenuServiceContextMenuDisplaying;

            documentTabStrip1.TabStripElement.ItemDragMode = PageViewItemDragMode.None;
            documentTabStrip1.OverflowMenuButton.ActionButton.ToolTipText = "Portfólios";

            LoadData();

        }


        public string DecimalFormating()
        {
            int decimals = Properties.Settings.Default.Decimals;
            string formating = "{0:0.";
            for (int i = 0; i < decimals; i++)
            {
                formating += "0";
            }
            formating += "}";
            DecimalsFormating = formating;
            //Update();
            return formating;
        }

        public void LoadData()
        {
            documentTabStrip1.Controls.Clear();

            /*if (File.Exists(ListAssets.Instance().Path))
                _allAssets = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);
              foreach (var assets in _allAssets) AllAssets.Add(assets.Symbol);*/

            AllAssets = new List<string>();
            foreach (Symbol symbol in frmMain2.GetStockListAll())
            {
                AllAssets.Add(symbol.Code);
            }
            /*int position = 0;
            foreach (string asset in AllAssets)
            {
                BarData bar = frmMain.GetLastBarData(asset);
                _allAssets.Add(new Assets()
                {
                    Close = (decimal)bar.ClosePrice,
                    High = (decimal)bar.HighPrice,
                    Last = (decimal)bar.ClosePrice,
                    Low = (decimal)bar.LowPrice,
                    Open = (decimal)bar.OpenPrice,
                    Position = position,
                    Symbol = bar.Symbol,
                    Time = bar.TradeDate.Date.ToString(),
                    Trades = 0,
                    Variation = bar.OpenPrice != 0.0 ? ((decimal)((bar.ClosePrice - bar.OpenPrice) * 100 / bar.OpenPrice)) : (decimal)0.0
                });
                position++;
            }*/


            //LoadGridDataAssetsAll();

            LoadPortfolios();

            documentTabStrip1.Controls.RemoveByKey("documentWindow1");
        }
        /*
        public void LoadGridDataAssetsAll()
        {
            GridDataAssets gridDataAssets = new GridDataAssets(this, AllAssets) { Dock = DockStyle.Fill, DisableOrderAssets = false };

            DocumentWindow tabAll = new DocumentWindow
                                        {
                                            DocumentButtons = DocumentStripButtons.ActiveWindowList,
                                            Name = "All",
                                            Text = Program.LanguageDefault.DictionarySelectTools["tabAll"]
                                        };

            tabAll.Controls.Add(gridDataAssets);

            documentTabStrip1.Controls.Add(tabAll);
        }

        private void ReloadAll()
        {
            if (File.Exists(ListAssets.Instance().Path))
                _allAssets = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);

            AllAssets.Clear();

            foreach (var assets in _allAssets)
                AllAssets.Add(assets.Symbol);

            for (int i = 0; i < documentTabStrip1.Controls.Count; i++)
            {
                if ((!documentTabStrip1.Controls[i].Name.Equals("All")) || (!documentTabStrip1.Controls[i].Name.Equals("TODOS")))
                    continue;

                GridDataAssets gridDataAssets = (GridDataAssets)documentTabStrip1.Controls[i].Controls[0];
                gridDataAssets.DisableOrderAssets = false;
                gridDataAssets.AssetList = AllAssets;
                gridDataAssets.AllAssets = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);

                Invoke((MethodInvoker)(gridDataAssets.LoadTable));
            }
        }
        */
        public void CreatePortfolioTab(/*string portfolio*/)
        {
            try
            {
                UserPortfolios = frmMain2.GetUserPortfolios();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //List<GridDataAssets> dataAssetses = new List<GridDataAssets>();
            List<GridDataAssetsRT> dataAssetses = new List<GridDataAssetsRT>();


            foreach (SymbolGroup p in UserPortfolios)
            {
                bool port = documentTabStrip1.Controls.Cast<DocumentWindow>().Any(control => control.Name.Equals("tab" + p.Name));

                if (port)
                    continue;

                //GridDataAssets dataAssets = new GridDataAssets(this, p.Symbols.ToList()) { Dock = DockStyle.Fill, DisableOrderAssets = true, NamePort = p.Name };
                GridDataAssetsRT dataAssets = new GridDataAssetsRT(this, p.Symbols.Split(new char[] { ',' }).ToList()) { Dock = DockStyle.Fill };

                dataAssetses.Add(dataAssets);

                DocumentWindow tabPage;
                if (p.Name.ToUpper() == "ALL")
                {
                    tabPage = new DocumentWindow
                    {
                        DocumentButtons = DocumentStripButtons.ActiveWindowList,
                        Name = "tabAll",
                        Text = Program.LanguageDefault.DictionarySelectTools["tabAll"]
                    };
                }
                else
                {
                    tabPage = new DocumentWindow
                    {
                        DocumentButtons = DocumentStripButtons.ActiveWindowList,
                        Name = "tab" + p.Name.Trim(),
                        Text = p.Name
                    };
                }
                tabPage.Controls.Add(dataAssets);

                if (InvokeRequired) Invoke(new Action(() => documentTabStrip1.Controls.Add(tabPage)));
                else documentTabStrip1.Controls.Add(tabPage);
            }
        }

        //Load User's Porfolios
        public void LoadPortfolios()
        {
            try
            {
                DocumentWindow tabAll = new DocumentWindow
                                                  {
                                                      DocumentButtons = DocumentStripButtons.ActiveWindowList,
                                                      Name = "tabAll",
                                                      Text = Program.LanguageDefault.DictionarySelectTools["tabAll"]
                                                  };
                documentTabStrip1.Controls.Add(tabAll);
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                        {
                            
                            UserPortfolios = frmMain2.GetUserPortfolios();
                            List<string> portfoliosNames = new List<string>();

                            //gridsDataAssets = new List<GridDataAssets>();
                            gridsDataAssets = new List<GridDataAssetsRT>();

                            foreach (SymbolGroup p in UserPortfolios)
                            {
                                portfoliosNames.Add(p.Name);
                                //GridDataAssets dataAssets = new GridDataAssets(this, p.Symbols.ToList()) { Dock = DockStyle.Fill, DisableOrderAssets = true, NamePort = p.Name };
                                GridDataAssetsRT dataAssets = new GridDataAssetsRT(this, p.Symbols.Split(new char[] { ',' }).ToList()) { Dock = DockStyle.Fill };
                                
                                DocumentWindow tabPage;
                                if (p.Name.ToUpper() == "ALL")
                                {
                                    //tabPage = tabAll;


                                }
                                else
                                {
                                    gridsDataAssets.Add(dataAssets);
                                    tabPage = new DocumentWindow
                                                  {
                                                      DocumentButtons = DocumentStripButtons.ActiveWindowList,
                                                      Name = "tab" + p.Name.Trim(),
                                                      Text = p.Name
                                                  };
                                    documentTabStrip1.Controls.Add(tabPage);
                                    tabPage.Controls.Add(gridsDataAssets.Last());
                                }




                                
                            }
                            GridDataAssetsRT gridDataAll = new GridDataAssetsRT(this, UserPortfolios[0].Symbols.Split(new char[]{','}).ToList()) { Dock = DockStyle.Fill };
                            gridsDataAssets.Insert(0, gridDataAll);
                            tabAll.Controls.Add(gridDataAll);
                        }));



                }
                else

                {
                    UserPortfolios = frmMain2.GetUserPortfolios();
                    List<string> portfoliosNames = new List<string>();

                    //gridsDataAssets = new List<GridDataAssets>();
                    gridsDataAssets = new List<GridDataAssetsRT>();

                    foreach (SymbolGroup p in UserPortfolios)
                    {
                        portfoliosNames.Add(p.Name);

                        Console.WriteLine("\tCREATING GridDataAssetsRT(" + p.Name + ")");
                        //GridDataAssets dataAssets = new GridDataAssets(this, p.Symbols.ToList()) { Dock = DockStyle.Fill, DisableOrderAssets = true, NamePort = p.Name };
                        GridDataAssetsRT dataAssets = new GridDataAssetsRT(this, p.Symbols.Split(new char[] { ',' }).ToList()) { Dock = DockStyle.Fill };

                        DocumentWindow tabPage;
                        if (p.Name.ToUpper() == "ALL")
                        {
                            //tabPage = tabAll;


                        }
                        else
                        {
                            gridsDataAssets.Add(dataAssets);
                            tabPage = new DocumentWindow
                            {
                                DocumentButtons = DocumentStripButtons.ActiveWindowList,
                                Name = "tab" + p.Name.Trim(),
                                Text = p.Name
                            };
                            documentTabStrip1.Controls.Add(tabPage);
                            tabPage.Controls.Add(gridsDataAssets.Last());
                        }





                    }
                    GridDataAssetsRT gridDataAll = new GridDataAssetsRT(this, UserPortfolios[0].Symbols.Split(new char[] { ',' }).ToList()) { Dock = DockStyle.Fill };
                    gridsDataAssets.Insert(0, gridDataAll);
                    tabAll.Controls.Add(gridDataAll);
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        public void SetViewDefault(int index)
        {
           foreach(GridDataAssetsRT grid in gridsDataAssets) 
           {
               grid._selectedViewIndex = index;
           }
        }

        //Adjust control's size to default when necessary:
        public void ResizeControls()
        {
            foreach(DocumentWindow document in documentTabStrip1.Controls)
            {
                if (document.Controls.Count > 0)
                {
                    ((GridDataAssetsRT)document.Controls[0]).Dock = DockStyle.Fill;
                }
            }
        }

        public int GetPortfolioNewIndex()
        {
            return UserPortfolios.Count;
        }

        public void RemovePortfolio(string name)
        {
            foreach (Control control in documentTabStrip1.Controls)
            {
                if (!control.Text.Equals(name))
                    continue;

                if (control.Controls[0] == null)
                    continue;

                documentTabStrip1.Controls.Remove(control);
            }
            frmMain2.RemoveUserPortfolio(name);
            UserPortfolios = frmMain2.GetUserPortfolios();
            frmMain2.SaveUserPortfolios(UserPortfolios);
        }

        //Load User's Porfolios
        public void LoadPortfolios(List<string> updatePortfolios)
        {
            /*Stopwatch timer = new Stopwatch();
            long t1, t2, t3, t4;
            t1 = t2 = t3 = t4 = 0;
            timer.Start();
            t4 = timer.ElapsedMilliseconds - t3 - t2 - t1;
            MessageBox.Show("T1 = " + t1 + "ms\nT2 = " + t2 + "ms\nT3 = " + t3 + "ms\nT4 = " + t4 + "ms\nT5 = "+timer.ElapsedMilliseconds);
             * */
            try
            {
                UserPortfolios = frmMain2.GetUserPortfolios();
                foreach (SymbolGroup p in UserPortfolios)
                {
                    bool port = updatePortfolios.Any(updatePortfolio => updatePortfolio.Equals(p.Name));

                    if (!port)
                        continue;

                    foreach (Control control in documentTabStrip1.Controls)
                    {
                        if (!control.Name.Equals("tab" + p.Name))
                            continue;

                        if (control.Controls.Count == 0)
                            continue;

                        GridDataAssetsRT dataAssets = (GridDataAssetsRT)control.Controls[0];
                        dataAssets.assetList = p.Symbols.Split(new char[]{','}).ToList();
                        dataAssets.ReloadAssets();
                        dataAssets.OrderRefresh();
                        dataAssets.LoadPerformanceTable();
                        //dataAssets.ReloadDetailsTable();
                    }
                }
                frmMain2.SaveUserPortfolios(UserPortfolios);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("\n\nSelectView:LoadPortfolios() " + ex.Message);
            }

        }

        private void Update()
        {
            try
            {
                foreach (Control control in documentTabStrip1.Controls)
                {
                    if (control.Controls.Count == 0)
                        continue;

                    GridDataAssetsRT dataAssets = (GridDataAssetsRT)control.Controls[0];
                    dataAssets.UpdateRefresh();
                    //dataAssets.ReloadDetailsTable();
                }
                frmMain2.SaveUserPortfolios(UserPortfolios);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("\n\nSelectView:Update() "+ex.Message);
            }
        }

        private void ReloadPortfolios()
        {
            List<string> portfoliosNames = new List<string>();
            List<GridDataAssetsRT> dataAssetses = new List<GridDataAssetsRT>();
            UserPortfolios = frmMain2.GetUserPortfolios();
            try
            {
                foreach (SymbolGroup p in UserPortfolios)
                {
                    for (int i = 0; i < documentTabStrip1.Controls.Count; i++)
                    {
                        if (!documentTabStrip1.Controls[i].Name.Equals("tab" + p.Name.Trim()))
                            continue;

                        portfoliosNames.Add(p.Name);

                        GridDataAssetsRT dataManagerExtends = (GridDataAssetsRT)documentTabStrip1.Controls[i].Controls[0];
                        dataManagerExtends.assetList = p.Symbols.Split(new char[] { ',' }).ToList();
                        dataManagerExtends.ReloadDetailsTable();

                        dataAssetses.Add(dataManagerExtends);

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nSelectView:ReloadPortfolios() " + ex.Message);
            }

        }

        public void Reload()
        {
            //ReloadAll();

            ReloadPortfolios();
        }

        public void Clear()
        {
            documentTabStrip1.Controls.Clear();
        }
        /*
        public void UpdateDataManager()
        {
            try
            {
                List<SymbolGroup> newPortfolio = new List<SymbolGroup>();

                foreach (DocumentWindow dw in radDock1.GetWindows<DocumentWindow>())
                {
                    SymbolGroup nPortfolio = new SymbolGroup();

                    if (dw == documentWindow1)
                        continue;

                    nPortfolio.Name = dw.Text;
                    List<string> Symbols = new List<string>();
                    
                    foreach (string s in from DataManagerExtends dm in dw.Controls from s in dm.AssetList select s)
                    {
                        Symbols.Add(s);
                    }
                    nPortfolio.Symbols = Symbols.ToArray();
                    newPortfolio.Add(nPortfolio);
                }

                //ListPortfolios.Instance().UpdatePortfolios(newPortfolio);
                DBSymbolShared.Connect();
                DBSymbolShared.SaveGroups(newPortfolio);
                DBSymbolShared.Disconnect();

                MFrmMain.ReloadSelectTools();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        */
        private void MenuServiceContextMenuDisplaying(object sender, ContextMenuDisplayingEventArgs e)
        {
            if (e.MenuType != ContextMenuType.ActiveWindowList)
                e.Cancel = true;
        }

        private void DocumentTabStrip1DoubleClick(object sender, EventArgs e)
        {
            if ((radDock1.ActiveWindow.Name.ToUpper().Equals("TABALL")) ||
                (radDock1.ActiveWindow.Name.ToUpper().Equals("TABTODOS")) ||
                (radDock1.ActiveWindow.Name.ToUpper().Equals("TABIBRX100")) ||
                (radDock1.ActiveWindow.Name.ToUpper().Equals("TABIBOV")) ||
                (radDock1.ActiveWindow == documentWindow1))
                return;

            RenameTab rename = new RenameTab
                                   {
                                       StartPosition = FormStartPosition.Manual,
                                       Location = PointToScreen(new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y - 60)),
                                       NameTab = radDock1.ActiveWindow.Text,
                                       UserPortfolios = UserPortfolios
                                       
                                   };

            DialogResult dialogResult = rename.ShowDialog();

            if (!dialogResult.Equals(DialogResult.OK))
                return;

            foreach (var userPortfolio in UserPortfolios.Where(userPortfolio => userPortfolio.Name.Equals(radDock1.ActiveWindow.Text)))
            {
                userPortfolio.Name = rename.NameTab;
            }

            //ListPortfolios.Instance().UpdatePortfolios(UserPortfolios);
            frmMain2.SaveUserPortfolios(UserPortfolios);
            UserPortfolios = frmMain2.GetUserPortfolios();
            radDock1.ActiveWindow.Text = rename.NameTab;
            radDock1.ActiveWindow.Name = "tab" + rename.NameTab.Trim();
        }

        private void DocumentTabStrip1SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frmMain2.GInstance.MActiveChart == null)
                return;

            frmMain2.GInstance.MActiveChart.LoadStockPortfolioActive();
        }
    }
}
