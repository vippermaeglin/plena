namespace M4
{
  partial class ctlBacktest
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlBacktest));
        this.cmdDocumentation = new Nevron.UI.WinForm.Controls.NButton();
        this.Label6 = new System.Windows.Forms.Label();
        this.txtInterval = new Nevron.UI.WinForm.Controls.NTextBox();
        this.Label7 = new System.Windows.Forms.Label();
        this.cboPeriodicity = new Nevron.UI.WinForm.Controls.NComboBox();
        this.tpBuy = new Nevron.UI.WinForm.Controls.NTabPage();
        this.txtBuyScript = new Nevron.UI.WinForm.Controls.NTextBox();
        this.tpSell = new Nevron.UI.WinForm.Controls.NTabPage();
        this.txtSellScript = new Nevron.UI.WinForm.Controls.NTextBox();
        this.txtExitLongScript = new Nevron.UI.WinForm.Controls.NTextBox();
        this.tpExitShort = new Nevron.UI.WinForm.Controls.NTabPage();
        this.txtExitShortScript = new Nevron.UI.WinForm.Controls.NTextBox();
        this.tpExitLong = new Nevron.UI.WinForm.Controls.NTabPage();
        this.tabScripts = new Nevron.UI.WinForm.Controls.NTabControl();
        this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
        this.txtBars = new Nevron.UI.WinForm.Controls.NTextBox();
        this.m_ListTrades = new System.Windows.Forms.ListView();
        this.Trades = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.grpTrades = new Nevron.UI.WinForm.Controls.NGrouper();
        this.cmdBacktest = new Nevron.UI.WinForm.Controls.NButton();
        this.Label8 = new System.Windows.Forms.Label();
        this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
        this.Label9 = new System.Windows.Forms.Label();
        this.grpData = new Nevron.UI.WinForm.Controls.NGrouper();
        this.txtSymbol = new Nevron.UI.WinForm.Controls.NTextBox();
        this.tpBuy.SuspendLayout();
        this.tpSell.SuspendLayout();
        this.tpExitShort.SuspendLayout();
        this.tpExitLong.SuspendLayout();
        this.tabScripts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grpTrades)).BeginInit();
        this.grpTrades.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grpData)).BeginInit();
        this.grpData.SuspendLayout();
        this.SuspendLayout();
        // 
        // cmdDocumentation
        // 
        this.cmdDocumentation.Location = new System.Drawing.Point(436, 366);
        this.cmdDocumentation.Name = "cmdDocumentation";
        this.cmdDocumentation.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.cmdDocumentation.Size = new System.Drawing.Size(97, 23);
        this.cmdDocumentation.TabIndex = 143;
        this.cmdDocumentation.Text = "&Script Guide";
        this.cmdDocumentation.UseVisualStyleBackColor = false;
        this.cmdDocumentation.Click += new System.EventHandler(this.CmdDocumentationClick);
        // 
        // Label6
        // 
        this.Label6.AutoSize = true;
        this.Label6.BackColor = System.Drawing.Color.Transparent;
        this.Label6.Location = new System.Drawing.Point(167, 42);
        this.Label6.Name = "Label6";
        this.Label6.Size = new System.Drawing.Size(61, 13);
        this.Label6.TabIndex = 109;
        this.Label6.Text = "Bar Interval";
        // 
        // txtInterval
        // 
        this.txtInterval.Location = new System.Drawing.Point(235, 40);
        this.txtInterval.Name = "txtInterval";
        this.txtInterval.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.txtInterval.Size = new System.Drawing.Size(76, 18);
        this.txtInterval.TabIndex = 2;
        // 
        // Label7
        // 
        this.Label7.AutoSize = true;
        this.Label7.BackColor = System.Drawing.Color.Transparent;
        this.Label7.Location = new System.Drawing.Point(13, 76);
        this.Label7.Name = "Label7";
        this.Label7.Size = new System.Drawing.Size(55, 13);
        this.Label7.TabIndex = 108;
        this.Label7.Text = "Periodicity";
        // 
        // cboPeriodicity
        // 
        this.cboPeriodicity.Items.AddRange(new object[] {
            new Nevron.UI.WinForm.Controls.NListBoxItem("Minute", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Hour", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Day", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Week", -1, false, 0, new System.Drawing.Size(0, 0))});
        this.cboPeriodicity.ListProperties.ColumnOnLeft = false;
        this.cboPeriodicity.Location = new System.Drawing.Point(75, 72);
        this.cboPeriodicity.Name = "cboPeriodicity";
        this.cboPeriodicity.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.cboPeriodicity.Size = new System.Drawing.Size(79, 20);
        this.cboPeriodicity.TabIndex = 3;
        // 
        // tpBuy
        // 
        this.tpBuy.Controls.Add(this.txtBuyScript);
        this.tpBuy.ImageIndex = 0;
        this.tpBuy.Location = new System.Drawing.Point(4, 29);
        this.tpBuy.Name = "tpBuy";
        this.tpBuy.Size = new System.Drawing.Size(623, 185);
        this.tpBuy.TabIndex = 1;
        this.tpBuy.Text = "Buy Script";
        // 
        // txtBuyScript
        // 
        this.txtBuyScript.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtBuyScript.Location = new System.Drawing.Point(0, 0);
        this.txtBuyScript.Multiline = true;
        this.txtBuyScript.Name = "txtBuyScript";
        this.txtBuyScript.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.WindowsDefault;
        this.txtBuyScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtBuyScript.Size = new System.Drawing.Size(623, 185);
        this.txtBuyScript.TabIndex = 8;
        // 
        // tpSell
        // 
        this.tpSell.Controls.Add(this.txtSellScript);
        this.tpSell.ImageIndex = 1;
        this.tpSell.Location = new System.Drawing.Point(4, 29);
        this.tpSell.Name = "tpSell";
        this.tpSell.Size = new System.Drawing.Size(623, 185);
        this.tpSell.TabIndex = 2;
        this.tpSell.Text = "Sell Script";
        // 
        // txtSellScript
        // 
        this.txtSellScript.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtSellScript.Location = new System.Drawing.Point(0, 0);
        this.txtSellScript.Multiline = true;
        this.txtSellScript.Name = "txtSellScript";
        this.txtSellScript.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.WindowsDefault;
        this.txtSellScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtSellScript.Size = new System.Drawing.Size(623, 185);
        this.txtSellScript.TabIndex = 9;
        // 
        // txtExitLongScript
        // 
        this.txtExitLongScript.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtExitLongScript.Location = new System.Drawing.Point(0, 0);
        this.txtExitLongScript.Multiline = true;
        this.txtExitLongScript.Name = "txtExitLongScript";
        this.txtExitLongScript.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.WindowsDefault;
        this.txtExitLongScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtExitLongScript.Size = new System.Drawing.Size(623, 185);
        this.txtExitLongScript.TabIndex = 10;
        // 
        // tpExitShort
        // 
        this.tpExitShort.Controls.Add(this.txtExitShortScript);
        this.tpExitShort.ImageIndex = 3;
        this.tpExitShort.Location = new System.Drawing.Point(4, 29);
        this.tpExitShort.Name = "tpExitShort";
        this.tpExitShort.Size = new System.Drawing.Size(623, 185);
        this.tpExitShort.TabIndex = 4;
        this.tpExitShort.Text = "Exit Short Script";
        // 
        // txtExitShortScript
        // 
        this.txtExitShortScript.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtExitShortScript.Location = new System.Drawing.Point(0, 0);
        this.txtExitShortScript.Multiline = true;
        this.txtExitShortScript.Name = "txtExitShortScript";
        this.txtExitShortScript.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.WindowsDefault;
        this.txtExitShortScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtExitShortScript.Size = new System.Drawing.Size(623, 185);
        this.txtExitShortScript.TabIndex = 11;
        // 
        // tpExitLong
        // 
        this.tpExitLong.Controls.Add(this.txtExitLongScript);
        this.tpExitLong.ImageIndex = 2;
        this.tpExitLong.Location = new System.Drawing.Point(4, 29);
        this.tpExitLong.Name = "tpExitLong";
        this.tpExitLong.Size = new System.Drawing.Size(623, 185);
        this.tpExitLong.TabIndex = 3;
        this.tpExitLong.Text = "Exit Long Script";
        // 
        // tabScripts
        // 
        this.tabScripts.Controls.Add(this.tpBuy);
        this.tabScripts.Controls.Add(this.tpSell);
        this.tabScripts.Controls.Add(this.tpExitLong);
        this.tabScripts.Controls.Add(this.tpExitShort);
        this.tabScripts.ImageList = this.m_ImgList;
        this.tabScripts.Location = new System.Drawing.Point(9, 134);
        this.tabScripts.Name = "tabScripts";
        this.tabScripts.Padding = new System.Windows.Forms.Padding(4, 29, 4, 4);
        this.tabScripts.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.WindowsDefault;
        this.tabScripts.Selectable = true;
        this.tabScripts.SelectedIndex = 0;
        this.tabScripts.Size = new System.Drawing.Size(631, 218);
        this.tabScripts.TabIndex = 147;
        // 
        // m_ImgList
        // 
        this.m_ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList.ImageStream")));
        this.m_ImgList.TransparentColor = System.Drawing.Color.Fuchsia;
        this.m_ImgList.Images.SetKeyName(0, "");
        this.m_ImgList.Images.SetKeyName(1, "");
        this.m_ImgList.Images.SetKeyName(2, "");
        this.m_ImgList.Images.SetKeyName(3, "");
        // 
        // txtBars
        // 
        this.txtBars.Location = new System.Drawing.Point(235, 72);
        this.txtBars.Name = "txtBars";
        this.txtBars.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.txtBars.Size = new System.Drawing.Size(76, 18);
        this.txtBars.TabIndex = 4;
        // 
        // m_ListTrades
        // 
        this.m_ListTrades.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
        this.m_ListTrades.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.m_ListTrades.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Trades});
        this.m_ListTrades.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_ListTrades.ForeColor = System.Drawing.SystemColors.WindowText;
        this.m_ListTrades.FullRowSelect = true;
        this.m_ListTrades.GridLines = true;
        this.m_ListTrades.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.m_ListTrades.Location = new System.Drawing.Point(1, 22);
        this.m_ListTrades.MultiSelect = false;
        this.m_ListTrades.Name = "m_ListTrades";
        this.m_ListTrades.Size = new System.Drawing.Size(334, 557);
        this.m_ListTrades.TabIndex = 8;
        this.m_ListTrades.UseCompatibleStateImageBehavior = false;
        this.m_ListTrades.View = System.Windows.Forms.View.Details;
        // 
        // grpTrades
        // 
        this.grpTrades.Controls.Add(this.m_ListTrades);
        this.grpTrades.FooterItem.ContentAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.grpTrades.FooterItem.Text = "Footer";
        this.grpTrades.FooterItem.Visible = false;
        this.grpTrades.HeaderItem.Text = "Trades";
        this.grpTrades.Location = new System.Drawing.Point(647, 9);
        this.grpTrades.Name = "grpTrades";
        this.grpTrades.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.grpTrades.Size = new System.Drawing.Size(341, 585);
        this.grpTrades.TabIndex = 146;
        this.grpTrades.Text = "Trades";
        // 
        // cmdBacktest
        // 
        this.cmdBacktest.Border.BaseColor = System.Drawing.Color.LimeGreen;
        this.cmdBacktest.Border.Style = Nevron.UI.BorderStyle3D.Flat;
        this.cmdBacktest.Location = new System.Drawing.Point(543, 366);
        this.cmdBacktest.Name = "cmdBacktest";
        this.cmdBacktest.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.cmdBacktest.Size = new System.Drawing.Size(97, 23);
        this.cmdBacktest.TabIndex = 144;
        this.cmdBacktest.Text = "&Back Test";
        this.cmdBacktest.UseVisualStyleBackColor = false;
        this.cmdBacktest.Click += new System.EventHandler(this.CmdBacktestClick);
        // 
        // Label8
        // 
        this.Label8.AutoSize = true;
        this.Label8.BackColor = System.Drawing.Color.Transparent;
        this.Label8.Location = new System.Drawing.Point(170, 75);
        this.Label8.Name = "Label8";
        this.Label8.Size = new System.Drawing.Size(58, 13);
        this.Label8.TabIndex = 107;
        this.Label8.Text = "Bar History";
        // 
        // tmrUpdate
        // 
        this.tmrUpdate.Interval = 500;
        // 
        // Label9
        // 
        this.Label9.AutoSize = true;
        this.Label9.BackColor = System.Drawing.Color.Transparent;
        this.Label9.Location = new System.Drawing.Point(26, 42);
        this.Label9.Name = "Label9";
        this.Label9.Size = new System.Drawing.Size(41, 13);
        this.Label9.TabIndex = 106;
        this.Label9.Text = "Symbol";
        // 
        // grpData
        // 
        this.grpData.Controls.Add(this.Label6);
        this.grpData.Controls.Add(this.txtInterval);
        this.grpData.Controls.Add(this.Label7);
        this.grpData.Controls.Add(this.cboPeriodicity);
        this.grpData.Controls.Add(this.Label8);
        this.grpData.Controls.Add(this.txtBars);
        this.grpData.Controls.Add(this.Label9);
        this.grpData.Controls.Add(this.txtSymbol);
        this.grpData.FooterItem.ContentAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.grpData.FooterItem.Text = "Footer";
        this.grpData.FooterItem.Visible = false;
        this.grpData.HeaderItem.Text = "Data Source";
        this.grpData.Location = new System.Drawing.Point(9, 9);
        this.grpData.Name = "grpData";
        this.grpData.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.grpData.Size = new System.Drawing.Size(338, 119);
        this.grpData.TabIndex = 145;
        this.grpData.Text = "Data Source";
        // 
        // txtSymbol
        // 
        this.txtSymbol.Location = new System.Drawing.Point(75, 40);
        this.txtSymbol.Name = "txtSymbol";
        this.txtSymbol.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.Standard;
        this.txtSymbol.Size = new System.Drawing.Size(79, 18);
        this.txtSymbol.TabIndex = 0;
        // 
        // ctlBacktest
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.cmdDocumentation);
        this.Controls.Add(this.tabScripts);
        this.Controls.Add(this.grpTrades);
        this.Controls.Add(this.cmdBacktest);
        this.Controls.Add(this.grpData);
        this.Name = "ctlBacktest";
        this.Size = new System.Drawing.Size(997, 602);
        this.Load += new System.EventHandler(this.CtlBacktestLoad);
        this.Resize += new System.EventHandler(this.CtlBacktestResize);
        this.tpBuy.ResumeLayout(false);
        this.tpSell.ResumeLayout(false);
        this.tpExitShort.ResumeLayout(false);
        this.tpExitLong.ResumeLayout(false);
        this.tabScripts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grpTrades)).EndInit();
        this.grpTrades.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grpData)).EndInit();
        this.grpData.ResumeLayout(false);
        this.grpData.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NButton cmdDocumentation;
    internal System.Windows.Forms.Label Label6;
    internal Nevron.UI.WinForm.Controls.NTextBox txtInterval;
    internal System.Windows.Forms.Label Label7;
    internal Nevron.UI.WinForm.Controls.NComboBox cboPeriodicity;
    internal Nevron.UI.WinForm.Controls.NTabPage tpBuy;
    internal Nevron.UI.WinForm.Controls.NTextBox txtBuyScript;
    internal Nevron.UI.WinForm.Controls.NTabPage tpSell;
    internal Nevron.UI.WinForm.Controls.NTextBox txtSellScript;
    internal Nevron.UI.WinForm.Controls.NTextBox txtExitLongScript;
    internal Nevron.UI.WinForm.Controls.NTabPage tpExitShort;
    internal Nevron.UI.WinForm.Controls.NTextBox txtExitShortScript;
    internal Nevron.UI.WinForm.Controls.NTabPage tpExitLong;
    internal Nevron.UI.WinForm.Controls.NTabControl tabScripts;
    internal System.Windows.Forms.ImageList m_ImgList;
    internal Nevron.UI.WinForm.Controls.NTextBox txtBars;
    internal System.Windows.Forms.ListView m_ListTrades;
    internal System.Windows.Forms.ColumnHeader Trades;
    internal Nevron.UI.WinForm.Controls.NGrouper grpTrades;
    internal Nevron.UI.WinForm.Controls.NButton cmdBacktest;
    internal System.Windows.Forms.Label Label8;
    internal System.Windows.Forms.Timer tmrUpdate;
    internal System.Windows.Forms.Label Label9;
    internal Nevron.UI.WinForm.Controls.NGrouper grpData;
    internal Nevron.UI.WinForm.Controls.NTextBox txtSymbol;
  }
}
