namespace M4.M4v2.BackTest.ORIGINAL
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
      this.cmdDocumentation = new System.Windows.Forms.Button();
      this.lblInterval = new System.Windows.Forms.Label();
      this.txtInterval = new System.Windows.Forms.TextBox();
      this.lblPeriodicity = new System.Windows.Forms.Label();
      this.cboPeriodicity = new System.Windows.Forms.ComboBox();
      this.tpBuy = new System.Windows.Forms.TabPage();
      this.txtBuyScript = new System.Windows.Forms.TextBox();
      this.tpSell = new System.Windows.Forms.TabPage();
      this.txtSellScript = new System.Windows.Forms.TextBox();
      this.txtExitLongScript = new System.Windows.Forms.TextBox();
      this.tpExitShort = new System.Windows.Forms.TabPage();
      this.txtExitShortScript = new System.Windows.Forms.TextBox();
      this.tpExitLong = new System.Windows.Forms.TabPage();
      this.tabScripts = new System.Windows.Forms.TabControl();
      this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
      this.txtBars = new System.Windows.Forms.TextBox();
      this.m_ListTrades = new System.Windows.Forms.ListView();
      this.Trades = new System.Windows.Forms.ColumnHeader();
      this.grpTrades = new System.Windows.Forms.GroupBox();
      this.cmdBacktest = new System.Windows.Forms.Button();
      this.lblBars = new System.Windows.Forms.Label();
      this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
      this.lblSymbol = new System.Windows.Forms.Label();
      this.grpData = new System.Windows.Forms.GroupBox();
      this.txtSymbol = new System.Windows.Forms.TextBox();
      this.tpBuy.SuspendLayout();
      this.tpSell.SuspendLayout();
      this.tpExitShort.SuspendLayout();
      this.tpExitLong.SuspendLayout();
      this.tabScripts.SuspendLayout();
      this.grpTrades.SuspendLayout();
      this.grpData.SuspendLayout();
      this.SuspendLayout();
      // 
      // cmdDocumentation
      // 
      this.cmdDocumentation.Location = new System.Drawing.Point(436, 366);
      this.cmdDocumentation.Name = "cmdDocumentation";
      this.cmdDocumentation.Size = new System.Drawing.Size(97, 23);
      this.cmdDocumentation.TabIndex = 143;
      this.cmdDocumentation.Text = "&Script Guide";
      this.cmdDocumentation.UseVisualStyleBackColor = false;
      this.cmdDocumentation.Click += new System.EventHandler(this.cmdDocumentation_Click);
      // 
      // Label6
      // 
      this.lblInterval.AutoSize = true;
      this.lblInterval.BackColor = System.Drawing.Color.Transparent;
      this.lblInterval.Location = new System.Drawing.Point(167, 42);
      this.lblInterval.Name = "Label6";
      this.lblInterval.Size = new System.Drawing.Size(61, 13);
      this.lblInterval.TabIndex = 109;
      this.lblInterval.Text = "Bar Interval";
      // 
      // txtInterval
      // 
      this.txtInterval.Location = new System.Drawing.Point(235, 40);
      this.txtInterval.Name = "txtInterval";
      this.txtInterval.Size = new System.Drawing.Size(76, 18);
      this.txtInterval.TabIndex = 2;
      // 
      // Label7
      // 
      this.lblPeriodicity.AutoSize = true;
      this.lblPeriodicity.BackColor = System.Drawing.Color.Transparent;
      this.lblPeriodicity.Location = new System.Drawing.Point(13, 76);
      this.lblPeriodicity.Name = "Label7";
      this.lblPeriodicity.Size = new System.Drawing.Size(55, 13);
      this.lblPeriodicity.TabIndex = 108;
      this.lblPeriodicity.Text = "Periodicity";
      // 
      // cboPeriodicity
      // 
      this.cboPeriodicity.Items.AddRange(new object[] {"Minute","Hour", "Day", "Week"});
      this.cboPeriodicity.Location = new System.Drawing.Point(75, 72);
      this.cboPeriodicity.Name = "cboPeriodicity";
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
      this.grpTrades.Location = new System.Drawing.Point(647, 9);
      this.grpTrades.Name = "grpTrades";
      this.grpTrades.Size = new System.Drawing.Size(341, 585);
      this.grpTrades.TabIndex = 146;
      this.grpTrades.Text = "Trades";
      // 
      // cmdBacktest
      // 
      this.cmdBacktest.Location = new System.Drawing.Point(543, 366);
      this.cmdBacktest.Name = "cmdBacktest";
      this.cmdBacktest.Size = new System.Drawing.Size(97, 23);
      this.cmdBacktest.TabIndex = 144;
      this.cmdBacktest.Text = "&Back Test";
      this.cmdBacktest.UseVisualStyleBackColor = false;
      this.cmdBacktest.Click += new System.EventHandler(this.cmdBacktest_Click);
      // 
      // Label8
      // 
      this.lblBars.AutoSize = true;
      this.lblBars.BackColor = System.Drawing.Color.Transparent;
      this.lblBars.Location = new System.Drawing.Point(170, 75);
      this.lblBars.Name = "Label8";
      this.lblBars.Size = new System.Drawing.Size(58, 13);
      this.lblBars.TabIndex = 107;
      this.lblBars.Text = "Bar History";
      // 
      // tmrUpdate
      // 
      this.tmrUpdate.Interval = 500;
      // 
      // Label9
      // 
      this.lblSymbol.AutoSize = true;
      this.lblSymbol.BackColor = System.Drawing.Color.Transparent;
      this.lblSymbol.Location = new System.Drawing.Point(26, 42);
      this.lblSymbol.Name = "Label9";
      this.lblSymbol.Size = new System.Drawing.Size(41, 13);
      this.lblSymbol.TabIndex = 106;
      this.lblSymbol.Text = "Symbol";
      // 
      // grpData
      // 
      this.grpData.Controls.Add(this.lblInterval);
      this.grpData.Controls.Add(this.txtInterval);
      this.grpData.Controls.Add(this.lblPeriodicity);
      this.grpData.Controls.Add(this.cboPeriodicity);
      this.grpData.Controls.Add(this.lblBars);
      this.grpData.Controls.Add(this.txtBars);
      this.grpData.Controls.Add(this.lblSymbol);
      this.grpData.Controls.Add(this.txtSymbol);
      this.grpData.Location = new System.Drawing.Point(9, 9);
      this.grpData.Name = "grpData";
      this.grpData.Size = new System.Drawing.Size(338, 119);
      this.grpData.TabIndex = 145;
      this.grpData.Text = "Data Source";
      // 
      // txtSymbol
      // 
      this.txtSymbol.Location = new System.Drawing.Point(75, 40);
      this.txtSymbol.Name = "txtSymbol";
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
      this.Load += new System.EventHandler(this.ctlBacktest_Load);
      this.Resize += new System.EventHandler(this.ctlBacktest_Resize);
      this.tpBuy.ResumeLayout(false);
      this.tpSell.ResumeLayout(false);
      this.tpExitShort.ResumeLayout(false);
      this.tpExitLong.ResumeLayout(false);
      this.tabScripts.ResumeLayout(false);
      this.grpTrades.ResumeLayout(false);
      this.grpData.ResumeLayout(false);
      this.grpData.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Button cmdDocumentation;
    internal System.Windows.Forms.Label lblInterval;
    internal System.Windows.Forms.TextBox txtInterval;
    internal System.Windows.Forms.Label lblPeriodicity;
    internal System.Windows.Forms.ComboBox cboPeriodicity;
    internal System.Windows.Forms.TabPage tpBuy;
    internal System.Windows.Forms.TextBox txtBuyScript;
    internal System.Windows.Forms.TabPage tpSell;
    internal System.Windows.Forms.TextBox txtSellScript;
    internal System.Windows.Forms.TextBox txtExitLongScript;
    internal System.Windows.Forms.TabPage tpExitShort;
    internal System.Windows.Forms.TextBox txtExitShortScript;
    internal System.Windows.Forms.TabPage tpExitLong;
    internal System.Windows.Forms.TabControl tabScripts;
    internal System.Windows.Forms.ImageList m_ImgList;
    internal System.Windows.Forms.TextBox txtBars;
    internal System.Windows.Forms.ListView m_ListTrades;
    internal System.Windows.Forms.ColumnHeader Trades;
    internal System.Windows.Forms.GroupBox grpTrades;
    internal System.Windows.Forms.Button cmdBacktest;
    internal System.Windows.Forms.Label lblBars;
    internal System.Windows.Forms.Timer tmrUpdate;
    internal System.Windows.Forms.Label lblSymbol;
    internal System.Windows.Forms.GroupBox grpData;
    internal System.Windows.Forms.TextBox txtSymbol;
  }
}
