namespace M4.M4v2.FormDock
{
  partial class OutputWindowV2
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindowV2));
        this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.cmnuClear = new System.Windows.Forms.ToolStripMenuItem();
        this.m_tabAlerts = new Nevron.UI.WinForm.Controls.NTabPage();
        this.GrdListAlerts = new Telerik.WinControls.UI.RadGridView();
        this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
        this.m_TabCtrl = new Nevron.UI.WinForm.Controls.NTabControl();
        this.m_tabConnection = new Nevron.UI.WinForm.Controls.NTabPage();
        this.GrdListConnection = new Telerik.WinControls.UI.RadGridView();
        this.m_ContextMenu.SuspendLayout();
        this.m_tabAlerts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.GrdListAlerts)).BeginInit();
        this.m_TabCtrl.SuspendLayout();
        this.m_tabConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.GrdListConnection)).BeginInit();
        this.SuspendLayout();
        // 
        // m_ContextMenu
        // 
        this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuClear});
        this.m_ContextMenu.Name = "m_ContextMenu";
        this.m_ContextMenu.Size = new System.Drawing.Size(102, 26);
        this.m_ContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MContextMenuOpening);
        // 
        // cmnuClear
        // 
        this.cmnuClear.Name = "cmnuClear";
        this.cmnuClear.Size = new System.Drawing.Size(101, 22);
        this.cmnuClear.Text = "Clear";
        this.cmnuClear.Click += new System.EventHandler(this.CmnuClearClick);
        // 
        // m_tabAlerts
        // 
        this.m_tabAlerts.Controls.Add(this.GrdListAlerts);
        this.m_tabAlerts.Location = new System.Drawing.Point(4, 4);
        this.m_tabAlerts.Name = "m_tabAlerts";
        this.m_tabAlerts.Size = new System.Drawing.Size(556, 293);
        this.m_tabAlerts.TabIndex = 2;
        this.m_tabAlerts.Text = "Alerts && Messages";
        // 
        // GrdListAlerts
        // 
        this.GrdListAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
        this.GrdListAlerts.Location = new System.Drawing.Point(0, 0);
        // 
        // GrdListAlerts
        // 
        this.GrdListAlerts.MasterTemplate.AllowAddNewRow = false;
        this.GrdListAlerts.MasterTemplate.AllowCellContextMenu = false;
        this.GrdListAlerts.MasterTemplate.AllowColumnChooser = false;
        this.GrdListAlerts.MasterTemplate.AllowColumnHeaderContextMenu = false;
        this.GrdListAlerts.MasterTemplate.AllowColumnReorder = false;
        this.GrdListAlerts.MasterTemplate.AllowColumnResize = false;
        this.GrdListAlerts.MasterTemplate.AllowDeleteRow = false;
        this.GrdListAlerts.MasterTemplate.AllowDragToGroup = false;
        this.GrdListAlerts.MasterTemplate.AllowRowResize = false;
        this.GrdListAlerts.MasterTemplate.ShowColumnHeaders = false;
        this.GrdListAlerts.MasterTemplate.ShowFilteringRow = false;
        this.GrdListAlerts.MasterTemplate.ShowRowHeaderColumn = false;
        this.GrdListAlerts.Name = "GrdListAlerts";
        this.GrdListAlerts.ShowCellErrors = false;
        this.GrdListAlerts.ShowGroupPanel = false;
        this.GrdListAlerts.ShowItemToolTips = false;
        this.GrdListAlerts.ShowNoDataText = false;
        this.GrdListAlerts.ShowRowErrors = false;
        this.GrdListAlerts.Size = new System.Drawing.Size(556, 293);
        this.GrdListAlerts.TabIndex = 1;
        // 
        // m_ImgList
        // 
        this.m_ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList.ImageStream")));
        this.m_ImgList.TransparentColor = System.Drawing.Color.Transparent;
        this.m_ImgList.Images.SetKeyName(0, "Icon_Chart.png");
        this.m_ImgList.Images.SetKeyName(1, "Icon_Check.png");
        this.m_ImgList.Images.SetKeyName(2, "Icon_Gear.png");
        this.m_ImgList.Images.SetKeyName(3, "Icon_Info.png");
        this.m_ImgList.Images.SetKeyName(4, "Icon_Report.png");
        this.m_ImgList.Images.SetKeyName(5, "Icon_Warning.png");
        // 
        // m_TabCtrl
        // 
        this.m_TabCtrl.Controls.Add(this.m_tabConnection);
        this.m_TabCtrl.Controls.Add(this.m_tabAlerts);
        this.m_TabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_TabCtrl.Location = new System.Drawing.Point(0, 0);
        this.m_TabCtrl.Name = "m_TabCtrl";
        this.m_TabCtrl.Padding = new System.Windows.Forms.Padding(4, 4, 4, 29);
        this.m_TabCtrl.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.m_TabCtrl.Selectable = true;
        this.m_TabCtrl.SelectedIndex = 1;
        this.m_TabCtrl.Size = new System.Drawing.Size(564, 326);
        this.m_TabCtrl.TabAlign = Nevron.UI.WinForm.Controls.TabAlign.Bottom;
        this.m_TabCtrl.TabIndex = 3;
        // 
        // m_tabConnection
        // 
        this.m_tabConnection.Controls.Add(this.GrdListConnection);
        this.m_tabConnection.Location = new System.Drawing.Point(4, 4);
        this.m_tabConnection.Name = "m_tabConnection";
        this.m_tabConnection.Size = new System.Drawing.Size(556, 293);
        this.m_tabConnection.TabIndex = 1;
        this.m_tabConnection.Text = "Connection Status";
        // 
        // GrdListConnection
        // 
        this.GrdListConnection.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.GrdListConnection.Dock = System.Windows.Forms.DockStyle.Fill;
        this.GrdListConnection.EnableCustomDrawing = true;
        this.GrdListConnection.Location = new System.Drawing.Point(0, 0);
        // 
        // GrdListConnection
        // 
        this.GrdListConnection.MasterTemplate.AllowAddNewRow = false;
        this.GrdListConnection.MasterTemplate.AllowCellContextMenu = false;
        this.GrdListConnection.MasterTemplate.AllowColumnChooser = false;
        this.GrdListConnection.MasterTemplate.AllowColumnHeaderContextMenu = false;
        this.GrdListConnection.MasterTemplate.AllowColumnReorder = false;
        this.GrdListConnection.MasterTemplate.AllowColumnResize = false;
        this.GrdListConnection.MasterTemplate.AllowDeleteRow = false;
        this.GrdListConnection.MasterTemplate.AllowDragToGroup = false;
        this.GrdListConnection.MasterTemplate.AllowRowResize = false;
        this.GrdListConnection.MasterTemplate.AutoGenerateColumns = false;
        this.GrdListConnection.MasterTemplate.EnableAlternatingRowColor = true;
        this.GrdListConnection.MasterTemplate.ShowColumnHeaders = false;
        this.GrdListConnection.MasterTemplate.ShowFilteringRow = false;
        this.GrdListConnection.MasterTemplate.ShowRowHeaderColumn = false;
        this.GrdListConnection.Name = "GrdListConnection";
        // 
        // 
        // 
        this.GrdListConnection.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
        this.GrdListConnection.ShowCellErrors = false;
        this.GrdListConnection.ShowGroupPanel = false;
        this.GrdListConnection.ShowItemToolTips = false;
        this.GrdListConnection.ShowNoDataText = false;
        this.GrdListConnection.ShowRowErrors = false;
        this.GrdListConnection.Size = new System.Drawing.Size(556, 293);
        this.GrdListConnection.TabIndex = 0;
        // 
        // OutputWindowV2
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.m_TabCtrl);
        this.Name = "OutputWindowV2";
        this.Size = new System.Drawing.Size(564, 326);
        this.Resize += new System.EventHandler(this.OutputWindowResize);
        this.m_ContextMenu.ResumeLayout(false);
        this.m_tabAlerts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.GrdListAlerts)).EndInit();
        this.m_TabCtrl.ResumeLayout(false);
        this.m_tabConnection.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.GrdListConnection)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    internal System.Windows.Forms.ToolStripMenuItem cmnuClear;
    internal Nevron.UI.WinForm.Controls.NTabPage m_tabAlerts;
    internal System.Windows.Forms.ImageList m_ImgList;
    internal Nevron.UI.WinForm.Controls.NTabControl m_TabCtrl;
    internal Nevron.UI.WinForm.Controls.NTabPage m_tabConnection;
    private Telerik.WinControls.UI.RadGridView GrdListConnection;
    private Telerik.WinControls.UI.RadGridView GrdListAlerts;
  }
}
