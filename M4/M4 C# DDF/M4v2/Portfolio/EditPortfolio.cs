using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using M4.DataServer.Interface;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using M4.M4v2.GridviewRowDetailsExtended;
using M4Data.MessageService;
using M4Core.Entities;
using System.Diagnostics;
using M4.DataServer.Interface.ProtocolStructs;
using System.Data.SqlClient;

namespace M4.M4v2.Portfolio
{
    public partial class EditPortfolio : RadForm
    {
        private readonly SelectView1 ParentControl;
        private List<string> _assetsSelected;
        public string TemporaryPortName;
        public List<string> TemporaryPortAssets;
        public string PreviewPortfolio;
        public List<string> AssetsLista;
        public bool Changes;
        private readonly string _tabSelected;

        public EditPortfolio(SelectView1 parentcontrol, string tabSelected)
        {
            InitializeComponent();
            ParentControl = parentcontrol;

            AssetsLista = ParentControl.AllAssets;
            LoadStock();
            PreviewPortfolio = "";
            Changes = false;

            _tabSelected = tabSelected;
            LoadPortfolioSelection();

            const float fontSize = (float)8.25;
            ddlPortfolios.DropDownListElement.ListElement.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.DropDownListElement.Popup.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.Popup.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.ListElement.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.Items.OwnerListElement.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.Items.First.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
            ddlPortfolios.Items.Last.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);

            btnAddPort.ToolTipTextNeeded += BtnAddPortToolTipTextNeeded;
            btnDeletePort.ToolTipTextNeeded += BtnDeletePortToolTipTextNeeded;
            btnAddPortList.ToolTipTextNeeded += BtnAddPortListToolTipTextNeeded;
            btnRemovePortList.ToolTipTextNeeded += BtnRemovePortListToolTipTextNeeded;

            LoadDictionary();

            radListControl1.ListElement.VScrollBar.MinSize = new Size(16, radListControl2.Height);
            radListControl2.ListElement.VScrollBar.MinSize = new Size(16, radListControl2.Height);
        }

        public void EditPortfolio_Shown(object sender, EventArgs e)
        {
            ddlFilter.Focus();
        }

        private void LoadPortfolioSelection()
        {
            IList<RadListDataItem> listDataItems = ParentControl.UserPortfolios.Select(p => new RadListDataItem(p.Name)).OrderBy(r => r.Text).ToList();
            listDataItems.Single(item => item.Text.ToUpper() == "ALL").Text =
                Program.LanguageDefault.DictionaryTabAssets["tabAll"];
            listDataItems.OrderBy(r => r.Text);
            ddlPortfolios.DataSource = listDataItems;

            if (String.IsNullOrEmpty(_tabSelected))
            {
                ddlPortfolios.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < ddlPortfolios.Items.Count; i++)
            {
                if (ddlPortfolios.Items[i].Text == _tabSelected)
                    ddlPortfolios.SelectedIndex = i;
            }
        }

