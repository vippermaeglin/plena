/***************************************************************************
 *  Project: WebControl
 *  File:    DownloadsForm.cs
 *  Version: 1.0.0.0
 *
 *  Copyright ©2012 Perikles C. Stephanidis; All rights reserved.
 *  This code is provided "AS IS" without warranty of any kind.
 *__________________________________________________________________________
 *
 *  Notes:
 *
 *  A Downloads content window displaying a list of active downloads.
 *   
 ***************************************************************************/

#region Using

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using M4.Properties;
using M4.WebControlSample.Model;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Specialized;

#endregion

namespace M4.WebControlSample
{
    partial class DownloadsForm : DockContent
    {
        readonly BrowserForm _browserForm;
        
        public DownloadsForm(BrowserForm parentForm)
        {
            _browserForm = parentForm;

            InitializeComponent();

            downloadCollectionBindingSource.DataSource = parentForm.Downloads;
            parentForm.Downloads.CollectionChanged += OnSourceCollectionChanged;
        }
        
        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);

            if (IsHidden)
                Settings.Default.Downloads = false;
            else if (VisibleState != DockState.Unknown)
                Settings.Default.Downloads = true;
        }

        private Download GetSelectedDownload()
        {
            if (downloadCollectionDataGridView.SelectedRows.Count == 0)
                return null;

            return downloadCollectionDataGridView.SelectedRows[0].DataBoundItem as Download;
        }

        private void CancelSelectedDownload()
        {
            Download download = GetSelectedDownload();

            if ((download != null) && download.IsDownloading)
            {
                if (MessageBox.Show(this,
                        String.Format("Are you sure you want to cancel downloading {0}?", download.FileName),
                        "Awesomium.NET",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Race condition.
                    if (download.IsDownloading)
                    {
                        download.Cancel();
                        BrowserForm.Downloads.Remove(download);
                    }
                    else
                        MessageBox.Show(this, "Download was complete before your response.", "Awesomium.NET");
                }
            }
        }
        
        public BrowserForm BrowserForm
        {
            get
            {
                return _browserForm;
            }
        }

        private void clearToolStripButton_MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex += 1;
        }

        private void clearToolStripButton_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).ImageIndex -= 1;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            downloadCollectionBindingSource.ResetBindings(true);
        }

        private void downloadCollectionDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            Download download = GetSelectedDownload();

            if ((download != null) && download.CanOpenDownloadedFile())
                download.OpenDownloadedFile();
        }

        private void downloadCollectionDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 5)
                return;

            CancelSelectedDownload();
        }

        private void downloadsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            Download download = GetSelectedDownload();

            if (download != null)
            {
                openContainingFolderToolStripMenuItem.Enabled = download.CanOpenDownloadedFile();
                cancelDownloadToolStripMenuItem.Enabled = download.IsDownloading;
                removeEntryToolStripMenuItem.Enabled = !download.IsDownloading;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Download download = GetSelectedDownload();

            if ((download != null) && download.CanOpenDownloadedFile())
                download.OpenDownloadedFileFolder();
        }

        private void cancelDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelSelectedDownload();
        }

        private void removeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Download download = GetSelectedDownload();

            if ((download != null) && !download.IsDownloading)
                BrowserForm.Downloads.Remove(download);
        }

        private void clearToolStripButton_Click(object sender, EventArgs e)
        {
            if (BrowserForm.Downloads.Count == 0)
                return;

            List<Download> downloads = new List<Download>(BrowserForm.Downloads);

            foreach (Download download in downloads)
            {
                if (download.IsDownloadComplete || download.IsCancelled)
                    BrowserForm.Downloads.Remove(download);
            }
        }
    }
}
