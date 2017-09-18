namespace M4
{
  partial class ctlPortfolio
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.pnlPortfolio = new System.Windows.Forms.Panel();
      this.grpTradeOptions = new System.Windows.Forms.GroupBox();
      this.cboExchanges = new System.Windows.Forms.ComboBox();
      this.txtSymbol = new System.Windows.Forms.TextBox();
      this.lblExchange = new System.Windows.Forms.Label();
      this.lblSymbol = new System.Windows.Forms.Label();
      this.udQuantity = new System.Windows.Forms.NumericUpDown();
      this.lblQuantity = new System.Windows.Forms.Label();
      this.rdoBtnSell = new System.Windows.Forms.RadioButton();
      this.rdoBtnBuy = new System.Windows.Forms.RadioButton();
      this.grpAccountSummary = new System.Windows.Forms.GroupBox();
      this.btnSubmit = new System.Windows.Forms.Button();
      this.grpExpires = new System.Windows.Forms.GroupBox();
      this.rdoGTCHours = new System.Windows.Forms.RadioButton();
      this.rdoGTC = new System.Windows.Forms.RadioButton();
      this.rdoDayHours = new System.Windows.Forms.RadioButton();
      this.rdoDay = new System.Windows.Forms.RadioButton();
      this.grpOrderType = new System.Windows.Forms.GroupBox();
      this.txtStopLimit = new System.Windows.Forms.TextBox();
      this.Label1 = new System.Windows.Forms.Label();
      this.rdoStopLimit = new System.Windows.Forms.RadioButton();
      this.rdoLimit = new System.Windows.Forms.RadioButton();
      this.rdoStopMarket = new System.Windows.Forms.RadioButton();
      this.rdoMarket = new System.Windows.Forms.RadioButton();
      this.cmdDeletePortfolio = new System.Windows.Forms.Button();
      this.m_PortfolioGrid = new M4DataGridView();
      this.m_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.miCancelOrder = new System.Windows.Forms.ToolStripMenuItem();
      this.miChartSymbol = new System.Windows.Forms.ToolStripMenuItem();
      //this.NLineControl1 = new Nevron.UI.WinForm.Controls.NLineControl();
      this.cmbPortfolio = new System.Windows.Forms.ComboBox();
      this.lblPortfolio = new System.Windows.Forms.Label();
      this.m_ContextMenu = new System.Windows.Forms.ContextMenu();
      this.TradeSymbol = new System.Windows.Forms.TextBox();
      this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
      this.OrderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Entry = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Last = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DollarGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PercentGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.pnlPortfolio.SuspendLayout();
      this.grpTradeOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udQuantity)).BeginInit();
      this.grpExpires.SuspendLayout();
      this.grpOrderType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_PortfolioGrid)).BeginInit();
      this.m_ContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlPortfolio
      // 
      this.pnlPortfolio.Controls.Add(this.grpTradeOptions);
      this.pnlPortfolio.Controls.Add(this.grpAccountSummary);
      this.pnlPortfolio.Controls.Add(this.btnSubmit);
      this.pnlPortfolio.Controls.Add(this.grpExpires);
      this.pnlPortfolio.Controls.Add(this.grpOrderType);
      this.pnlPortfolio.Controls.Add(this.cmdDeletePortfolio);
      this.pnlPortfolio.Controls.Add(this.m_PortfolioGrid);
      //this.pnlPortfolio.Controls.Add(this.NLineControl1);
      this.pnlPortfolio.Controls.Add(this.cmbPortfolio);
      this.pnlPortfolio.Controls.Add(this.lblPortfolio);
      this.pnlPortfolio.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlPortfolio.Location = new System.Drawing.Point(0, 0);
      this.pnlPortfolio.Name = "pnlPortfolio";
      this.pnlPortfolio.Size = new System.Drawing.Size(848, 507);
      this.pnlPortfolio.TabIndex = 2;
      this.pnlPortfolio.Text = "NuiPanel1";
      // 
      // grpTradeOptions
      // 
      this.grpTradeOptions.Controls.Add(this.cboExchanges);
      this.grpTradeOptions.Controls.Add(this.txtSymbol);
      this.grpTradeOptions.Controls.Add(this.lblExchange);
      this.grpTradeOptions.Controls.Add(this.lblSymbol);
      this.grpTradeOptions.Controls.Add(this.udQuantity);
      this.grpTradeOptions.Controls.Add(this.lblQuantity);
      this.grpTradeOptions.Controls.Add(this.rdoBtnSell);
      this.grpTradeOptions.Controls.Add(this.rdoBtnBuy);
      this.grpTradeOptions.Location = new System.Drawing.Point(12, 74);
      this.grpTradeOptions.Name = "grpTradeOptions";
      this.grpTradeOptions.Size = new System.Drawing.Size(257, 122);
      this.grpTradeOptions.TabIndex = 59;
      this.grpTradeOptions.Text = "Trade Options";
      // 
      // cboExchanges
      // 
      this.cboExchanges.Location = new System.Drawing.Point(126, 59);
      this.cboExchanges.Name = "cboExchanges";
      this.cboExchanges.Size = new System.Drawing.Size(100, 22);
      this.cboExchanges.TabIndex = 53;
      // 
      // txtSymbol
      // 
      this.txtSymbol.Location = new System.Drawing.Point(126, 34);
      this.txtSymbol.Name = "txtSymbol";
      this.txtSymbol.Size = new System.Drawing.Size(100, 18);
      this.txtSymbol.TabIndex = 51;
      // 
      // lblExchange
      // 
      this.lblExchange.AutoSize = true;
      this.lblExchange.BackColor = System.Drawing.Color.Transparent;
      this.lblExchange.Location = new System.Drawing.Point(63, 63);
      this.lblExchange.Name = "lblExchange";
      this.lblExchange.Size = new System.Drawing.Size(55, 13);
      this.lblExchange.TabIndex = 54;
      this.lblExchange.Text = "Exchange";
      // 
      // lblSymbol
      // 
      this.lblSymbol.AutoSize = true;
      this.lblSymbol.BackColor = System.Drawing.Color.Transparent;
      this.lblSymbol.Location = new System.Drawing.Point(77, 37);
      this.lblSymbol.Name = "lblSymbol";
      this.lblSymbol.Size = new System.Drawing.Size(41, 13);
      this.lblSymbol.TabIndex = 52;
      this.lblSymbol.Text = "Symbol";
      // 
      // udQuantity
      // 
      this.udQuantity.Location = new System.Drawing.Point(126, 86);
      this.udQuantity.Name = "udQuantity";
      this.udQuantity.Size = new System.Drawing.Size(60, 20);
      this.udQuantity.TabIndex = 50;
      this.udQuantity.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
      // 
      // lblQuantity
      // 
      this.lblQuantity.AutoSize = true;
      this.lblQuantity.BackColor = System.Drawing.Color.Transparent;
      this.lblQuantity.Location = new System.Drawing.Point(72, 90);
      this.lblQuantity.Name = "lblQuantity";
      this.lblQuantity.Size = new System.Drawing.Size(46, 13);
      this.lblQuantity.TabIndex = 49;
      this.lblQuantity.Text = "Quantity";
      // 
      // rdoBtnSell
      // 
      this.rdoBtnSell.Appearance = System.Windows.Forms.Appearance.Button;
      this.rdoBtnSell.AutoSize = true;
      this.rdoBtnSell.Location = new System.Drawing.Point(17, 77);
      this.rdoBtnSell.Name = "rdoBtnSell";
      this.rdoBtnSell.Size = new System.Drawing.Size(34, 23);
      this.rdoBtnSell.TabIndex = 48;
      this.rdoBtnSell.Text = "Sell";
      this.rdoBtnSell.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // rdoBtnBuy
      // 
      this.rdoBtnBuy.Appearance = System.Windows.Forms.Appearance.Button;
      this.rdoBtnBuy.AutoSize = true;
      this.rdoBtnBuy.Checked = true;
      this.rdoBtnBuy.Location = new System.Drawing.Point(16, 36);
      this.rdoBtnBuy.Name = "rdoBtnBuy";
      this.rdoBtnBuy.Size = new System.Drawing.Size(35, 23);
      this.rdoBtnBuy.TabIndex = 47;
      this.rdoBtnBuy.TabStop = true;
      this.rdoBtnBuy.Text = "Buy";
      this.rdoBtnBuy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // grpAccountSummary
      // 
      this.grpAccountSummary.Location = new System.Drawing.Point(275, 12);
      this.grpAccountSummary.Name = "grpAccountSummary";
      this.grpAccountSummary.Size = new System.Drawing.Size(545, 50);
      this.grpAccountSummary.TabIndex = 63;
      this.grpAccountSummary.Text = "Account Summary";
      // 
      // btnSubmit
      // 
      this.btnSubmit.Location = new System.Drawing.Point(742, 125);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new System.Drawing.Size(80, 27);
      this.btnSubmit.TabIndex = 61;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = false;
      this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
      // 
      // grpExpires
      // 
      this.grpExpires.Controls.Add(this.rdoGTCHours);
      this.grpExpires.Controls.Add(this.rdoGTC);
      this.grpExpires.Controls.Add(this.rdoDayHours);
      this.grpExpires.Controls.Add(this.rdoDay);
      this.grpExpires.Location = new System.Drawing.Point(509, 74);
      this.grpExpires.Name = "grpExpires";
      this.grpExpires.Size = new System.Drawing.Size(211, 122);
      this.grpExpires.TabIndex = 59;
      this.grpExpires.Text = "Expires";
      // 
      // rdoGTCHours
      // 
      this.rdoGTCHours.AutoSize = true;
      this.rdoGTCHours.Location = new System.Drawing.Point(92, 58);
      this.rdoGTCHours.Name = "rdoGTCHours";
      this.rdoGTCHours.Size = new System.Drawing.Size(108, 17);
      this.rdoGTCHours.TabIndex = 3;
      this.rdoGTCHours.Text = "GTC + Ext. Hours";
      this.rdoGTCHours.UseVisualStyleBackColor = false;
      // 
      // rdoGTC
      // 
      this.rdoGTC.AutoSize = true;
      this.rdoGTC.Location = new System.Drawing.Point(13, 59);
      this.rdoGTC.Name = "rdoGTC";
      this.rdoGTC.Size = new System.Drawing.Size(47, 17);
      this.rdoGTC.TabIndex = 2;
      this.rdoGTC.Text = "GTC";
      this.rdoGTC.UseVisualStyleBackColor = false;
      // 
      // rdoDayHours
      // 
      this.rdoDayHours.AutoSize = true;
      this.rdoDayHours.Location = new System.Drawing.Point(92, 31);
      this.rdoDayHours.Name = "rdoDayHours";
      this.rdoDayHours.Size = new System.Drawing.Size(105, 17);
      this.rdoDayHours.TabIndex = 1;
      this.rdoDayHours.Text = "Day + Ext. Hours";
      this.rdoDayHours.UseVisualStyleBackColor = false;
      // 
      // rdoDay
      // 
      this.rdoDay.AutoSize = true;
      this.rdoDay.Checked = true;
      this.rdoDay.Location = new System.Drawing.Point(13, 31);
      this.rdoDay.Name = "rdoDay";
      this.rdoDay.Size = new System.Drawing.Size(44, 17);
      this.rdoDay.TabIndex = 0;
      this.rdoDay.TabStop = true;
      this.rdoDay.Text = "Day";
      this.rdoDay.UseVisualStyleBackColor = false;
      // 
      // grpOrderType
      // 
      this.grpOrderType.Controls.Add(this.txtStopLimit);
      this.grpOrderType.Controls.Add(this.Label1);
      this.grpOrderType.Controls.Add(this.rdoStopLimit);
      this.grpOrderType.Controls.Add(this.rdoLimit);
      this.grpOrderType.Controls.Add(this.rdoStopMarket);
      this.grpOrderType.Controls.Add(this.rdoMarket);
      this.grpOrderType.Location = new System.Drawing.Point(275, 74);
      this.grpOrderType.Name = "grpOrderType";
      this.grpOrderType.Size = new System.Drawing.Size(228, 122);
      this.grpOrderType.TabIndex = 58;
      this.grpOrderType.Text = "Order Type";
      // 
      // txtStopLimit
      // 
      this.txtStopLimit.Location = new System.Drawing.Point(118, 84);
      this.txtStopLimit.Name = "txtStopLimit";
      this.txtStopLimit.Size = new System.Drawing.Size(79, 18);
      this.txtStopLimit.TabIndex = 46;
      this.txtStopLimit.TextChanged += new System.EventHandler(this.txtStopLimit_TextChanged);
      this.txtStopLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStopLimit_KeyPress);
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.BackColor = System.Drawing.Color.Transparent;
      this.Label1.Location = new System.Drawing.Point(25, 87);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(88, 13);
      this.Label1.TabIndex = 45;
      this.Label1.Text = "Stop / Limit Price";
      // 
      // rdoStopLimit
      // 
      this.rdoStopLimit.AutoSize = true;
      this.rdoStopLimit.Location = new System.Drawing.Point(118, 59);
      this.rdoStopLimit.Name = "rdoStopLimit";
      this.rdoStopLimit.Size = new System.Drawing.Size(71, 17);
      this.rdoStopLimit.TabIndex = 3;
      this.rdoStopLimit.Text = "Stop Limit";
      this.rdoStopLimit.UseVisualStyleBackColor = false;
      // 
      // rdoLimit
      // 
      this.rdoLimit.AutoSize = true;
      this.rdoLimit.Location = new System.Drawing.Point(118, 32);
      this.rdoLimit.Name = "rdoLimit";
      this.rdoLimit.Size = new System.Drawing.Size(46, 17);
      this.rdoLimit.TabIndex = 2;
      this.rdoLimit.Text = "Limit";
      this.rdoLimit.UseVisualStyleBackColor = false;
      // 
      // rdoStopMarket
      // 
      this.rdoStopMarket.AutoSize = true;
      this.rdoStopMarket.Location = new System.Drawing.Point(9, 58);
      this.rdoStopMarket.Name = "rdoStopMarket";
      this.rdoStopMarket.Size = new System.Drawing.Size(83, 17);
      this.rdoStopMarket.TabIndex = 1;
      this.rdoStopMarket.Text = "Stop Market";
      this.rdoStopMarket.UseVisualStyleBackColor = false;
      // 
      // rdoMarket
      // 
      this.rdoMarket.AutoSize = true;
      this.rdoMarket.Checked = true;
      this.rdoMarket.Location = new System.Drawing.Point(9, 31);
      this.rdoMarket.Name = "rdoMarket";
      this.rdoMarket.Size = new System.Drawing.Size(58, 17);
      this.rdoMarket.TabIndex = 0;
      this.rdoMarket.TabStop = true;
      this.rdoMarket.Text = "Market";
      this.rdoMarket.UseVisualStyleBackColor = false;
      // 
      // cmdDeletePortfolio
      // 
      this.cmdDeletePortfolio.Location = new System.Drawing.Point(66, 43);
      this.cmdDeletePortfolio.Name = "cmdDeletePortfolio";
      this.cmdDeletePortfolio.Size = new System.Drawing.Size(100, 23);
      this.cmdDeletePortfolio.TabIndex = 32;
      this.cmdDeletePortfolio.Text = "Delete Portfolio";
      this.cmdDeletePortfolio.UseVisualStyleBackColor = false;
      this.cmdDeletePortfolio.Visible = false;
      this.cmdDeletePortfolio.Click += new System.EventHandler(this.cmdDeletePortfolio_Click);
      // 
      // m_PortfolioGrid
      // 
      this.m_PortfolioGrid.AllowUserToAddRows = false;
      this.m_PortfolioGrid.AllowUserToDeleteRows = false;
      this.m_PortfolioGrid.AllowUserToResizeColumns = false;
      this.m_PortfolioGrid.AllowUserToResizeRows = false;
      this.m_PortfolioGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.m_PortfolioGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.m_PortfolioGrid.BackgroundColor = System.Drawing.Color.White;
      this.m_PortfolioGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.m_PortfolioGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.m_PortfolioGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.m_PortfolioGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderID,
            this.Status,
            this.Details,
            this.Time,
            this.Symbol,
            this.Type,
            this.Qty,
            this.Entry,
            this.Last,
            this.DollarGain,
            this.PercentGain});
      this.m_PortfolioGrid.ContextMenuStrip = this.m_ContextMenuStrip;
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
      dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.m_PortfolioGrid.DefaultCellStyle = dataGridViewCellStyle7;
      this.m_PortfolioGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.m_PortfolioGrid.GridColor = System.Drawing.SystemColors.ControlDarkDark;
      this.m_PortfolioGrid.Location = new System.Drawing.Point(5, 217);
      this.m_PortfolioGrid.MultiSelect = false;
      this.m_PortfolioGrid.Name = "m_PortfolioGrid";
      this.m_PortfolioGrid.ReadOnly = true;
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
      dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.m_PortfolioGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
      this.m_PortfolioGrid.RowHeadersVisible = false;
      this.m_PortfolioGrid.RowHeadersWidth = 4;
      this.m_PortfolioGrid.RowTemplate.ReadOnly = true;
      this.m_PortfolioGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.m_PortfolioGrid.Size = new System.Drawing.Size(836, 283);
      this.m_PortfolioGrid.TabIndex = 30;
      this.m_PortfolioGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_PortfolioGrid_MouseUp);
      // 
      // m_ContextMenuStrip
      // 
      this.m_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCancelOrder,
            this.miChartSymbol});
      this.m_ContextMenuStrip.Name = "m_ContextMenuStrip";
      this.m_ContextMenuStrip.Size = new System.Drawing.Size(154, 48);
      // 
      // miCancelOrder
      // 
      this.miCancelOrder.Name = "miCancelOrder";
      this.miCancelOrder.Size = new System.Drawing.Size(153, 22);
      this.miCancelOrder.Text = "Cancel Symbol";
      this.miCancelOrder.Click += new System.EventHandler(this.miCancelOrder_Click);
      // 
      // miChartSymbol
      // 
      this.miChartSymbol.Name = "miChartSymbol";
      this.miChartSymbol.Size = new System.Drawing.Size(153, 22);
      this.miChartSymbol.Text = "Chart Symbol";
      this.miChartSymbol.Click += new System.EventHandler(this.miChartSymbol_Click);
      // 
      // cmbPortfolio
      // 
      this.cmbPortfolio.Location = new System.Drawing.Point(66, 17);
      this.cmbPortfolio.Name = "cmbPortfolio";
      this.cmbPortfolio.Size = new System.Drawing.Size(170, 22);
      this.cmbPortfolio.TabIndex = 23;
      this.cmbPortfolio.SelectedIndexChanged += new System.EventHandler(this.cmbPortfolio_SelectedIndexChanged);
      // 
      // lblPortfolio
      // 
      this.lblPortfolio.AutoSize = true;
      this.lblPortfolio.Location = new System.Drawing.Point(12, 21);
      this.lblPortfolio.Name = "lblPortfolio";
      this.lblPortfolio.Size = new System.Drawing.Size(48, 13);
      this.lblPortfolio.TabIndex = 22;
      this.lblPortfolio.Text = "Portfolio:";
      // 
      // TradeSymbol
      // 
      this.TradeSymbol.Text = "Trade Symbol";
      // 
      // tmrUpdate
      // 
      this.tmrUpdate.Enabled = true;
      this.tmrUpdate.Interval = 1000;
      this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
      // 
      // OrderID
      // 
      this.OrderID.HeaderText = "OrderID";
      this.OrderID.Name = "OrderID";
      this.OrderID.ReadOnly = true;
      // 
      // Status
      // 
      this.Status.HeaderText = "Status";
      this.Status.Name = "Status";
      this.Status.ReadOnly = true;
      // 
      // Details
      // 
      this.Details.HeaderText = "Details";
      this.Details.Name = "Details";
      this.Details.ReadOnly = true;
      // 
      // Time
      // 
      this.Time.HeaderText = "Time";
      this.Time.Name = "Time";
      this.Time.ReadOnly = true;
      // 
      // Symbol
      // 
      this.Symbol.HeaderText = "Symbol";
      this.Symbol.Name = "Symbol";
      this.Symbol.ReadOnly = true;
      this.Symbol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      // 
      // Type
      // 
      this.Type.HeaderText = "Type";
      this.Type.Name = "Type";
      this.Type.ReadOnly = true;
      // 
      // Qty
      // 
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.Qty.DefaultCellStyle = dataGridViewCellStyle2;
      this.Qty.HeaderText = "Qty";
      this.Qty.Name = "Qty";
      this.Qty.ReadOnly = true;
      // 
      // Entry
      // 
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.Entry.DefaultCellStyle = dataGridViewCellStyle3;
      this.Entry.HeaderText = "Entry";
      this.Entry.Name = "Entry";
      this.Entry.ReadOnly = true;
      // 
      // Last
      // 
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.Last.DefaultCellStyle = dataGridViewCellStyle4;
      this.Last.HeaderText = "Last";
      this.Last.Name = "Last";
      this.Last.ReadOnly = true;
      // 
      // DollarGain
      // 
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.DollarGain.DefaultCellStyle = dataGridViewCellStyle5;
      this.DollarGain.HeaderText = "$ Gain";
      this.DollarGain.Name = "DollarGain";
      this.DollarGain.ReadOnly = true;
      // 
      // PercentGain
      // 
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.PercentGain.DefaultCellStyle = dataGridViewCellStyle6;
      this.PercentGain.HeaderText = "% Gain";
      this.PercentGain.Name = "PercentGain";
      this.PercentGain.ReadOnly = true;
      // 
      // ctlPortfolio
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.pnlPortfolio);
      this.Name = "ctlPortfolio";
      this.Size = new System.Drawing.Size(848, 507);
      this.Resize += new System.EventHandler(this.ctlPortfolio_Resize);
      this.pnlPortfolio.ResumeLayout(false);
      this.pnlPortfolio.PerformLayout();
      this.grpTradeOptions.ResumeLayout(false);
      this.grpTradeOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udQuantity)).EndInit();
      this.grpExpires.ResumeLayout(false);
      this.grpExpires.PerformLayout();
      this.grpOrderType.ResumeLayout(false);
      this.grpOrderType.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_PortfolioGrid)).EndInit();
      this.m_ContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Panel pnlPortfolio;
    internal System.Windows.Forms.GroupBox TradeOptions;
    internal System.Windows.Forms.ComboBox cboExchanges;
    internal System.Windows.Forms.TextBox txtSymbol;
    internal System.Windows.Forms.Label lblExchange;
    internal System.Windows.Forms.Label lblSymbol;
    internal System.Windows.Forms.NumericUpDown udQuantity;
    internal System.Windows.Forms.Label lblQuantity;
    internal System.Windows.Forms.RadioButton rdoBtnSell;
    internal System.Windows.Forms.RadioButton rdoBtnBuy;
    internal System.Windows.Forms.GroupBox grpAccountSummary;
    internal System.Windows.Forms.Button btnSubmit;
    internal System.Windows.Forms.GroupBox grpExpires;
    internal System.Windows.Forms.RadioButton rdoGTCHours;
    internal System.Windows.Forms.RadioButton rdoGTC;
    internal System.Windows.Forms.RadioButton rdoDayHours;
    internal System.Windows.Forms.RadioButton rdoDay;
    internal System.Windows.Forms.GroupBox grpOrderType;
    internal System.Windows.Forms.GroupBox grpTradeOptions;
    internal System.Windows.Forms.TextBox txtStopLimit;
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.RadioButton rdoStopLimit;
    internal System.Windows.Forms.RadioButton rdoLimit;
    internal System.Windows.Forms.RadioButton rdoStopMarket;
    internal System.Windows.Forms.RadioButton rdoMarket;
    internal System.Windows.Forms.Button cmdDeletePortfolio;
    internal System.Windows.Forms.ContextMenuStrip m_ContextMenuStrip;
    internal System.Windows.Forms.ToolStripMenuItem miCancelOrder;
    internal System.Windows.Forms.ToolStripMenuItem miChartSymbol;
    //internal System.Windows.Forms.LineControl NLineControl1;
    internal System.Windows.Forms.ComboBox cmbPortfolio;
    internal System.Windows.Forms.Label lblPortfolio;
    internal System.Windows.Forms.ContextMenu m_ContextMenu;
    internal System.Windows.Forms.TextBox TradeSymbol;
    internal System.Windows.Forms.Timer tmrUpdate;
    internal M4DataGridView m_PortfolioGrid;
    private System.Windows.Forms.DataGridViewTextBoxColumn OrderID;
    private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    private System.Windows.Forms.DataGridViewTextBoxColumn Details;
    private System.Windows.Forms.DataGridViewTextBoxColumn Time;
    private System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
    private System.Windows.Forms.DataGridViewTextBoxColumn Type;
    private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
    private System.Windows.Forms.DataGridViewTextBoxColumn Entry;
    private System.Windows.Forms.DataGridViewTextBoxColumn Last;
    private System.Windows.Forms.DataGridViewTextBoxColumn DollarGain;
    private System.Windows.Forms.DataGridViewTextBoxColumn PercentGain;
  }
}
