namespace M4.M4v2.Portfolio
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
            this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
            this.tabPageView = new Telerik.WinControls.UI.RadPageView();
            this.m_tabConnection = new Telerik.WinControls.UI.RadPageViewPage();
            this.GrdListConnection = new Telerik.WinControls.UI.RadGridView();
            this.m_tabAlerts = new Telerik.WinControls.UI.RadPageViewPage();
            this.GrdListAlerts = new Telerik.WinControls.UI.RadGridView();
            this.m_ContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPageView)).BeginInit();
            this.tabPageView.SuspendLayout();
            this.m_tabConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdListConnection)).BeginInit();
            this.m_tabAlerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdListAlerts)).BeginInit();
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
            // tabPageView
            // 
            this.tabPageView.Controls.Add(this.m_tabConnection);
            this.tabPageView.Controls.Add(this.m_tabAlerts);
            this.tabPageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPageView.Location = new System.Drawing.Point(0, 0);
            this.tabPageView.Name = "tabPageView";
            this.tabPageView.SelectedPage = this.m_tabConnection;
            this.tabPageView.Size = new System.Drawing.Size(564, 326);
            this.tabPageView.TabIndex = 1;
            this.tabPageView.Text = "radPageView1";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.tabPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.tabPageView.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Bottom;
            // 
            // m_tabConnection
            // 
            this.m_tabConnection.Controls.Add(this.GrdListConnection);
            this.m_tabConnection.Location = new System.Drawing.Point(10, 10);
            this.m_tabConnection.Name = "m_tabConnection";
            this.m_tabConnection.Size = new System.Drawing.Size(543, 278);
            this.m_tabConnection.Text = "Conexão";
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
            this.GrdListConnection.ShowCellErrors = false;
            this.GrdListConnection.ShowGroupPanel = false;
            this.GrdListConnection.ShowItemToolTips = false;
            this.GrdListConnection.ShowNoDataText = false;
            this.GrdListConnection.ShowRowErrors = false;
            this.GrdListConnection.Size = new System.Drawing.Size(543, 278);
            this.GrdListConnection.TabIndex = 1;
            // 
            // m_tabAlerts
            // 
            this.m_tabAlerts.Controls.Add(this.GrdListAlerts);
            this.m_tabAlerts.Location = new System.Drawing.Point(4, 4);
            this.m_tabAlerts.Name = "m_tabAlerts";
            this.m_tabAlerts.Size = new System.Drawing.Size(0, 0);
            this.m_tabAlerts.Text = "Alertas & Mensagens";
            // 
            // GrdListAlerts
            // 
            this.GrdListAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrdListAlerts.Location = new System.Drawing.Point(0, 0);
            // 
            // 
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
            this.GrdListAlerts.Size = new System.Drawing.Size(0, 0);
            this.GrdListAlerts.TabIndex = 2;
            // 
            // OutputWindowV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabPageView);
            this.Name = "OutputWindowV2";
            this.Size = new System.Drawing.Size(564, 326);
            this.Load += new System.EventHandler(this.OutputWindowV2Load);
            this.Resize += new System.EventHandler(this.OutputWindowResize);
            this.m_ContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPageView)).EndInit();
            this.tabPageView.ResumeLayout(false);
            this.m_tabConnection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdListConnection)).EndInit();
            this.m_tabAlerts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdListAlerts)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    internal System.Windows.Forms.ToolStripMenuItem cmnuClear;
    internal System.Windows.Forms.ImageList m_ImgList;
    private Telerik.WinControls.UI.RadPageView tabPageView;
    private Telerik.WinControls.UI.RadPageViewPage m_tabAlerts;
    private Telerik.WinControls.UI.RadPageViewPage m_tabConnection;
    private Telerik.WinControls.UI.RadGridView GrdListAlerts;
    private Telerik.WinControls.UI.RadGridView GrdListConnection;
  }
}
