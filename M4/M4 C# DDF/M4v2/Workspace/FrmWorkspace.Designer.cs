namespace M4.M4v2.Workspace
{
    partial class FrmWorkspace
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWorkspace));
            this.pnlAction = new Telerik.WinControls.UI.RadPanel();
            this.btnNew = new Telerik.WinControls.UI.RadButton();
            this.btnRemove = new Telerik.WinControls.UI.RadButton();
            this.btnDismiss = new Telerik.WinControls.UI.RadButton();
            this.btnApply = new Telerik.WinControls.UI.RadButton();
            this.trvWorkspace = new Telerik.WinControls.UI.RadTreeView();
            this.listImage = new System.Windows.Forms.ImageList(this.components);
            this.cmnuWorkspace = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.mnuDefaultWorkspace = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuLoadWorkspace = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuRenameWorkspace = new Telerik.WinControls.UI.RadMenuItem();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAction)).BeginInit();
            this.pnlAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.btnNew);
            this.pnlAction.Controls.Add(this.btnRemove);
            this.pnlAction.Controls.Add(this.btnDismiss);
            this.pnlAction.Controls.Add(this.btnApply);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAction.Location = new System.Drawing.Point(0, 182);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(222, 30);
            this.pnlAction.TabIndex = 25;
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNew.Location = new System.Drawing.Point(3, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(24, 24);
            this.btnNew.TabIndex = 26;
            this.btnNew.Click += new System.EventHandler(this.BtnNewClick);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemove.Location = new System.Drawing.Point(63, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 27;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // btnDismiss
            // 
            this.btnDismiss.Location = new System.Drawing.Point(129, 3);
            this.btnDismiss.Name = "btnDismiss";
            this.btnDismiss.Size = new System.Drawing.Size(83, 24);
            this.btnDismiss.TabIndex = 20;
            this.btnDismiss.Text = "Dismiss";
            this.btnDismiss.Click += new System.EventHandler(this.BtnDismissClick);
            // 
            // btnApply
            // 
            this.btnApply.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnApply.Enabled = false;
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply.Location = new System.Drawing.Point(33, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(24, 24);
            this.btnApply.TabIndex = 25;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // trvWorkspace
            // 
            this.trvWorkspace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.trvWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvWorkspace.ImageList = this.listImage;
            this.trvWorkspace.Location = new System.Drawing.Point(0, 0);
            this.trvWorkspace.Name = "trvWorkspace";
            this.trvWorkspace.RadContextMenu = this.cmnuWorkspace;
            this.trvWorkspace.ShowDragHint = false;
            this.trvWorkspace.ShowDropHint = false;
            this.trvWorkspace.ShowLines = true;
            this.trvWorkspace.ShowNodeToolTips = true;
            this.trvWorkspace.Size = new System.Drawing.Size(222, 182);
            this.trvWorkspace.SpacingBetweenNodes = -1;
            this.trvWorkspace.TabIndex = 24;
            this.trvWorkspace.Text = "radTreeView1";
            this.trvWorkspace.DragEnded += new Telerik.WinControls.UI.RadTreeView.DragEndedHandler(this.TrvWorkspaceDragEnded);
            this.trvWorkspace.DragOverNode += new System.EventHandler<Telerik.WinControls.UI.RadTreeViewDragCancelEventArgs>(this.TrvWorkspaceDragOverNode);
            this.trvWorkspace.SelectedNodeChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.trvWorkspace_SelectedNodeChanging);
            this.trvWorkspace.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.TrvWorkspaceSelectedNodeChanged);
            this.trvWorkspace.DoubleClick += new System.EventHandler(this.TrvWorkspaceDoubleClick);
            // 
            // listImage
            // 
            this.listImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listImage.ImageStream")));
            this.listImage.TransparentColor = System.Drawing.Color.Transparent;
            this.listImage.Images.SetKeyName(0, "Select.png");
            // 
            // cmnuWorkspace
            // 
            this.cmnuWorkspace.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuDefaultWorkspace,
            this.mnuLoadWorkspace,
            this.mnuRenameWorkspace});
            this.cmnuWorkspace.DropDownOpening += new System.ComponentModel.CancelEventHandler(this.CmnuWorkspaceDropDownOpening);
            this.cmnuWorkspace.DropDownClosed += new System.EventHandler(this.CmnuWorkspaceDropDownClosed);
            // 
            // mnuDefaultWorkspace
            // 
            this.mnuDefaultWorkspace.AccessibleDescription = "Default";
            this.mnuDefaultWorkspace.AccessibleName = "Default";
            this.mnuDefaultWorkspace.Name = "mnuDefaultWorkspace";
            this.mnuDefaultWorkspace.Text = "Default";
            this.mnuDefaultWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // mnuLoadWorkspace
            // 
            this.mnuLoadWorkspace.AccessibleDescription = "Load";
            this.mnuLoadWorkspace.AccessibleName = "Load";
            this.mnuLoadWorkspace.Name = "mnuLoadWorkspace";
            this.mnuLoadWorkspace.Text = "Load";
            this.mnuLoadWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuRenameWorkspace
            // 
            this.mnuRenameWorkspace.AccessibleDescription = "Rename";
            this.mnuRenameWorkspace.AccessibleName = "Rename";
            this.mnuRenameWorkspace.Name = "mnuRenameWorkspace";
            this.mnuRenameWorkspace.Text = "Rename";
            this.mnuRenameWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.radPanel1.Controls.Add(this.trvWorkspace);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(222, 182);
            this.radPanel1.TabIndex = 26;
            // 
            // FrmWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 212);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.pnlAction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWorkspace";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workspace";
            this.ThemeName = "ControlDefault";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWorkspaceFormClosing);
            this.Load += new System.EventHandler(this.FrmWorkspace_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlAction)).EndInit();
            this.pnlAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel pnlAction;
        private Telerik.WinControls.UI.RadButton btnNew;
        private Telerik.WinControls.UI.RadButton btnRemove;
        private Telerik.WinControls.UI.RadButton btnDismiss;
        private Telerik.WinControls.UI.RadButton btnApply;
        private Telerik.WinControls.UI.RadTreeView trvWorkspace;
        private System.Windows.Forms.ImageList listImage;
        private Telerik.WinControls.UI.RadContextMenu cmnuWorkspace;
        private Telerik.WinControls.UI.RadMenuItem mnuDefaultWorkspace;
        private Telerik.WinControls.UI.RadMenuItem mnuLoadWorkspace;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadMenuItem mnuRenameWorkspace;

    }
}
