using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using M4;
using M4.M4v2.Portfolio;
using M4.M4v2.TradeDiary.InsertTableMenuItem;
using M4.Properties;
using Telerik.Charting.Styles;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.RichTextBox;
using Telerik.WinControls.RichTextBox.FileFormats.Html;
using Telerik.WinControls.RichTextBox.FileFormats.OpenXml.Docx;
using Telerik.WinControls.RichTextBox.FileFormats.Pdf;
using Telerik.WinControls.RichTextBox.FileFormats.Rtf;
using Telerik.WinControls.RichTextBox.FileFormats.Xaml;
using Telerik.WinControls.RichTextBox.FormatProviders;
using Telerik.WinControls.RichTextBox.FormatProviders.Txt;
using Telerik.WinControls.RichTextBox.Layout;
using Telerik.WinControls.RichTextBox.Lists;
using Telerik.WinControls.RichTextBox.Model;
using Telerik.WinControls.RichTextBox.Model.Styles;
using Telerik.WinControls.RichTextBox.TextSearch;
using Telerik.WinControls.RichTextBox.UI;
using M4.M4v2.TradeDiary;
using Telerik.WinControls.UI;
using Unit = Telerik.WinControls.RichTextBox.Model.Unit;

//using TelerikEditor.Properties;

namespace M4.M4v2.TradeDiary
{
    public partial class TradeDiaryView : UserControl
    {
        private bool stylesInitializing = false;
        private bool firstShow = true;
        private bool singleInstance;
        private System.Windows.Forms.Timer closeTimer;
        public frmMain2 MFrmMain;
        public PortfolioView1 ParentControl;
        private string path = Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\TRADEDIARY.html";
        public List<string> URL_Open;
        private RadDocument document;
        private DateTime New_Date;

        public TradeDiaryView(frmMain2 MAIN, PortfolioView1 PARENT)
        {

            MFrmMain = MAIN;
            ParentControl = PARENT;
            Initialize();
        }

        public TradeDiaryView(bool singleInstance)
        {
            // TODO: Complete member initialization
            this.singleInstance = singleInstance;

            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();
            URL_Open = new List<string>();
            RadMenuInsertTableItem insertTableBoxItem = new RadMenuInsertTableItem();
            insertTableBoxItem.SelectionChanged += new EventHandler<TableSelectionChangedEventArgs>(OnInsertTableSelectionChanged);
            radRichTextBox1.IsReadOnly = true;
            LoadDiaries();
            FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart, ParentControl.DiaryEnd);

            //this.radRichTextBox1.HyperlinkClicked += new System.EventHandler<Telerik.WinControls.RichTextBox.Model.HyperlinkClickedEventArgs>(this.HyperlinkClicked);
            this.radRichTextBox1.HyperlinkNavigationMode = Telerik.WinControls.RichTextBox.HyperlinkNavigationMode.Click;



        }

