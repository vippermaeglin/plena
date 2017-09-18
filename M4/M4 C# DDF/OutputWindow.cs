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

namespace M4
{
    public partial class OutputWindow : UserControl
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

        private readonly ListViewColumnSorter lvwColumnSorter;

        public OutputWindow()
        {
            InitializeComponent();

            m_ListConnection.HeaderStyle = ColumnHeaderStyle.None;
            m_ListAlerts.HeaderStyle = ColumnHeaderStyle.None;

            lvwColumnSorter = new ListViewColumnSorter();
            m_ListAlerts.ListViewItemSorter = lvwColumnSorter;
            m_ListConnection.ListViewItemSorter = lvwColumnSorter;

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            m_tabConnection.Text = Program.LanguageDefault.DictionaryOutput["tabConnection"];
            m_tabAlerts.Text = Program.LanguageDefault.DictionaryOutput["tabAlerts"];
        }

        public void DisplayConnectionStatus(string Info, OutputIcon Icon)
        {
            DisplayConnectionStatus(Info, Icon, "");
        }
        public void DisplayConnectionStatus(string Info, OutputIcon Icon, string _Tag)
        {
            DisplayInfo(OutputType.ConnectionStatus, Info, Icon, _Tag, "");
        }

        public void DisplayAlertOrMessage(string Info, OutputIcon Icon)
        {
            DisplayAlertOrMessage(Info, Icon, "", "");
        }
        public void DisplayAlertOrMessage(string Info, OutputIcon Icon, string _Tag)
        {
            DisplayAlertOrMessage(Info, Icon, _Tag, "");
        }
        public void DisplayAlertOrMessage(string Info, OutputIcon Icon, string _Tag, string _DateTime)
        {
            DisplayInfo(OutputType.AlertsAndMessaging, Info, Icon, _Tag, _DateTime);
        }

        //Displays a record in the list box. Called by one of the functions above.
        private void DisplayInfo(OutputType ot, string Info, OutputIcon Icon, string _Tag, string _DateTime)
        {
            ListView list;
            switch (ot)
            {
                case OutputType.AlertsAndMessaging:
                    list = m_ListAlerts;
                    m_TabCtrl.SelectedTab = m_tabAlerts;
                    break;
                case OutputType.ConnectionStatus:
                    list = m_ListConnection;
                    m_TabCtrl.SelectedTab = m_tabConnection;
                    break;
                default:
                    list = m_ListConnection;
                    m_TabCtrl.SelectedTab = m_tabConnection;
                    break;
            }

            ListViewItem lvi = new ListViewItem
                                 {
                                     Text = (_DateTime.Length > 0 ? _DateTime : DateTime.Now.ToString()),
                                     ImageIndex = ((int)Icon)
                                 };

            lvi.SubItems.Add(Info);
            lvi.Tag = _Tag;
            if (list.Items.Count % 2 == 0) lvi.BackColor = Color.WhiteSmoke;
            list.Items.Add(lvi);

            switch (ot)
            {
                case OutputType.AlertsAndMessaging:
                    m_ListAlerts.Sort();
                    break;
                case OutputType.ConnectionStatus:
                    m_ListConnection.Sort();
                    break;
            }

            lvi = list.Items[list.Items.Count - 1];
            lvi.EnsureVisible();
        }

        private void m_ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            ListView list = (ListView)m_TabCtrl.SelectedTab.Controls[0];

            if (list.Items.Count > 0)
            {
                cmnuClear.Enabled = true;
                cmnuClear.Text = "Clear " + m_TabCtrl.SelectedTab.Text;
            }
            else
            {
                cmnuClear.Text = "Clear";
                cmnuClear.Enabled = false;
            }
        }

        private void cmnuClear_Click(object sender, EventArgs e)
        {
            ListView list = (ListView)m_TabCtrl.SelectedTab.Controls[0];
            list.Items.Clear();
        }

        private void OutputWindow_Paint(object sender, PaintEventArgs e)
        {
            m_ListAlerts.BackColor = Color.White; //Fix for Nevron bug
            m_ListConnection.BackColor = Color.White;
            //      m_ListAlerts.ForeColor = frmMain.ForeColor;
            //      m_ListConnection.ForeColor = frmMain.ForeColor;
        }

        private void OutputWindow_Resize(object sender, EventArgs e)
        {
            m_ListAlerts.Columns[0].Width = 140;
            m_ListAlerts.Columns[1].Width = Width - m_ListAlerts.Columns[0].Width - 15;
            m_ListConnection.Columns[0].Width = 140;
            m_ListConnection.Columns[1].Width = Width - m_ListConnection.Columns[0].Width - 15;
        }
    }

    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort;
        private SortOrder OrderOfSort;
        private readonly CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            ColumnToSort = 0;
            OrderOfSort = SortOrder.Descending;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int compareResult = 0;
            try
            {
                compareResult = ObjectCompare.Compare(DateTime.Parse(listviewX.SubItems[ColumnToSort].Text),
                                                          DateTime.Parse(listviewY.SubItems[ColumnToSort].Text));
            }
            catch (Exception) { }

            if (OrderOfSort == SortOrder.Ascending)
                return (-compareResult);
            if (OrderOfSort == SortOrder.Descending)
                return compareResult;
            return 0;
        }

        public int SortColumn
        {
            get { return ColumnToSort; }
            set { ColumnToSort = value; }
        }

        public SortOrder Order
        {
            get { return OrderOfSort; }
            set { OrderOfSort = value; }
        }
    }
}
