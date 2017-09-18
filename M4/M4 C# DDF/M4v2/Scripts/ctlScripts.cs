using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace M4.M4v2.Scripts
{
    public partial class ctlScripts : UserControl
    {
        private readonly ctlData m_ctlData;
        private readonly frmMain2 m_frmMain;
        private readonly ScriptManager svc = new ScriptManager();
        public ctlScripts(frmMain2 oMain, ctlData oData)
        {
            InitializeComponent();
            m_frmMain = oMain;
            m_ctlData = oData;
        }

        private void ctlScripts_Load(object sender, EventArgs e)
        {
            tabBuyScript.Controls.Add(new ctlPalette(this));
            tabSellScript.Controls.Add(new ctlPalette(this));
            GetScriptsNames();
        }
        private void GetScriptsNames()
        {
            string[] alerts = null;
            try
            {
                object[] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
                if (_ != null)
                {
                    alerts = new string[_.Length];
                    for (int i = 0; i < _.Length; i++)
                        alerts[i] = _[i].ToString();
                }
            }
            catch (Exception)
            {
                return;
            }
            if (alerts == null) return;

            //Add them to combobox
            cboScript.Items.Clear();
            for (int n = 0; n <= alerts.Length - 1; n++)
            {
                if (alerts[n].StartsWith("Trade Alert Settings: "))
                {
                    cboScript.Items.Add(alerts[n].Replace("Trade Alert Settings: ", ""));
                }
            }
        }

        //Load script from disk:
        private void cboScript_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadScript();
        }

        //Loads an alert
        private void LoadScript()
        {
            string alertName = cboScript.Text;
            if (alertName == "") return;
            string data;
            try
            {
                data = svc.GetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                                       "Trade Alert Settings: " + alertName);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] text = data.Split(new[] { Utils.Chr(134) });
            if (text.Length < 23) return;

            txtSymbol.Text = text[0];
            for (short n = 0; n <= cboPeriodicity.Items.Count - 1; n = (short)(n + 1))
            {
                if (cboPeriodicity.Items[n].ToString() == text[1])
                {
                    cboPeriodicity.SelectedIndex = n;
                    break;
                }
            }
            txtInterval.Text = text[2];
            txtBars.Text = text[3];

            ((ctlPalette)tabBuyScript.Controls[0]).LoadDiagram(svc.Path + "ALERTS\\" + alertName + "\\" + alertName + " - BUY.xml");
            ((ctlPalette)tabSellScript.Controls[0]).LoadDiagram(svc.Path + "ALERTS\\" + alertName + "\\" + alertName + " - SELL.xml");
            //txtBuyScript = text[4];
            //txtSellScript = text[5];
            //txtExitLongScript = text[6];
            //txtExitShortScript = text[7];

            

        }
        public void SaveScript()
        {
            if (cboScript.Text == "")
            {
                cboScript.Items.Add("Untitled");
                cboScript.Text = "Untitled";
            }
            string alertName = cboScript.Text;

            if (!VerifyForm()) return;

            string txtBuyScript = ((ctlPalette)tabBuyScript.Controls[0]).GenerateScript();
            string txtSellScript = ((ctlPalette)tabSellScript.Controls[0]).GenerateScript();
            string txtExitLongScript = "";// ((ctlPalette)tabBuyScript.Controls[0]).GenerateScript();
            string txtExitShortScript = "";// ((ctlPalette)tabSellScript.Controls[0]).GenerateScript();

            if (txtBuyScript == "[ERROR]") 
            {
                MessageBox.Show("Há erros no script de compra!");
                return;
            }
            if (txtSellScript == "[ERROR]")
            {
                MessageBox.Show("Há erros no script de venda!");
                return;
            }

            string d = Utils.Chr(134).ToString();

            string data = txtSymbol.Text + d + cboPeriodicity.Text + d + txtInterval.Text + d +
                          txtBars.Text +
                          d + txtBuyScript + d + txtSellScript + d + txtExitLongScript + d +
                          txtExitShortScript + d + false + d +
                          "Sumário" +
                          d + txtSymbol.Text + d + "" + d + "" + d +
                          false + d + false + d +
                          false + d + false + d +
                          "" + d + false + d + false +
                          d + false + d + false + d + "";
            try
            {
                svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                                "Trade Alert Settings: " + alertName, data);


                if (!Directory.Exists(svc.Path + "ALERTS\\" + alertName))
                    Directory.CreateDirectory(svc.Path + "ALERTS\\" + alertName);
                ((ctlPalette)tabBuyScript.Controls[0]).SaveDiagram(svc.Path+"ALERTS\\"+alertName+"\\",alertName+" - BUY");
                ((ctlPalette)tabSellScript.Controls[0]).SaveDiagram(svc.Path + "ALERTS\\" + alertName + "\\", alertName + " - SELL");
                MessageBox.Show("Script salvo com sucesso!");
                cboScript.SelectedIndexChanged -= cboScript_SelectedIndexChanged;
                GetScriptsNames();
                cboScript.SelectedItem = cboScript.Items[cboScript.Items.IndexOf(alertName)];
                cboScript.SelectedIndexChanged += cboScript_SelectedIndexChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*bool found = false;
            for (short n = 0; n <= cboScript.Items.Count - 1; n++)
            {
                if (cboScript.Items[n] == txtAlertName.Text)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                LoadScript();
                for (short n = 0; n <= cboAlerts.Items.Count - 1; n++)
                {
                    if (cboAlerts.Items[n] == alertName)
                    {
                        cboAlerts.SelectedIndex = n;
                        break;
                    }
                }
            }
            UpdateName(txtAlertName.Text);
            cmdSave.Enabled = true;
            if (cboAlerts.SelectedIndex > -1)
            {
                cmdDelete.Enabled = true;
            }
            cboAlerts.Enabled = true;*/
        }
        public void DisplayScript(string script)
        {
            richTextBox1.Text = script;
        }

        //Validates the form for saving
        private bool VerifyForm()
        {
            uint tmpVal;
            if (cboScript.Text == "")
            {
                cboScript.Items.Add("Untitled");
                cboScript.Text = "Untitled";
            }
            if (txtSymbol.Text == "")
            {
                txtSymbol.Focus();
                MessageBox.Show("Please enter a symbol", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cboPeriodicity.Text == "")
            {
                cboPeriodicity.Focus();
                MessageBox.Show("Please select a periodicity", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((txtBars.Text == "") | !Utils.IsNumeric(txtInterval.Text))
            {
                MessageBox.Show("Please enter the number of bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((txtInterval.Text == "") | !Utils.IsNumeric(txtInterval.Text))
            {
                MessageBox.Show("Please enter the interval", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!uint.TryParse(txtBars.Text, out tmpVal))
            {
                txtBars.SelectAll();
                txtBars.Focus();
                MessageBox.Show("Please enter the number of bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!uint.TryParse(txtInterval.Text, out tmpVal))
            {
                txtInterval.SelectAll();
                txtInterval.Focus();
                MessageBox.Show("Please enter the interval", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            //Get tab in focus:
            if (tabScripts.SelectedIndex==0)((ctlPalette)tabBuyScript.Controls[0]).GenerateScript();
            else if (tabScripts.SelectedIndex == 1) ((ctlPalette)tabSellScript.Controls[0]).GenerateScript();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveScript();
        }
    }
}
