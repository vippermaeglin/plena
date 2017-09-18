using System;
using Awesomium.Core;
using System.Windows.Forms;
using M4.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace M4.WebControlSample
{
    partial class HistoryForm : DockContent
    {
        #region Ctors
        public HistoryForm()
        {
            InitializeComponent();
            filterToolStripComboBox.SelectedIndex = 0;
        }
        #endregion


        #region Methods
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ClearSource();
            base.OnFormClosed(e);
        }

        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);

            if (IsHidden)
            {
                ClearSource();
                Settings.Default.History = false;
            }
            else if (VisibleState != DockState.Unknown)
            {
                RefreshSource();
                Settings.Default.History = true;
            }
        }

        // Disposes unused HistoryQueryResult.
        private void ClearSource()
        {
            HistoryQueryResult oldSource = historyQueryResultBindingSource.DataSource as HistoryQueryResult;

            if (oldSource != null)
            {
                using (oldSource)
                    historyQueryResultBindingSource.DataSource = null;
            }
        }

        internal void RefreshSource()
        {
            // Make sure you always dispose previous HistoryQueryResult,
            // to avoid memory leaks.
            ClearSource();

            if (!WebCore.IsRunning)
                return;

            switch (filterToolStripComboBox.SelectedIndex)
            {
                case 0:
                    historyQueryResultBindingSource.DataSource = WebCore.QueryHistory(1);
                    break;
                case 1:
                    historyQueryResultBindingSource.DataSource = WebCore.QueryHistory(7);
                    break;
                case 2:
                    historyQueryResultBindingSource.DataSource = WebCore.QueryHistory(30);
                    break;
                case 3:
                    historyQueryResultBindingSource.DataSource = WebCore.QueryHistory();
                    break;
            }
        }
        #endregion

        #region Properties

        public BrowserForm BrowserForm
        {
            get;
            set;
        }

        #endregion

        #region Event Handlers
        private void filterToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSource();
        }

        private void refreshToolStripButton_MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex += 1;
        }

        private void refreshToolStripButton_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex -= 1;
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshSource();
        }

        private void historyQueryResultDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
                return;

            if (DockPanel != null)
            {
                HistoryQueryResult source = historyQueryResultBindingSource.DataSource as HistoryQueryResult;

                if (source != null)
                {
                    HistoryEntry entry = source[e.RowIndex];
                    WebDocument doc = new WebDocument(entry.Url);
                    doc.Show(DockPanel);
                }
            }
        }
        #endregion
    }
}
