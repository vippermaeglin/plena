/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using M4.modulusfe.platform;

namespace M4
{
    public partial class ctlData : DdfDataManager
    {
        #region Members and Initialization

        //private readonly Service _svc = new Service();
        private readonly CultureInfo _usCulture = new CultureInfo("en-US");

        #endregion



        public ctlData(frmMain2 oMain2)
        {
            InitializeComponent();
            //MFrmMain = oMain;
            MFrmMain2 = oMain2;
        }

        #region Alert Handling
        private void tmrAlerts_Tick(object sender, EventArgs e)
        {
            //CheckAlerts();
        }

        
        private void CheckAlerts()
        {
            //Checks the web service for alerts
            bool NewAlert = false;
            DataSet ds = null;

            try
            {
               // ds = _svc.GetAlerts(frmMain.ClientId, frmMain.ClientPassword, frmMain.LicenseKey);
                //ds = _svc.GetAlerts(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
                if ((ds == null) || (ds.Tables["UserData"].Rows.Count == 0))
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            for (int n = 0; n < ds.Tables["UserData"].Rows.Count; n++)
            {
                if (!Shown(ds.Tables["UserData"].Rows[n]["DataKey"].ToString()))
                {
                    string alert = "";
                    string symbol = "";
                    string id = "";
                    DateTime dateTime;
                    string[] keys = Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]).Split(new[] { '|' });
                    if (keys.Length > 4)
                    {
                        try
                        {
                            dateTime = DateTime.Parse(keys[1], _usCulture.DateTimeFormat);
                        }
                        catch
                        {
                            try
                            {
                                dateTime = Convert.ToDateTime(keys[1]);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        alert = keys[2];
                        symbol = keys[3];
                        id = keys[4];
                    }

                    switch (alert)
                    {
                        case "NEWS":
                            ShowNewsAlert(symbol, Convert.ToString(ds.Tables["UserData"].Rows[n]["EntryDate"]),
                                               Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]),
                                               Convert.ToString(ds.Tables["UserData"].Rows[n]["Data"]));
                            break;

                        case "ORDER":
                            ShowOrderAlert(symbol, Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]),
                                                Convert.ToString(ds.Tables["UserData"].Rows[n]["Data"]));
                            break;

                        case "SCRIPT":
                            ShowScriptAlert(Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]),
                                                 Convert.ToString(ds.Tables["UserData"].Rows[n]["Data"]));
                            break;
                    }
                    NewAlert = true;
                }
            }

            if (NewAlert && File.Exists(Application.StartupPath + @"\Res\Alert.wav"))
            {
                try
                {
                    UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\Alert.wav", 0, UnmanagedMethods.SND_FILENAME | UnmanagedMethods.SND_ASYNC);
                }
                catch (Exception)
                {
                }
            }
        }
        

        private void ShowNewsAlert(string symbol, string EntryDate, string DataKey, string Data)
        {
            string headline = "";
            string[] blocks = Data.Split(new[] { '}' });
            if (blocks.Length > 2)
            {
                headline = blocks[2].Substring(2);
            }
            string text = "NEWS: " + symbol + ": " + headline;
            //OutputWindow1.DisplayAlertOrMessage(text, OutputWindow.OutputIcon.Warning, DataKey, EntryDate);
        }

        private void ShowOrderAlert(string symbol, string DataKey, string Data)
        {
            string[] order = Data.Split('|');
            if (order.Length >= 8)
            {
                string text = "ORDER: " + symbol + ": STATUS=" + order[1] + ", SIDE=" + order[4] + ", QTY=" + order[5] +
                              ", ENTRY=" + order[6] + ", TYPE=" + order[7] + ", EXPIRES=" + order[8];
                //OutputWindow1.DisplayAlertOrMessage(text, OutputWindow.OutputIcon.Warning, DataKey, order[3]);
            }
        }

        private void ShowScriptAlert(string DataKey, string Data)
        {
            string[] script = Data.Split('|');
            if (script.Length >= 1)
            {
                //OutputWindow1.DisplayAlertOrMessage(script[1], OutputWindow.OutputIcon.Warning, DataKey, script[0]);
            }
        }

        private bool Shown(string DataKey)
        {
            //int count = OutputWindow1.m_ListAlerts.Items.Count - 1;
            //for (int n = 0; n <= count; n++)
            //{
            //    ListViewItem item = OutputWindow1.m_ListAlerts.Items[n];
            //    if (item.Tag.ToString() == DataKey)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        #endregion

    }
}
