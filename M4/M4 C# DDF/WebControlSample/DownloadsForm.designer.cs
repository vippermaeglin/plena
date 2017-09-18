using M4.WebControlSample.ComponentModel;
using M4.WebControlSample.Model;

namespace M4.WebControlSample
{
    partial class DownloadsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.downloadsImageList = new System.Windows.Forms.ImageList(this.components);
            this.clearToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.downloadCollectionDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new DataGridViewStatusColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new DataGridViewProgressBarColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.downloadCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.downloadsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadCollectionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadCollectionBindingSource)).BeginInit();
            this.downloadsContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageList = this.downloadsImageList;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(524, 25);
            this.toolStrip.TabIndex = 1;
            // 
            // downloadsImageList
            // 
            this.downloadsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("downloadsImageList.ImageStream")));
            this.downloadsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.downloadsImageList.Images.SetKeyName(0, "development 02 grey.png");
            this.downloadsImageList.Images.SetKeyName(1, "development 02.png");
            // 
            // clearToolStripButton
            // 
            this.clearToolStripButton.ImageIndex = 0;
            this.clearToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.clearToolStripButton.Name = "clearToolStripButton";
            this.clearToolStripButton.Size = new System.Drawing.Size(125, 22);
            this.clearToolStripButton.Text = "Remove Complete";
            this.clearToolStripButton.Click += new System.EventHandler(this.clearToolStripButton_Click);
            this.clearToolStripButton.MouseEnter += new System.EventHandler(this.clearToolStripButton_MouseEnter);
            this.clearToolStripButton.MouseLeave += new System.EventHandler(this.clearToolStripButton_MouseLeave);
            // 
            // downloadCollectionDataGridView
            // 
            this.downloadCollectionDataGridView.AllowUserToAddRows = false;
            this.downloadCollectionDataGridView.AllowUserToResizeColumns = false;
            this.downloadCollectionDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.downloadCollectionDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.downloadCollectionDataGridView.AutoGenerateColumns = false;
            this.downloadCollectionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.downloadCollectionDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.downloadCollectionDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.downloadCollectionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.downloadCollectionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewCheckBoxColumn3,
            this.dataGridViewCheckBoxColumn4});
            this.downloadCollectionDataGridView.DataSource = this.downloadCollectionBindingSource;
            this.downloadCollectionDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadCollectionDataGridView.Location = new System.Drawing.Point(0, 25);
            this.downloadCollectionDataGridView.Name = "downloadCollectionDataGridView";
            this.downloadCollectionDataGridView.ReadOnly = true;
            this.downloadCollectionDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.downloadCollectionDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.downloadCollectionDataGridView.RowTemplate.ContextMenuStrip = this.downloadsContextMenuStrip;
            this.downloadCollectionDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.downloadCollectionDataGridView.Size = new System.Drawing.Size(524, 383);
            this.downloadCollectionDataGridView.TabIndex = 3;
            this.downloadCollectionDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadCollectionDataGridView_CellContentClick);
            this.downloadCollectionDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadCollectionDataGridView_CellContentDoubleClick);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsDownloading";
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.Width = 5;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FileName";
            this.dataGridViewTextBoxColumn3.HeaderText = "File Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "FileSize";
            this.dataGridViewTextBoxColumn4.HeaderText = "Size";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 52;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "URL";
            this.dataGridViewTextBoxColumn1.HeaderText = "URL";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Progress";
            this.dataGridViewTextBoxColumn2.HeaderText = "Progress";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.dataGridViewCheckBoxColumn2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewCheckBoxColumn2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGridViewCheckBoxColumn2.HeaderText = "";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.ReadOnly = true;
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn2.Text = "X";
            this.dataGridViewCheckBoxColumn2.ToolTipText = "Cancel";
            this.dataGridViewCheckBoxColumn2.UseColumnTextForButtonValue = true;
            this.dataGridViewCheckBoxColumn2.Width = 5;
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.DataPropertyName = "IsDownloadComplete";
            this.dataGridViewCheckBoxColumn3.HeaderText = "IsDownloadComplete";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.ReadOnly = true;
            this.dataGridViewCheckBoxColumn3.Visible = false;
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.DataPropertyName = "IsDisposed";
            this.dataGridViewCheckBoxColumn4.HeaderText = "IsDisposed";
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            this.dataGridViewCheckBoxColumn4.ReadOnly = true;
            this.dataGridViewCheckBoxColumn4.Visible = false;
            // 
            // downloadCollectionBindingSource
            // 
            this.downloadCollectionBindingSource.DataSource = typeof(Download);
            // 
            // downloadsContextMenuStrip
            // 
            this.downloadsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openContainingFolderToolStripMenuItem,
            this.cancelDownloadToolStripMenuItem,
            this.removeEntryToolStripMenuItem});
            this.downloadsContextMenuStrip.Name = "downloadsContextMenuStrip";
            this.downloadsContextMenuStrip.Size = new System.Drawing.Size(202, 70);
            this.downloadsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.downloadsContextMenuStrip_Opening);
            // 
            // openContainingFolderToolStripMenuItem
            // 
            this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            this.openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.openContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.openContainingFolderToolStripMenuItem_Click);
            // 
            // cancelDownloadToolStripMenuItem
            // 
            this.cancelDownloadToolStripMenuItem.Name = "cancelDownloadToolStripMenuItem";
            this.cancelDownloadToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.cancelDownloadToolStripMenuItem.Text = "Cancel Download";
            this.cancelDownloadToolStripMenuItem.Click += new System.EventHandler(this.cancelDownloadToolStripMenuItem_Click);
            // 
            // removeEntryToolStripMenuItem
            // 
            this.removeEntryToolStripMenuItem.Name = "removeEntryToolStripMenuItem";
            this.removeEntryToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.removeEntryToolStripMenuItem.Text = "Remove Entry";
            this.removeEntryToolStripMenuItem.Click += new System.EventHandler(this.removeEntryToolStripMenuItem_Click);
            // 
            // DownloadsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(524, 408);
            this.Controls.Add(this.downloadCollectionDataGridView);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadsForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
            this.Text = "Downloads";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadCollectionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadCollectionBindingSource)).EndInit();
            this.downloadsContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.BindingSource downloadCollectionBindingSource;
        private System.Windows.Forms.DataGridView downloadCollectionDataGridView;
        private System.Windows.Forms.ContextMenuStrip downloadsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeEntryToolStripMenuItem;
        private System.Windows.Forms.ImageList downloadsImageList;
        private System.Windows.Forms.ToolStripButton clearToolStripButton;
        private DataGridViewStatusColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewProgressBarColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
    }
}