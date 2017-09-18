/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using M4.Properties;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace M4.M4v2.Portfolio
{
    public partial class OutputWindowV2 : UserControl
    {
        public enum OutputIcon
        {
            Chart,
            Check,
            System,
            Info,
            Report,
            Warning,
        }

        public enum OutputType
        {
            ConnectionStatus,
            AlertsAndMessaging,
        }

        public OutputWindowV2()
        {
            InitializeComponent();

            PrepareGrid();

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            m_tabConnection.Text = Program.LanguageDefault.DictionaryOutput["tabConnection"];
            m_tabAlerts.Text = Program.LanguageDefault.DictionaryOutput["tabAlerts"];
        }

        public void DisplayConnectionStatus(string info, OutputIcon icon)
        {
            DisplayConnectionStatus(info, icon, "");
        }

        public void DisplayConnectionStatus(string info, OutputIcon icon, string _Tag)
        {
            DisplayInfo(info, "", icon);
        }

        public void DisplayAlertOrMessage(string info, OutputIcon icon)
        {
            DisplayAlertOrMessage(info, icon, "", "");
        }

        public void DisplayAlertOrMessage(string info, OutputIcon icon, string _Tag)
        {
            DisplayAlertOrMessage(info, icon, _Tag, "");
        }

        public void DisplayAlertOrMessage(string info, OutputIcon icon, string _Tag, string _DateTime)
        {
            DisplayInfo(info, _DateTime, icon);
        }

        private void PrepareGrid()
        {
            GridViewImageColumn imageColumn = new GridViewImageColumn("Imagem")
            {
                Width = 16,
                HeaderText = String.Empty,
                TextAlignment = ContentAlignment.MiddleCenter
            };

            GridViewTextBoxColumn dateColumn = new GridViewTextBoxColumn("Data")
                                                   {
                                                       Width = 50, 
                                                       TextAlignment = ContentAlignment.MiddleCenter
                                                   };
            GridViewTextBoxColumn infoColumn = new GridViewTextBoxColumn("Informações")
                                                   {
                                                       Width = 90,
                                                       TextAlignment = ContentAlignment.MiddleLeft
                                                   };

            GrdListConnection.Columns.Add(imageColumn);
            GrdListConnection.Columns.Add(dateColumn);
            GrdListConnection.Columns.Add(infoColumn);
            GrdListConnection.TableElement.RowHeight = 22;
            GrdListConnection.MasterTemplate.AllowRowReorder = false;
            GrdListConnection.ReadOnly = true;
            GrdListConnection.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;


        }

        //Displays a record in the list box. Called by one of the functions above.
        private void DisplayInfo(string info, string dateTime, OutputIcon icon)
        {
            Image image = null;

            switch (icon)
            {
                case OutputIcon.Info:
                    image = Resources.info;
                    break;
                case OutputIcon.Warning:
                    image = Resources.warning;
                    break;
                case OutputIcon.Chart:
                    image = Resources.chart;
                    break;
            }

            GrdListConnection.Rows.Add(image, (dateTime.Length > 0 ? dateTime : DateTime.Now.ToString()), info);
        }

        private void MContextMenuOpening(object sender, CancelEventArgs e)
        {
            ListView list = (ListView)tabPageView.SelectedPage.Controls[0];

            if (list.Items.Count > 0)
            {
                cmnuClear.Enabled = true;
                cmnuClear.Text = "Clear " + tabPageView.SelectedPage.Text;
            }
            else
            {
                cmnuClear.Text = "Clear";
                cmnuClear.Enabled = false;
            }
        }

        private void CmnuClearClick(object sender, EventArgs e)
        {
            ListView list = (ListView)tabPageView.SelectedPage.Controls[0];
            list.Items.Clear();
        }

        private void OutputWindowResize(object sender, EventArgs e)
        {
            //GrdListAlerts.Columns[0].Width = 20;
            //GrdListAlerts.Columns[1].Width = 140;
            //GrdListAlerts.Columns[2].Width = Width - (GrdListConnection.Columns[0].Width + GrdListConnection.Columns[1].Width) - 15;

            GrdListConnection.Columns[0].Width = 20;
            GrdListConnection.Columns[1].Width = 140;
            GrdListConnection.Columns[2].Width = Width - (GrdListConnection.Columns[0].Width + GrdListConnection.Columns[1].Width) - 15;
        }

        private void OutputWindowV2Load(object sender, EventArgs e)
        {
            tabPageView.SelectedPage = m_tabConnection;
        }
    }

    public class ListViewColumnSorter : IComparer
    {
        private int _columnToSort;
        private SortOrder _orderOfSort;
        private readonly CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            _columnToSort = 0;
            _orderOfSort = SortOrder.Descending;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int compareResult = 0;
            try
            {
                compareResult = ObjectCompare.Compare(DateTime.Parse(listviewX.SubItems[_columnToSort].Text),
                                                          DateTime.Parse(listviewY.SubItems[_columnToSort].Text));
            }
            catch (Exception) { }

            if (_orderOfSort == SortOrder.Ascending)
                return (-compareResult);
            return _orderOfSort == SortOrder.Descending ? compareResult : 0;
        }

        public int SortColumn
        {
            get { return _columnToSort; }
            set { _columnToSort = value; }
        }

        public SortOrder Order
        {
            get { return _orderOfSort; }
            set { _orderOfSort = value; }
        }
    }
}
