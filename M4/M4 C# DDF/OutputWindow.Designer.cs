namespace M4
{
  partial class OutputWindow
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindow));
        this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.cmnuClear = new System.Windows.Forms.ToolStripMenuItem();
        this.m_tabAlerts = new System.Windows.Forms.TabPage();
        this.m_ListAlerts = new System.Windows.Forms.ListView();
        this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
        this.m_TabCtrl = new System.Windows.Forms.TabControl();
        this.m_tabConnection = new System.Windows.Forms.TabPage();
        this.clnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.clnInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.m_ListConnection = new System.Windows.Forms.ListView();
        this.m_ContextMenu.SuspendLayout();
        this.m_tabAlerts.SuspendLayout();
        this.m_TabCtrl.SuspendLayout();
        this.m_tabConnection.SuspendLayout();
        this.SuspendLayout();
        // 
        // m_ContextMenu
        // 
        this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuClear});
        this.m_ContextMenu.Name = "m_ContextMenu";
        this.m_ContextMenu.Size = new System.Drawing.Size(102, 26);
        this.m_ContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.m_ContextMenu_Opening);
        // 
        // cmnuClear
        // 
        this.cmnuClear.Name = "cmnuClear";
        this.cmnuClear.Size = new System.Drawing.Size(101, 22);
        this.cmnuClear.Text = "Clear";
        this.cmnuClear.Click += new System.EventHandler(this.cmnuClear_Click);
        // 
        // m_tabAlerts
        // 
        this.m_tabAlerts.Controls.Add(this.m_ListAlerts);
        this.m_tabAlerts.Location = new System.Drawing.Point(4, 4);
        this.m_tabAlerts.Name = "m_tabAlerts";
        this.m_tabAlerts.Size = new System.Drawing.Size(556, 293);
        this.m_tabAlerts.TabIndex = 2;
        this.m_tabAlerts.Text = "Alerts & Messages";
        // 
        // m_ListAlerts
        // 
        this.m_ListAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        this.m_ListAlerts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2});
        this.m_ListAlerts.ContextMenuStrip = this.m_ContextMenu;
        this.m_ListAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_ListAlerts.ForeColor = System.Drawing.SystemColors.WindowText;
        this.m_ListAlerts.FullRowSelect = true;
        this.m_ListAlerts.GridLines = true;
        this.m_ListAlerts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.m_ListAlerts.Location = new System.Drawing.Point(0, 0);
        this.m_ListAlerts.MultiSelect = false;
        this.m_ListAlerts.Name = "m_ListAlerts";
        this.m_ListAlerts.Size = new System.Drawing.Size(556, 293);
        this.m_ListAlerts.SmallImageList = this.m_ImgList;
        this.m_ListAlerts.TabIndex = 1;
        this.m_ListAlerts.UseCompatibleStateImageBehavior = false;
        this.m_ListAlerts.View = System.Windows.Forms.View.Details;
        // 
        // ColumnHeader1
        // 
        this.ColumnHeader1.Text = "Time";
        this.ColumnHeader1.Width = 100;
        // 
        // ColumnHeader2
        // 
        this.ColumnHeader2.Text = "Info";
        this.ColumnHeader2.Width = 221;
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
        this.m_TabCtrl.SelectedIndex = 1;
        this.m_TabCtrl.Size = new System.Drawing.Size(564, 326);
        this.m_TabCtrl.TabIndex = 3;
        // 
        // m_tabConnection
        // 
        this.m_tabConnection.Controls.Add(this.m_ListConnection);
        this.m_tabConnection.Location = new System.Drawing.Point(4, 4);
        this.m_tabConnection.Name = "m_tabConnection";
        this.m_tabConnection.Size = new System.Drawing.Size(556, 293);
        this.m_tabConnection.TabIndex = 1;
        this.m_tabConnection.Text = "Connection Status";
        // 
        // clnTime
        // 
        this.clnTime.Text = "";
        this.clnTime.Width = 100;
        // 
        // clnInfo
        // 
        this.clnInfo.Text = "";
        this.clnInfo.Width = 221;
        // 
        // m_ListConnection
        // 
        this.m_ListConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        this.m_ListConnection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnTime,
            this.clnInfo});
        this.m_ListConnection.ContextMenuStrip = this.m_ContextMenu;
        this.m_ListConnection.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_ListConnection.ForeColor = System.Drawing.SystemColors.WindowText;
        this.m_ListConnection.FullRowSelect = true;
        this.m_ListConnection.GridLines = true;
        this.m_ListConnection.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.m_ListConnection.Location = new System.Drawing.Point(0, 0);
        this.m_ListConnection.MultiSelect = false;
        this.m_ListConnection.Name = "m_ListConnection";
        this.m_ListConnection.Size = new System.Drawing.Size(556, 293);
        this.m_ListConnection.SmallImageList = this.m_ImgList;
        this.m_ListConnection.TabIndex = 0;
        this.m_ListConnection.UseCompatibleStateImageBehavior = false;
        this.m_ListConnection.View = System.Windows.Forms.View.Details;
        // 
        // OutputWindow
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.m_TabCtrl);
        this.Name = "OutputWindow";
        this.Size = new System.Drawing.Size(564, 326);
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.OutputWindow_Paint);
        this.Resize += new System.EventHandler(this.OutputWindow_Resize);
        this.m_ContextMenu.ResumeLayout(false);
        this.m_tabAlerts.ResumeLayout(false);
        this.m_TabCtrl.ResumeLayout(false);
        this.m_tabConnection.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.ContextMenuStrip m_ContextMenu;
    internal System.Windows.Forms.ToolStripMenuItem cmnuClear;
    internal System.Windows.Forms.TabPage m_tabAlerts;
    internal System.Windows.Forms.ListView m_ListAlerts;
    internal System.Windows.Forms.ColumnHeader ColumnHeader1;
    internal System.Windows.Forms.ColumnHeader ColumnHeader2;
    internal System.Windows.Forms.ImageList m_ImgList;
    internal System.Windows.Forms.TabControl m_TabCtrl;
    internal System.Windows.Forms.TabPage m_tabConnection;
    internal System.Windows.Forms.ListView m_ListConnection;
    internal System.Windows.Forms.ColumnHeader clnTime;
    internal System.Windows.Forms.ColumnHeader clnInfo;
  }
}
