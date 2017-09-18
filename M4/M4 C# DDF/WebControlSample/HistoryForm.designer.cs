namespace M4.WebControlSample
{
    partial class HistoryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryForm));
            this.historyQueryResultDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historyQueryResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.historyToolStrip = new System.Windows.Forms.ToolStrip();
            this.historyImageList = new System.Windows.Forms.ImageList(this.components);
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.filterToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.historyQueryResultDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.historyQueryResultBindingSource)).BeginInit();
            this.historyToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyQueryResultDataGridView
            // 
            this.historyQueryResultDataGridView.AllowUserToAddRows = false;
            this.historyQueryResultDataGridView.AllowUserToDeleteRows = false;
            this.historyQueryResultDataGridView.AllowUserToResizeColumns = false;
            this.historyQueryResultDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.historyQueryResultDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.historyQueryResultDataGridView.AutoGenerateColumns = false;
            this.historyQueryResultDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.historyQueryResultDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.historyQueryResultDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.historyQueryResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.historyQueryResultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.historyQueryResultDataGridView.DataSource = this.historyQueryResultBindingSource;
            this.historyQueryResultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyQueryResultDataGridView.Location = new System.Drawing.Point(0, 25);
            this.historyQueryResultDataGridView.Name = "historyQueryResultDataGridView";
            this.historyQueryResultDataGridView.ReadOnly = true;
            this.historyQueryResultDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.historyQueryResultDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.historyQueryResultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.historyQueryResultDataGridView.Size = new System.Drawing.Size(447, 338);
            this.historyQueryResultDataGridView.TabIndex = 1;
            this.historyQueryResultDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.historyQueryResultDataGridView_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Url";
            this.dataGridViewTextBoxColumn1.HeaderText = "Url";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn1.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Title";
            this.dataGridViewTextBoxColumn2.HeaderText = "Title";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "VisitTime";
            this.dataGridViewTextBoxColumn3.HeaderText = "VisitTime";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "VisitCount";
            this.dataGridViewTextBoxColumn4.HeaderText = "Visits";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // historyQueryResultBindingSource
            // 
            this.historyQueryResultBindingSource.DataSource = typeof(Awesomium.Core.HistoryEntry);
            this.historyQueryResultBindingSource.Sort = "";
            // 
            // historyToolStrip
            // 
            this.historyToolStrip.ImageList = this.historyImageList;
            this.historyToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripButton,
            this.toolStripSeparator1,
            this.filterToolStripComboBox});
            this.historyToolStrip.Location = new System.Drawing.Point(0, 0);
            this.historyToolStrip.Name = "historyToolStrip";
            this.historyToolStrip.Size = new System.Drawing.Size(447, 25);
            this.historyToolStrip.TabIndex = 2;
            // 
            // historyImageList
            // 
            this.historyImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("historyImageList.ImageStream")));
            this.historyImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.historyImageList.Images.SetKeyName(0, "development 39 grey.ico");
            this.historyImageList.Images.SetKeyName(1, "development 39.ico");
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripButton.ImageIndex = 0;
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshToolStripButton.Text = "Refresh";
            this.refreshToolStripButton.Click += new System.EventHandler(this.refreshToolStripButton_Click);
            this.refreshToolStripButton.MouseEnter += new System.EventHandler(this.refreshToolStripButton_MouseEnter);
            this.refreshToolStripButton.MouseLeave += new System.EventHandler(this.refreshToolStripButton_MouseLeave);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // filterToolStripComboBox
            // 
            this.filterToolStripComboBox.Items.AddRange(new object[] {
            "Today",
            "Last Week",
            "Last Month",
            "Full"});
            this.filterToolStripComboBox.Name = "filterToolStripComboBox";
            this.filterToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.filterToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.filterToolStripComboBox_SelectedIndexChanged);
            // 
            // HistoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(447, 363);
            this.Controls.Add(this.historyQueryResultDataGridView);
            this.Controls.Add(this.historyToolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HistoryForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
            this.Text = "History";
            ((System.ComponentModel.ISupportInitialize)(this.historyQueryResultDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.historyQueryResultBindingSource)).EndInit();
            this.historyToolStrip.ResumeLayout(false);
            this.historyToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource historyQueryResultBindingSource;
        private System.Windows.Forms.DataGridView historyQueryResultDataGridView;
        private System.Windows.Forms.DataGridViewLinkColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.ToolStrip historyToolStrip;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox filterToolStripComboBox;
        private System.Windows.Forms.ImageList historyImageList;

    }
}