        private void BtnAddPortToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = Program.LanguageDefault.DictionaryPortfolio["toolTipAddPortfolio"];
        }

        private void BtnDeletePortToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = Program.LanguageDefault.DictionaryPortfolio["toolTipDeletePortfolio"];
        }

        private void BtnAddPortListToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = Program.LanguageDefault.DictionaryPortfolio["toolTipAddPortList"];
        }

        private void BtnRemovePortListToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = Program.LanguageDefault.DictionaryPortfolio["toolTipRemovePortList"];
        }

        private void LoadDictionary()
        {
            Text = Program.LanguageDefault.DictionaryPortfolio["frmEditPortfolio"];
            btnOk.Text = Program.LanguageDefault.DictionaryPortfolio["btnOk"];
            btnApply.Text = Program.LanguageDefault.DictionaryPortfolio["btnApply"];
            btnCancel.Text = Program.LanguageDefault.DictionaryPortfolio["btnCancel"];
            lblSearchPort.Text = Program.LanguageDefault.DictionaryPortfolio["lblSearchPort"];
        }

        private void LoadStock()
        {
            List<RadListDataItem> list = AssetsLista.Select(stock => new RadListDataItem(stock.ToUpper(), stock)).ToList();
            radListControl1.DataSource = list;
            ddlFilter.DataSource = list;
        }

        private void RadButton1Click(object sender, EventArgs e)
        {
            List<string> itemsBlocked =new List<string>();
            if (Program.LoginAuthentication.Login.ToUpper().Equals("GUEST"))
            {
                foreach (RadListDataItem item in radListControl1.SelectedItems)
                {
                    if (!ParentControl.UserPortfolios[0].Symbols.Contains(item.Text))
                    {
                        RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["msgSymbolGuest"]+item.Text);
                        itemsBlocked.Add(item.Text);
                    }
                }
            }


            foreach (RadListDataItem item in radListControl1.SelectedItems.Where(item => radListControl2.Items.Where(r => r.Text.Equals(item.Text)).SingleOrDefault() == null))
            {
                if (!itemsBlocked.Contains(item.Text))
                {
                    radListControl2.Items.Add(new RadListDataItem(item.Text));
                    Changes = true;
                }
            }
        }

        private void RadButton2Click(object sender, EventArgs e)
        {
            Changes = true;

            IList<RadListDataItem> listDataItems = radListControl2.SelectedItems.ToList();

            foreach (RadListDataItem item in listDataItems)
            {
                radListControl2.Items.Remove(item);
            }
        }

        private void ddlPortfolios_TextChanged(object sender, EventArgs e)
        {
            if (!ddlPortfolios.Items.Any(port => port.Text.Contains(ddlPortfolios.Text)))
            {
                Telerik.WinControls.RadMessageBox.Show(

                    "Portfolio \"" + ddlPortfolios.Text + " \" " +
                    Program.LanguageDefault.DictionaryPortfolio["PortfolioExists"], "", MessageBoxButtons.OK,
                    Telerik.WinControls.RadMessageIcon.Exclamation);

                if (PreviewPortfolio != "")
                {
                    ddlPortfolios.TextChanged -= ddlPortfolios_TextChanged;
                    ddlPortfolios.SelectedItem = ddlPortfolios.Items.Single(port => port.Text == PreviewPortfolio);
                    ddlPortfolios.Text = PreviewPortfolio;
                    ddlPortfolios.SelectAllText();
                    ddlPortfolios.TextChanged += ddlPortfolios_TextChanged;
                }
                else
                {
                    ddlPortfolios.TextChanged -= ddlPortfolios_TextChanged;
                    ddlPortfolios.SelectedItem = ddlPortfolios.Items.First();
                    ddlPortfolios.Text = ddlPortfolios.SelectedItem.Text;
                    ddlPortfolios.SelectAllText();
                    ddlPortfolios.TextChanged += ddlPortfolios_TextChanged;
                }

            }
            else
            {
                ddlPortfolios.TextChanged -= ddlPortfolios_TextChanged;
                ddlPortfolios.SelectedItem = ddlPortfolios.Items.First(port=> port.Text.Contains(ddlPortfolios.Text));
                ddlPortfolios.TextChanged += ddlPortfolios_TextChanged;
            }
        }
        private void DdlPortfoliosSelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (Changes && (PreviewPortfolio != ""))
            {
                DialogResult dialogResult = Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryPortfolio["ConfirmSavePortfolio"] +
                     " \"" + PreviewPortfolio + "\" ?", "", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question);

                if (dialogResult.Equals(DialogResult.OK))
                    SaveChanges();
            }

            Changes = false;
            PreviewPortfolio = ddlPortfolios.Text;
            _assetsSelected = new List<string>();
            radListControl2.Items.Clear();
            string portName = ddlPortfolios.Text;
            if (portName == Program.LanguageDefault.DictionaryTabAssets["tabAll"]) portName = "All";
            foreach (SymbolGroup p in ParentControl.UserPortfolios)
            {
                if (p.Name == portName)
                {
                    _assetsSelected = p.Symbols.Split(new char[] { ',' }).ToList();

                }
            }

            List<RadListDataItem> listDataItems = new List<RadListDataItem>();

            foreach (var stock in _assetsSelected)
            {
                if (stock == "") continue;
                string[] splitCode = stock.Split(new[] { '-' });

                if (PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]))
                {
                    listDataItems.Add(new RadListDataItem(stock.ToUpper()));
                    continue;
                }
                else
                {
                    listDataItems.AddRange(_assetsSelected.Where(s => splitCode[0].Trim() == s).Select(s => new RadListDataItem(stock.ToUpper())));
                }
            }

            btnAddPortList.Enabled = !PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]) && !PreviewPortfolio.Equals("IBOV") && !PreviewPortfolio.Equals("IBRX100");
            btnRemovePortList.Enabled = !PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]) && !PreviewPortfolio.Equals("IBOV") && !PreviewPortfolio.Equals("IBRX100");
            btnApply.Enabled = !PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]) && !PreviewPortfolio.Equals("IBOV") && !PreviewPortfolio.Equals("IBRX100");
            btnOk.Enabled = !PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]) && !PreviewPortfolio.Equals("IBOV") && !PreviewPortfolio.Equals("IBRX100");
            btnDeletePort.Enabled = !PreviewPortfolio.Equals(Program.LanguageDefault.DictionaryTabAssets["tabAll"]) && !PreviewPortfolio.Equals("IBOV") && !PreviewPortfolio.Equals("IBRX100");

            radListControl2.DataSource = listDataItems;
            radListControl2.SelectedItem = null;
            foreach (RadListDataItem r in radListControl2.Items)
            {
                if (r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper())
                    radListControl2.SelectedItem = r;
            }
            foreach (RadListDataItem r in radListControl1.Items.Where(r => r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper()))
            {
                radListControl1.SelectedItem = r;
            }
        }

        private void DdlFilterSelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (ddlFilter.SelectedItem == null)
                return;

            radListControl2.SelectedItem = null;
            foreach (RadListDataItem r in radListControl1.Items.Where(r => r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper()))
            {
                radListControl1.SelectedItem = r;
            }

            foreach (RadListDataItem r in radListControl2.Items)
            {
                if (r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper())
                    radListControl2.SelectedItem = r;
            }
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            if (!Changes)
            {
                return;
            }
            if (ddlPortfolios.SelectedIndex != -1)
            {
                SaveChanges();
            }
            CreateNewPortfolio();
            if(frmMain2.GInstance.MActiveChart != null)
                frmMain2.GInstance.MActiveChart.LoadStockPortfolioActive();
            Focus();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            if (Changes)
            {
                DialogResult dialogResult = RadMessageBox.Show(this,Program.LanguageDefault.DictionaryPortfolio["ConfirmSavePortfolio"] +
                     " \"" + ddlPortfolios.Text + "\" ?", "", MessageBoxButtons.OKCancel, RadMessageIcon.Question);

                if (dialogResult.Equals(DialogResult.OK))
                    SaveChanges();
                Changes = false;
            }

            CreateNewPortfolio();

            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if(!Changes)
            {
                Close();
                return;
            }
            SaveChanges();
            CreateNewPortfolio();
            if(frmMain2.GInstance.MActiveChart != null)
                frmMain2.GInstance.MActiveChart.LoadStockPortfolioActive();

            Close();
        }

        public void CreateNewPortfolio()
        {
      //      foreach (var item in ddlPortfolios.Items)
     //       {
                ParentControl.MFrmMain2.CreatePortfolioTab();
      //      }
            ParentControl.UserPortfolios = frmMain2.GetUserPortfolios();
        }

        public void SaveChanges()
        {
            List<string> updatePortfolios = new List<string>();

            SymbolGroup portChanged;

            if (ddlPortfolios.SelectedItem != null)
            {
                //Create a new portfolio
                portChanged = new SymbolGroup()
                {
                    Name = ddlPortfolios.SelectedItem.Text == Program.LanguageDefault.DictionaryTabAssets["tabAll"] ? "All" : ddlPortfolios.SelectedItem.Text,
                    Type = (int)DataServer.Interface.GroupType.Portfolio,
                    Index = ParentControl.GetPortfolioNewIndex()
                };
                List<string> Symbols = new List<string>();
                foreach (var r in radListControl2.Items)
                {
                    string[] codeParse = r.Text.Split(new[] { ' ' });
                    string newAsset = codeParse[0];
                    Symbols.Add(newAsset);
                }
                portChanged.Symbols = string.Join(",", Symbols.ToArray());
                //Replace it on User's Portfolios
                List<string> SymbolsNew = new List<string>();

                foreach (var p in ParentControl.UserPortfolios)
                {
                    if (p.Name != portChanged.Name)
                        continue;

                    // if (UnorderedEqual(p.Symbols, portChanged.Symbols))
                    //  continue;
                    SymbolsNew = p.Symbols.Split(new char[] { ',' }).ToList();
                    //Remove os simbolos removidos
                    if (!UnorderedEqual(SymbolsNew, Symbols.ToArray()))
                    {
                        for (int i = 0; i < SymbolsNew.Count; i++)
                        {
                            if (!Symbols.Any(s => s == SymbolsNew[i]))
                            {
                                //Update portfolio "All":
                                bool remove = true;
                                // This symbol exists in another portfolio?
                                foreach (SymbolGroup port in ParentControl.UserPortfolios.GetRange(1,ParentControl.UserPortfolios.Count()-1)) 
                                {
                                    if (port.Name == portChanged.Name) continue;
                                    if (port.Symbols.Split(new char[] { ',' }).ToList().Exists(s => s == SymbolsNew[i])) remove = false;
                                }
                                if (remove)
                                {
                                    List<string> userSymbols = ParentControl.UserPortfolios[0].Symbols.Split(new char[] { ',' }).ToList();
                                    if (userSymbols.Remove(SymbolsNew[i])) ParentControl.UserPortfolios[0].Symbols = string.Join(",", userSymbols.ToArray());
                                    ParentControl.UserPortfolios[0].Symbols = string.Join(",", userSymbols.ToArray());
                                    List<Symbol> updateSync = new List<Symbol>();
                                    foreach (string s in userSymbols) updateSync.Add(new Symbol() {Code = s } );
                                    SqlConnection _con = DBlocalSQL.Connect();
                                    DBlocalSQL.SaveSymbols(updateSync, _con, true);
                                    DBlocalSQL.Disconnect(_con);
                                    if(!updatePortfolios.Exists(s=>s=="All"))updatePortfolios.Insert(0, "All");
                                }

                                SymbolsNew.Remove(SymbolsNew[i]);
                                i--;
                            }
                        }


                    }
                    List<string> symbolsRequest = new List<string>();
                    //insere os simbolos inseridos
                    foreach (string symbol in Symbols)
                    {
                        if (!SymbolsNew.Any(s => s == symbol))
                        {

                            SymbolsNew.Add(symbol);
                            //Update portfolio "All":
                            if (!ParentControl.UserPortfolios[0].Symbols.Split(new char[] { ',' }).ToList().Exists(s => s == symbol))
                            {
                                ParentControl.UserPortfolios[0].Symbols += "," + symbol;

                                //Add request to list:
                                symbolsRequest.Add(symbol);

                                List<Symbol> updateSync = new List<Symbol>();
                                foreach (string s in SymbolsNew) updateSync.Add(new Symbol() { Code = s } );
                                SqlConnection _con = DBlocalSQL.Connect();
                                DBlocalSQL.SaveSymbols(updateSync, _con, true);
                                DBlocalSQL.Disconnect(_con);
                                if (!updatePortfolios.Exists(s => s == "All")) updatePortfolios.Insert(0, "All");

                            }
                        }
                    }
                    if (symbolsRequest.Count()>0)
                    {
                        //Sync new symbol with HUB:
                        MessageService.SubmitRequest(new MSRequest("INSERT SYMBOL m" + ParentControl.MFrmMain2._messageRequestID, MSRequestStatus.Pending,
                                                    MSRequestType.GetHistoricalData,
                                                    MSRequestOwner.M4,
                                                    new object[] { symbolsRequest, (int)BaseType.Days }));
                        ParentControl.MFrmMain2.AddRequestedOperation(new Operations("m" + ParentControl.MFrmMain2._messageRequestID, M4Core.Enums.TypeOperations.LoadSelectView,
                                                                    new object[] { }));
                    }

                    updatePortfolios.Add(p.Name);

                    p.Symbols = string.Join(",", SymbolsNew.ToArray());
                    break;
                }

                if (updatePortfolios.Count > 0)
                {
                    frmMain2.SaveUserPortfolios(ParentControl.UserPortfolios.GetRange(1,ParentControl.UserPortfolios.Count()-1));
                    frmMain2.ReloadStockListPortfolios();
                    //Update PortfolioView1
                    ParentControl.LoadPortfolios(updatePortfolios);
                }
            }
            Changes = false;
        }

       

        private void BtnAddPortClick(object sender, EventArgs e)
        {
            ddlPortfolios.TextChanged -= ddlPortfolios_TextChanged;
            NewPortfolio newPortfolio = new NewPortfolio(){ParentControl = ParentControl};
            DialogResult dialogResult = newPortfolio.ShowDialog();

            if (dialogResult == DialogResult.Abort)
                return;

            SymbolGroup portfolios = new SymbolGroup() { Name = newPortfolio.PortFolio, Index = ParentControl.GetPortfolioNewIndex(), Type = (int)DataServer.Interface.GroupType.Portfolio, Symbols = ""};
            ParentControl.UserPortfolios.Add(portfolios);

            frmMain2.SaveUserPortfolios(ParentControl.UserPortfolios);
            //ListPortfolios.Instance().UpdatePortfolios(ParentControl.UserPortfolios);

            ddlPortfolios.Items.Clear();
            IList<RadListDataItem> listDataItems = ParentControl.UserPortfolios.Select(p => new RadListDataItem(p.Name)).OrderBy(r => r.Text).ToList();
            //listDataItems.Insert(0, new RadListDataItem(Program.LanguageDefault.DictionaryTabAssets["tabAll"]));
            listDataItems[0].Text = Program.LanguageDefault.DictionaryTabAssets["tabAll"];
            ddlPortfolios.DataSource = listDataItems;

            ddlPortfolios.SelectedIndex = ddlPortfolios.Items.IndexOf(newPortfolio.PortFolio);
            ddlPortfolios.TextChanged += ddlPortfolios_TextChanged;
        }

        private void RadButton3Click(object sender, EventArgs e)
        {
            bool allRemoved = false;
            if (ddlPortfolios.SelectedItem != null)
            {
                DialogResult dialogResult = Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryPortfolio["ConfirmDeletePortfolio"],
                    "", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    //Update portfolio "All":
                    bool remove = true;
                    List<string> SymbolsNew = ParentControl.UserPortfolios.First(p => p.Name == ddlPortfolios.Text).Symbols.Split(new char[] { ',' }).ToList();
                    for (int i = 0; i < SymbolsNew.Count(); i++)
                    {
                        // This symbol exists in another portfolio?
                        foreach (SymbolGroup port in ParentControl.UserPortfolios.GetRange(1, ParentControl.UserPortfolios.Count() - 1))
                        {
                            if (port.Name == ddlPortfolios.Text) continue;
                            if (port.Symbols.Split(new char[] { ',' }).ToList().Exists(s => s == SymbolsNew[i])) remove = false;
                        }
                        if (remove)
                        {
                            List<string> userSymbols = ParentControl.UserPortfolios[0].Symbols.Split(new char[] { ',' }).ToList();
                            if (userSymbols.Remove(SymbolsNew[i])) ParentControl.UserPortfolios[0].Symbols = string.Join(",", userSymbols.ToArray());
                            ParentControl.UserPortfolios[0].Symbols = string.Join(",", userSymbols.ToArray());
                            List<Symbol> updateSync = new List<Symbol>();
                            foreach (string s in userSymbols) updateSync.Add(new Symbol() { Code = s });
                            SqlConnection _con = DBlocalSQL.Connect();
                            DBlocalSQL.SaveSymbols(updateSync, _con, true);
                            DBlocalSQL.Disconnect(_con);
                            allRemoved = true;

                        }
                    }
                    ParentControl.RemovePortfolio(ddlPortfolios.Text);
                    if (allRemoved)
                    {
                        frmMain2.ReloadStockListPortfolios();
                        //Update PortfolioView1
                        ParentControl.LoadPortfolios(new List<string>() { "All" });
                    }
                    LoadPortfolioSelection();
                }
            }

            PreviewPortfolio = "";

            foreach (RadListDataItem r in radListControl1.Items.Where(r => r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper()))
            {
                radListControl1.SelectedItem = r;
            }

            foreach (RadListDataItem r in radListControl2.Items)
            {
                if (r.Text.ToUpper() == ddlFilter.SelectedItem.Text.ToUpper())
                    radListControl2.SelectedItem = r;
            }
        }

        private void EditPortfolioKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;

            Close();
        }

        private static bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            // 1
            // Require that the counts are equal
            if (a.Count != b.Count)
            {
                return false;
            }
            // 2
            // Initialize new Dictionary of the type
            Dictionary<T, int> d = new Dictionary<T, int>();
            // 3
            // Add each key's frequency from collection A to the Dictionary
            foreach (T item in a)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }
            // 4
            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach (T item in b)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }
            // 5
            // Verify that all frequencies are zero
            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }
            // 6
            // We know the collections are equal
            return true;
        }

        private void EditPortfolio_Load(object sender, EventArgs e)
        {
            radListControl1.ListElement.BackColor = Color.White;
            radListControl2.ListElement.BackColor = Color.White;
            BackColor = Utils.GetDefaultBackColor();
            FormElement.BackColor = Utils.GetDefaultBackColor();
        }

        private void radListControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPortfolios.Text != Program.LanguageDefault.DictionaryTabAssets["tabAll"] || radListControl2.SelectedItem==null) return;

            bool fixedIndex = false;
            foreach (SymbolGroup p in ParentControl.UserPortfolios.GetRange(1,ParentControl.UserPortfolios.Count()-1))
            {
                //if (p.Type != (int)M4.DataServer.Interface.GroupType.Index) continue; //Restrict to indexes
                foreach (string s in p.Symbols.Split(new char[] { ',' }).ToList())
                {
                    foreach (RadListDataItem item in radListControl2.SelectedItems)
                    {
                        if (s == item.Text)
                        {
                            fixedIndex = true;
                            break;
                        }
                    }
                }
                if (fixedIndex) break;
            }
            if (fixedIndex)
            {
                btnAddPortList.Enabled = false;
                btnRemovePortList.Enabled = false;
            }
            else
            {
                btnAddPortList.Enabled = true;
                btnRemovePortList.Enabled = true;
            }
            
        }


        private void radListControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPortfolios.Text != Program.LanguageDefault.DictionaryTabAssets["tabAll"]) return;

            bool fixedIndex = false;
            foreach (SymbolGroup p in ParentControl.UserPortfolios.GetRange(1, ParentControl.UserPortfolios.Count() - 1))
            {
                //if (p.Type != (int)M4.DataServer.Interface.GroupType.Index) continue; //Restrict to indexes
                foreach (string s in p.Symbols.Split(new char[] { ',' }).ToList())
                {
                    if (s == radListControl1.SelectedItem.Text)
                    {
                        fixedIndex = true;
                        break;
                    }
                }
                if (fixedIndex) break;
            }
            if (fixedIndex)
            {
                btnAddPortList.Enabled = false;
                btnRemovePortList.Enabled = false;
            }
            else
            {
                btnAddPortList.Enabled = true;
                btnRemovePortList.Enabled = true;
            }

        }


    }
}