        private void LoadDiaries()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY");
            FileInfo[] rgFiles = di.GetFiles();
            string name_file;
            string[] name_file_split;
            string symbol;
            ParentControl.SymbolsDiary = new List<string>();
            ParentControl.DiaryCurrentSymbols = new List<string>();
            bool symbol_inserted;
            foreach (FileInfo fi in rgFiles)
            {
                name_file = fi.Name;
                name_file_split = name_file.Split(new char[] { '_' });
                if (name_file_split.Length == 7)
                {
                    symbol = name_file_split[0].Trim();
                    symbol_inserted = false;
                    foreach (string s in ParentControl.SymbolsDiary)
                    {
                        if (s == symbol) symbol_inserted = true;
                    }
                    if (!symbol_inserted)
                    {
                        ParentControl.SymbolsDiary.Add(symbol);
                        ParentControl.DiaryCurrentSymbols.Add(symbol);
                    }
                }
            }
        }

        public void FilterDiaries(List<string> FSymbols,DateTime FBegin, DateTime FEnd)
        {
            try
            {
                string name_file;
                string[] name_file_split;
                string date = "";
                string time = "";
                string symbol;
                string html_string;


                DateTime date_time;
                StreamWriter html_writer =
                    new StreamWriter(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\TRADEDIARY.html");
                StreamReader html_reader;
                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY");
                FileInfo[] rgFiles = di.GetFiles("*.html");
                ParentControl.DiaryCurrentSymbols = FSymbols;
                foreach (FileInfo fi in rgFiles)
                {
                    name_file = fi.Name;
                    name_file_split = name_file.Split(new char[] {'_'});
                    if (name_file_split.Length == 7)
                    {
                        symbol = name_file_split[0].Trim();
                        time = name_file_split[4].Trim() + ":" + name_file_split[5].Trim() + ":" +
                               name_file_split[6].Replace(".html", "").Trim();
                        date = name_file_split[3].Trim() + "/" + name_file_split[2].Trim() + "/" +
                               name_file_split[1].Trim();
                        date_time = new DateTime(Convert.ToInt32(name_file_split[1]),
                                                 Convert.ToInt32(name_file_split[2]),
                                                 Convert.ToInt32(name_file_split[3]));
                        if ((date_time.Date >= FBegin.Date) && (date_time.Date <= FEnd.Date) &&
                            (FSymbols.Contains(symbol)))
                        {
                            html_reader =
                                new StreamReader(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\HEADER.html");
                            html_string = html_reader.ReadToEnd();
                            html_reader.Close();
                            html_string = html_string.Replace("SYMBOL", symbol);
                            html_string = html_string.Replace("TIME", time);
                            html_string = html_string.Replace("DATE", date);
                            html_string = html_string.Replace("EDIT",
                                                              "EDIT: " + Directory.GetCurrentDirectory() +
                                                              "\\Base\\TRADEDIARY\\" + name_file);
                            html_string = html_string.Replace("DELETE",
                                                              "DELETE: " + Directory.GetCurrentDirectory() +
                                                              "\\Base\\TRADEDIARY\\" + name_file); 
                            html_string = html_string.Replace("CREATED",
                                                               "CREATED: " + Directory.GetCurrentDirectory() +
                                                               "\\Base\\TRADEDIARY\\" + name_file);
                            html_writer.Write(html_string);
                            html_writer.Write("\n");
                            html_reader =
                                new StreamReader(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\" + name_file);
                            html_string = html_reader.ReadToEnd();
                            html_reader.Close();
                            html_writer.Write(html_string);
                            html_writer.Write("\n");
                        }


                    }

                }
                html_writer.Close();
                using (Stream stream = new FileStream(path, FileMode.Open))
                {
                    HtmlFormatProvider htmlProvider = new HtmlFormatProvider();
                    document = htmlProvider.Import(stream);
                    stream.Close();
                    this.radRichTextBox1.Document = document;
                    radRichTextBox1.Refresh();
                    this.radRichTextBox1.HyperlinkClicked += new System.EventHandler<Telerik.WinControls.RichTextBox.Model.HyperlinkClickedEventArgs>(this.HyperlinkClicked);
                    this.radRichTextBox1.HyperlinkNavigationMode =
                        Telerik.WinControls.RichTextBox.HyperlinkNavigationMode.Click;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Update()
        {
            ParentControl.LoadToolTipMenu();
            ParentControl.DiaryCurrentSymbols = ParentControl.SymbolsDiary;
            FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart, ParentControl.DiaryEnd);
            //this.radRichTextBox1.HyperlinkClicked += new System.EventHandler<Telerik.WinControls.RichTextBox.Model.HyperlinkClickedEventArgs>(this.HyperlinkClicked);
            this.radRichTextBox1.HyperlinkNavigationMode = Telerik.WinControls.RichTextBox.HyperlinkNavigationMode.Click;
        }

        private void InitializeUI()
        {
            this.stylesInitializing = true;
            StyleDefinition styleDefinition = this.radRichTextBox1.CurrentEditingStyle;
            this.InitializeCurrentFontStyles(styleDefinition);
            this.InitializeCurrentParagraphStyles(styleDefinition);

            BaselineAlignment baselineAlignment = (BaselineAlignment)styleDefinition.GetPropertyValue(Span.BaselineAlignmentProperty);

            this.stylesInitializing = false;
        }

        private void InitializeCurrentFontStyles(StyleDefinition styleDefinition)
        {
            UnderlineType underlineType = (UnderlineType)styleDefinition.GetPropertyValue(Span.UnderlineTypeProperty);

            string fontFamiliy = (string)styleDefinition.GetPropertyValue(Span.FontFamilyProperty);

            float fontSize = (float)styleDefinition.GetPropertyValue(Span.FontSizeProperty);
            fontSize = (float)Math.Round(Unit.DipToPoint(fontSize), 1);

            TextStyle fontStyle = (TextStyle)styleDefinition.GetPropertyValue(Span.FontStyleProperty);
            bool strikeThrough = (bool)styleDefinition.GetPropertyValue(Span.StrikethroughProperty);
        }

        private void InitializeCurrentParagraphStyles(StyleDefinition styleDefinition)
        {
            Paragraph paragraph = this.radRichTextBox1.Document.CaretPosition.GetCurrentParagraphBox().AssociatedParagraph;
            RadTextAlignment textAlignment = paragraph.TextAlignment;

            ListNumberingFormat? format = null;

            if (paragraph.IsInList)
            {
                format = paragraph.ListItemInfo.List.Style.Levels[paragraph.ListItemInfo.ListLevel].NumberingFormat;
            }

        }

        #region Event handlers

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            InstalledFontCollection font = new InstalledFontCollection();

            List<string> families = new List<string>();

            foreach (FontFamily familiy in font.Families)
            {
                if (familiy.IsStyleAvailable(FontStyle.Regular) &&
                    familiy.IsStyleAvailable(FontStyle.Italic) &&
                    familiy.IsStyleAvailable(FontStyle.Bold))
                {
                    families.Add(familiy.Name);
                }
            }


            this.InitializeUI();
        }

        private void OnInsertTableSelectionChanged(object sender, TableSelectionChangedEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.radRichTextBox1.InsertTable(e.RowIndex + 1, e.ColumnIndex + 1);
            }
        }

        #endregion
        private void HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            //e.Handled = true;
            if (e.URL.Contains("http://"))
            {
                ctlWeb aba = (ctlWeb)MFrmMain.radDock2.DocumentManager.ActiveDocument.ActiveControl;
                aba.Navigate(e.URL);
                
                MFrmMain.radDock2.ActiveWindow =
                    MFrmMain.radDock2.DocumentManager.DocumentArray[0];
                e.Handled = true;
            }
            else if (e.URL.StartsWith("CREATED: "))
            {
                Change_Date(e.URL.Replace("CREATED: ", ""));
                FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart,
                                      ParentControl.DiaryEnd);
                e.Handled = true;

            }
            else if (e.URL.StartsWith("EDIT: "))
            {
                Create_Edit(e.URL.Replace("EDIT: ", "")); 
                FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart,
                                        ParentControl.DiaryEnd);
                e.Handled = true;
            }
            else if (e.URL.StartsWith("DELETE: "))
            {
                try
                {
                    if (RadMessageBox.Show("Delete the post " +
                                       e.URL.Replace("DELETE: "+Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\", "") + "?", "DELETE POST", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        FileInfo file_delete = new FileInfo(e.URL.Replace("DELETE: ", ""));
                        file_delete.Delete();
                        FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart,
                                      ParentControl.DiaryEnd);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                
            }

            radRichTextBox1.Focusable = false; 
            e.Handled = true;
            this.radRichTextBox1.HyperlinkNavigationMode = Telerik.WinControls.RichTextBox.HyperlinkNavigationMode.Click; 
            //this.radRichTextBox1.HyperlinkClicked += new System.EventHandler<Telerik.WinControls.RichTextBox.Model.HyperlinkClickedEventArgs>(this.HyperlinkClicked);
            
           
        }
          
        private void Change_Date(string url)
        {
            NewDiaryPost _change_date = new NewDiaryPost(this, url);
            _change_date.ShowDialog();
        }
        private void Create_Edit(string URL)
        {
            foreach (string s in URL_Open)
            {
                if (URL == s)
                {
                    MessageBox.Show("The post is already open!");
                    return;
                }
            }
            DiaryEditor _edit = new DiaryEditor(URL, this);
            _edit.Show();
            _edit.BringToFront();
            URL_Open.Add(URL);
        }

        public void Close_URL(string URL)
        {
            URL_Open.Remove(URL);
        }

        private void radRichTextBox1_Click(object sender, EventArgs e)
        {
            radRichTextBox1.Focusable = true;
        }

        private void radRichTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) e.Handled = true;
        }

        private void BnAddPost_Click(object sender, EventArgs e)
        {
            NewDiaryPost _new = new NewDiaryPost(this);
            _new.ShowDialog();
            ParentControl.RefreshDdldiarysymbol();
            ParentControl.LoadToolTipMenu();
            ParentControl.DiaryCurrentSymbols = ParentControl.SymbolsDiary;
            FilterDiaries(ParentControl.DiaryCurrentSymbols, ParentControl.DiaryStart, ParentControl.DiaryEnd);

        }



    }
}
