/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using M4Core.Entities;
using M4.M4v2.Chart;

namespace M4
{
    public partial class ctlWeb : UserControl
    {
        public string Title;
        public string Url;
        public frmMain2 mFrmMain2;


        public ctlWeb(string sUrl, string sTitle, frmMain2 parent)
        {
            InitializeComponent();

            Url = sUrl;
            Title = sTitle;

            Navigate(Url);

            SetIeProcess();

            Translate();

            mFrmMain2 = parent;

            //DONT WORKS!
            //AllowDrop = true;
            WebBrowser1.AllowWebBrowserDrop = true;
            //radPanel1.AllowDrop = true;
            //radPanel1.DragEnter += ctlWeb_DragEnter;
            //radPanel1.DragDrop += ctlWeb_DragDrop;


        }

        void ctlWeb_DragDrop(object sender, DragEventArgs e)
        {
            string symbol = e.Data.GetData(typeof(string)).ToString();
            if (symbol == "") return;

            ChartSelection selection = null;
            switch (Properties.Settings.Default.DefaultPeriodicity)
            {
                case "Daily":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Daily,
                        Source = "PLENA"
                    };
                    break;
                case "Weekly":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Weekly,
                        Source = "PLENA"
                    };
                    break;
                case "Month":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Month,
                        Source = "PLENA"
                    };
                    break;
                case "Yearly":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Year,
                        Source = "PLENA"
                    };
                    break;
                default:
                    selection = (new FrmSelectChart { CodeStock = symbol }).GetChartSelection(symbol);
                    break;
            }

            if (selection == null)
                return;
            mFrmMain2.CreateNewCtlPainel(selection, chart =>
            {
                M4.M4v2.Settings.Scheme.Instance().UpdateChartColors(chart.StockChartX1, Properties.Settings.Default.SchemeColor);
                chart.m_SchemeColor = Properties.Settings.Default.SchemeColor;
                chart.StockChartX1.Visible = true;
                this.Focus();
            });
        }

        void ctlWeb_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;


        }

        private void Translate()
        {
            tsbUndo.ToolTipText = Program.LanguageDefault.DictionaryBrowser["tsbUndo"];
            tsbRefresh.ToolTipText = Program.LanguageDefault.DictionaryBrowser["tsbRefresh"];
            tsbRedo.ToolTipText = Program.LanguageDefault.DictionaryBrowser["tsbRedo"];
            tsbPageHome.ToolTipText = Program.LanguageDefault.DictionaryBrowser["tsbPageHome"];
            tsbNavigate.ToolTipText = Program.LanguageDefault.DictionaryBrowser["tsbNavigate"];
        }

        private void WebBrowser1DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //_mFrmMain.ShowStatus("");

            addressBox.Text = WebBrowser1.Url.ToString();
        }

        private void WebBrowser1Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //_mFrmMain.ShowStatus("Opening " + Title + "...");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            WebBrowser1.Width = ClientSize.Width;
            WebBrowser1.Height = ClientSize.Height - pageToolStrip.Height;
            WebBrowser1.Location = new Point(0, pageToolStrip.Height);
        }

        // Navigates to the given URL if it is valid.
        public void Navigate(String address)
        {
            if (String.IsNullOrEmpty(address))
                return;

            if (address.Equals("about:blank"))
                return;

            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                Url = address;
                WebBrowser1.Navigate(new Uri(address));
            }
            catch (UriFormatException)
            {
                return;
            }
        }

        private void AddressBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyCode.Equals(Keys.Enter))
                return;

            Navigate(addressBox.Text);

            e.SuppressKeyPress = true;
        }

        private void ToolStripButton1Click(object sender, EventArgs e)
        {
            WebBrowser1.GoBack();
        }

        private void ToolStripButton2Click(object sender, EventArgs e)
        {
            WebBrowser1.GoForward();
        }

        private void ToolStripButton3Click(object sender, EventArgs e)
        {
            WebBrowser1.Refresh();
        }

        private void ToolStripButton4Click(object sender, EventArgs e)
        {
            WebBrowser1.GoHome();
        }

        public void SetIeProcess()
        {
            String appname = Process.GetCurrentProcess().ProcessName + ".exe";
            RegistryKey rk8 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            const int value9 = 9000;
            const int value8 = 8000;
            Version ver = WebBrowser1.Version;
            int value = value9;
            try
            {
                string[] parts = ver.ToString().Split('.');
                int vn;
                int.TryParse(parts[0], out vn);
                if (vn != 0)
                {
                    value = vn == 9 ? value9 : value8;
                }
            }
            catch
            {
                value = value9;
            }
            
            //Setting the key in LocalMachine
            if (rk8 != null)
            {
                try
                {
                    rk8.SetValue(appname, value, RegistryValueKind.DWord);
                    rk8.Close();
                }
                catch (Exception)
                {
                    //Telerik.WinControls.RadMessageBox.Show(ex.Message);
                }
            }
        }

        private void ToolStripButton5Click(object sender, EventArgs e)
        {
            Navigate(addressBox.Text);
        }
    }
}
