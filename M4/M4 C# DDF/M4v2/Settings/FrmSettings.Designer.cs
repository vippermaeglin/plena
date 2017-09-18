namespace M4.M4v2.Settings
{
    partial class FrmSettings
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
            this.pgvSettings = new Telerik.WinControls.UI.RadPageView();
            this.tabProxy = new Telerik.WinControls.UI.RadPageViewPage();
            this.tabStudies = new Telerik.WinControls.UI.RadPageViewPage();
            this.tabChart = new Telerik.WinControls.UI.RadPageViewPage();
            this.tabPrice = new Telerik.WinControls.UI.RadPageViewPage();
            this.tabUser = new Telerik.WinControls.UI.RadPageViewPage();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.pgvSettings)).BeginInit();
            this.pgvSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pgvSettings
            // 
            this.pgvSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgvSettings.Controls.Add(this.tabProxy);
            this.pgvSettings.Controls.Add(this.tabStudies);
            this.pgvSettings.Controls.Add(this.tabChart);
            this.pgvSettings.Controls.Add(this.tabPrice);
            this.pgvSettings.Controls.Add(this.tabUser);
            this.pgvSettings.Location = new System.Drawing.Point(-3, 0);
            this.pgvSettings.Name = "pgvSettings";
            this.pgvSettings.SelectedPage = this.tabProxy;
            this.pgvSettings.Size = new System.Drawing.Size(392, 327);
            this.pgvSettings.TabIndex = 1;
            this.pgvSettings.Text = "Settings";
            this.pgvSettings.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Near;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemFitMode = Telerik.WinControls.UI.StripViewItemFitMode.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemDragMode = Telerik.WinControls.UI.PageViewItemDragMode.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemSpacing = 1;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemSizeMode = Telerik.WinControls.UI.PageViewItemSizeMode.Individual;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.pgvSettings.GetChildAt(0))).ItemContentOrientation = Telerik.WinControls.UI.PageViewContentOrientation.Auto;
            // 
            // tabProxy
            // 
            this.tabProxy.Location = new System.Drawing.Point(10, 37);
            this.tabProxy.Name = "tabProxy";
            this.tabProxy.Size = new System.Drawing.Size(371, 279);
            this.tabProxy.Text = "Proxy";
            // 
            // tabStudies
            // 
            this.tabStudies.Location = new System.Drawing.Point(10, 37);
            this.tabStudies.Name = "tabStudies";
            this.tabStudies.Size = new System.Drawing.Size(433, 354);
            this.tabStudies.Text = "Studies";
            this.tabStudies.Paint += new System.Windows.Forms.PaintEventHandler(this.TabStudiesPaint);
            // 
            // tabChart
            // 
            this.tabChart.Location = new System.Drawing.Point(10, 37);
            this.tabChart.Name = "tabChart";
            this.tabChart.Size = new System.Drawing.Size(371, 279);
            this.tabChart.Text = "Chart";
            this.tabChart.Paint += new System.Windows.Forms.PaintEventHandler(this.TabChartPaint);
            // 
            // tabPrice
            // 
            this.tabPrice.Location = new System.Drawing.Point(10, 37);
            this.tabPrice.Name = "tabPrice";
            this.tabPrice.Size = new System.Drawing.Size(367, 377);
            this.tabPrice.Text = "Price";
            this.tabPrice.Paint += new System.Windows.Forms.PaintEventHandler(this.TabPricePaint);
            // 
            // tabUser
            // 
            this.tabUser.Location = new System.Drawing.Point(10, 37);
            this.tabUser.Name = "tabUser";
            this.tabUser.Size = new System.Drawing.Size(367, 377);
            this.tabUser.Text = "User";
            this.tabUser.Paint += new System.Windows.Forms.PaintEventHandler(this.TabUserPaint);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(220, 333);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 24);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.BtnConfirmClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 333);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelarClick);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 362);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pgvSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSettingsKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pgvSettings)).EndInit();
            this.pgvSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        protected void LoadProxy()
        {
            this.txtPasswordAuthenticationProxy = new Telerik.WinControls.UI.RadTextBox();
            this.txtPortProxySocks = new Telerik.WinControls.UI.RadTextBox();
            this.lblPasswordAuthenticationProxy = new Telerik.WinControls.UI.RadLabel();
            this.txtUserAuthenticationProxy = new Telerik.WinControls.UI.RadTextBox();
            this.lblPortProxySocks = new Telerik.WinControls.UI.RadLabel();
            this.lblUserAuthenticationProxy = new Telerik.WinControls.UI.RadLabel();
            this.txtPortProxy = new Telerik.WinControls.UI.RadTextBox();
            this.txtServerURL = new Telerik.WinControls.UI.RadTextBox();
            this.txtSuperscriptionProxySocks = new Telerik.WinControls.UI.RadTextBox();
            this.lblPortProxy = new Telerik.WinControls.UI.RadLabel();
            this.lblSuperscriptionProxySocks = new Telerik.WinControls.UI.RadLabel();
            this.txtSuperscriptionProxy = new Telerik.WinControls.UI.RadTextBox();
            this.lblSuperscriptionProxy = new Telerik.WinControls.UI.RadLabel();
            this.lblServerURL = new Telerik.WinControls.UI.RadLabel();
            this.cbxAuthenticationProxy = new Telerik.WinControls.UI.RadCheckBox();
            this.optProxySocks = new Telerik.WinControls.UI.RadRadioButton();
            this.optProxyServer = new Telerik.WinControls.UI.RadRadioButton();
            this.optConfigProxyNavigator = new Telerik.WinControls.UI.RadRadioButton();
            this.optNotProxy = new Telerik.WinControls.UI.RadRadioButton();

            // 
            // grpSettingsProxy
            // 
            this.tabProxy.Controls.Add(this.txtPasswordAuthenticationProxy);
            this.tabProxy.Controls.Add(this.txtPortProxySocks);
            this.tabProxy.Controls.Add(this.lblPasswordAuthenticationProxy);
            this.tabProxy.Controls.Add(this.txtUserAuthenticationProxy);
            this.tabProxy.Controls.Add(this.lblPortProxySocks);
            this.tabProxy.Controls.Add(this.lblUserAuthenticationProxy);
            this.tabProxy.Controls.Add(this.txtPortProxy);
            this.tabProxy.Controls.Add(this.txtServerURL);
            this.tabProxy.Controls.Add(this.txtSuperscriptionProxySocks);
            this.tabProxy.Controls.Add(this.lblPortProxy);
            this.tabProxy.Controls.Add(this.lblSuperscriptionProxySocks);
            this.tabProxy.Controls.Add(this.txtSuperscriptionProxy);
            this.tabProxy.Controls.Add(this.lblServerURL);
            this.tabProxy.Controls.Add(this.lblSuperscriptionProxy);
            this.tabProxy.Controls.Add(this.cbxAuthenticationProxy);
            this.tabProxy.Controls.Add(this.optProxySocks);
            this.tabProxy.Controls.Add(this.optProxyServer);
            this.tabProxy.Controls.Add(this.optConfigProxyNavigator);
            this.tabProxy.Controls.Add(this.optNotProxy);
            // 
            // txtPasswordAuthenticationProxy
            // 
            this.txtPasswordAuthenticationProxy.Enabled = false;
            this.txtPasswordAuthenticationProxy.Location = new System.Drawing.Point(271, 235);
            this.txtPasswordAuthenticationProxy.Name = "txtPasswordAuthenticationProxy";
            this.txtPasswordAuthenticationProxy.PasswordChar = '*';
            this.txtPasswordAuthenticationProxy.Size = new System.Drawing.Size(101, 20);
            this.txtPasswordAuthenticationProxy.TabIndex = 22;
            this.txtPasswordAuthenticationProxy.TabStop = false;
            // 
            // txtPortProxySocks
            // 
            this.txtPortProxySocks.Enabled = false;
            this.txtPortProxySocks.Location = new System.Drawing.Point(270, 179);
            this.txtPortProxySocks.Name = "txtPortProxySocks";
            this.txtPortProxySocks.Size = new System.Drawing.Size(65, 20);
            this.txtPortProxySocks.TabIndex = 18;
            this.txtPortProxySocks.TabStop = false;
            // 
            // lblPasswordAuthenticationProxy
            // 
            this.lblPasswordAuthenticationProxy.Enabled = false;
            this.lblPasswordAuthenticationProxy.Location = new System.Drawing.Point(215, 235);
            this.lblPasswordAuthenticationProxy.Name = "lblPasswordAuthenticationProxy";
            this.lblPasswordAuthenticationProxy.Size = new System.Drawing.Size(39, 18);
            this.lblPasswordAuthenticationProxy.TabIndex = 21;
            this.lblPasswordAuthenticationProxy.Text = "Senha:";
            // 
            // txtUserAuthenticationProxy
            // 
            this.txtUserAuthenticationProxy.Enabled = false;
            this.txtUserAuthenticationProxy.Location = new System.Drawing.Point(74, 235);
            this.txtUserAuthenticationProxy.Name = "txtUserAuthenticationProxy";
            this.txtUserAuthenticationProxy.Size = new System.Drawing.Size(122, 20);
            this.txtUserAuthenticationProxy.TabIndex = 20;
            this.txtUserAuthenticationProxy.TabStop = false;
            // 
            // lblPortProxySocks
            // 
            this.lblPortProxySocks.Enabled = false;
            this.lblPortProxySocks.Location = new System.Drawing.Point(229, 179);
            this.lblPortProxySocks.Name = "lblPortProxySocks";
            this.lblPortProxySocks.Size = new System.Drawing.Size(35, 18);
            this.lblPortProxySocks.TabIndex = 17;
            this.lblPortProxySocks.Text = "Porta:";
            // 
            // lblUserAuthenticationProxy
            // 
            this.lblUserAuthenticationProxy.Enabled = false;
            this.lblUserAuthenticationProxy.Location = new System.Drawing.Point(17, 235);
            this.lblUserAuthenticationProxy.Name = "lblUserAuthenticationProxy";
            this.lblUserAuthenticationProxy.Size = new System.Drawing.Size(47, 18);
            this.lblUserAuthenticationProxy.TabIndex = 19;
            this.lblUserAuthenticationProxy.Text = "Usuário:";
            // 
            // txtPortProxy
            // 
            this.txtPortProxy.Enabled = false;
            this.txtPortProxy.Location = new System.Drawing.Point(270, 116);
            this.txtPortProxy.Name = "txtPortProxy";
            this.txtPortProxy.Size = new System.Drawing.Size(65, 20);
            this.txtPortProxy.TabIndex = 14;
            this.txtPortProxy.TabStop = false;
            // 
            // txtSuperscriptionProxySocks
            // 
            this.txtSuperscriptionProxySocks.Enabled = false;
            this.txtSuperscriptionProxySocks.Location = new System.Drawing.Point(94, 179);
            this.txtSuperscriptionProxySocks.Name = "txtSuperscriptionProxySocks";
            this.txtSuperscriptionProxySocks.Size = new System.Drawing.Size(120, 20);
            this.txtSuperscriptionProxySocks.TabIndex = 16;
            this.txtSuperscriptionProxySocks.TabStop = false;
            // 
            // lblPortProxy
            // 
            this.lblPortProxy.Enabled = false;
            this.lblPortProxy.Location = new System.Drawing.Point(229, 116);
            this.lblPortProxy.Name = "lblPortProxy";
            this.lblPortProxy.Size = new System.Drawing.Size(35, 18);
            this.lblPortProxy.TabIndex = 13;
            this.lblPortProxy.Text = "Porta:";
            // 
            // lblSuperscriptionProxySocks
            // 
            this.lblSuperscriptionProxySocks.Enabled = false;
            this.lblSuperscriptionProxySocks.Location = new System.Drawing.Point(33, 179);
            this.lblSuperscriptionProxySocks.Name = "lblSuperscriptionProxySocks";
            this.lblSuperscriptionProxySocks.Size = new System.Drawing.Size(55, 18);
            this.lblSuperscriptionProxySocks.TabIndex = 15;
            this.lblSuperscriptionProxySocks.Text = "Endereço:";
            // 
            // txtSuperscriptionProxy
            // 
            this.txtSuperscriptionProxy.Enabled = false;
            this.txtSuperscriptionProxy.Location = new System.Drawing.Point(94, 116);
            this.txtSuperscriptionProxy.Name = "txtSuperscriptionProxy";
            this.txtSuperscriptionProxy.Size = new System.Drawing.Size(120, 20);
            this.txtSuperscriptionProxy.TabIndex = 12;
            this.txtSuperscriptionProxy.TabStop = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.Enabled = true;
            this.txtServerURL.Location = new System.Drawing.Point(78, 8);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(120, 20);
            this.txtServerURL.TabIndex = 14;
            this.txtServerURL.TabStop = false;
            // 
            // lblSuperscriptionProxy
            // 
            this.lblSuperscriptionProxy.Enabled = false;
            this.lblSuperscriptionProxy.Location = new System.Drawing.Point(33, 116);
            this.lblSuperscriptionProxy.Name = "lblSuperscriptionProxy";
            this.lblSuperscriptionProxy.Size = new System.Drawing.Size(53, 18);
            this.lblSuperscriptionProxy.TabIndex = 11;
            this.lblSuperscriptionProxy.Text = "Endereço";
            // 
            // lblServerURL
            // 
            this.lblServerURL.Enabled = true;
            this.lblServerURL.Location = new System.Drawing.Point(17, 9);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(53, 18);
            this.lblServerURL.TabIndex = 11;
            this.lblServerURL.Text = "SERVIDOR:";
            // 
            // cbxAuthenticationProxy
            // 
            this.cbxAuthenticationProxy.Enabled = false;
            this.cbxAuthenticationProxy.Location = new System.Drawing.Point(17, 209);
            this.cbxAuthenticationProxy.Name = "cbxAuthenticationProxy";
            this.cbxAuthenticationProxy.Size = new System.Drawing.Size(124, 18);
            this.cbxAuthenticationProxy.TabIndex = 10;
            this.cbxAuthenticationProxy.Text = "Requer Autenticação";
            this.cbxAuthenticationProxy.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.CbxAuthenticationProxyToggleStateChanged);
            // 
            // optProxySocks
            // 
            this.optProxySocks.Location = new System.Drawing.Point(17, 148);
            this.optProxySocks.Name = "optProxySocks";
            this.optProxySocks.Size = new System.Drawing.Size(166, 18);
            this.optProxySocks.TabIndex = 9;
            this.optProxySocks.Text = "Usar o seguinte proxy SOCKS";
            this.optProxySocks.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.OptProxySocksToggleStateChanged);
            // 
            // optProxyServer
            // 
            this.optProxyServer.Location = new System.Drawing.Point(17, 86);
            this.optProxyServer.Name = "optProxyServer";
            this.optProxyServer.Size = new System.Drawing.Size(188, 18);
            this.optProxyServer.TabIndex = 8;
            this.optProxyServer.Text = "Usar o seguinte servidor de proxy";
            this.optProxyServer.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.OptProxyServerToggleStateChanged);
            // 
            // optConfigProxyNavigator
            // 
            this.optConfigProxyNavigator.Location = new System.Drawing.Point(17, 62);
            this.optConfigProxyNavigator.Name = "optConfigProxyNavigator";
            this.optConfigProxyNavigator.Size = new System.Drawing.Size(290, 18);
            this.optConfigProxyNavigator.TabIndex = 7;
            this.optConfigProxyNavigator.TabStop = true;
            this.optConfigProxyNavigator.Text = "Usar as configurações de proxy do navegador da Web";
            this.optConfigProxyNavigator.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.optConfigProxyNavigator.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.OptConfigProxyNavigatorToggleStateChanged);
            // 
            // optNotProxy
            // 
            this.optNotProxy.Location = new System.Drawing.Point(17, 38);
            this.optNotProxy.Name = "optNotProxy";
            this.optNotProxy.Size = new System.Drawing.Size(155, 18);
            this.optNotProxy.TabIndex = 6;
            this.optNotProxy.Text = "Não usar servidor de proxy";
            this.optNotProxy.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.OptNotProxyToggleStateChanged);

            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordAuthenticationProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPortProxySocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPasswordAuthenticationProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserAuthenticationProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPortProxySocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserAuthenticationProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPortProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerURL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSuperscriptionProxySocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPortProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSuperscriptionProxySocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblServerURL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSuperscriptionProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSuperscriptionProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxAuthenticationProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optProxySocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optProxyServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optConfigProxyNavigator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optNotProxy)).EndInit();
        }

        protected void LoadStudies()
        {
            this.lblLineThickness = new Telerik.WinControls.UI.RadLabel();
            this.boxColor = new Telerik.WinControls.UI.RadColorBox();
            this.lblFibonacci = new Telerik.WinControls.UI.RadLabel();
            this.grdFibonacci = new Telerik.WinControls.UI.RadGridView();
            this.lblColor = new Telerik.WinControls.UI.RadLabel();
            this.rseLineThickness = new Telerik.WinControls.UI.RadSpinEditor();

            // 
            // tabStudies
            // 
            this.tabStudies.Controls.Add(this.lblLineThickness);
            this.tabStudies.Controls.Add(this.boxColor);
            this.tabStudies.Controls.Add(this.lblFibonacci);
            this.tabStudies.Controls.Add(this.grdFibonacci);
            this.tabStudies.Controls.Add(this.lblColor);
            this.tabStudies.Controls.Add(this.rseLineThickness);
            // 
            // lblLineThickness
            // 
            this.lblLineThickness.Location = new System.Drawing.Point(14, 26);
            this.lblLineThickness.Name = "lblLineThickness";
            this.lblLineThickness.Size = new System.Drawing.Size(78, 18);
            this.lblLineThickness.TabIndex = 0;
            this.lblLineThickness.Text = "Line Thickness";
            // 
            // boxColor
            // 
            this.boxColor.Location = new System.Drawing.Point(112, 52);
            this.boxColor.Name = "boxColor";
            this.boxColor.Size = new System.Drawing.Size(100, 20);
            this.boxColor.TabIndex = 6;
            this.boxColor.Text = "Selected Color";
            this.boxColor.Value = System.Drawing.Color.Red;
            // 
            // lblFibonacci
            // 
            this.lblFibonacci.Location = new System.Drawing.Point(15, 78);
            this.lblFibonacci.Name = "lblFibonacci";
            this.lblFibonacci.Size = new System.Drawing.Size(53, 18);
            this.lblFibonacci.TabIndex = 2;
            this.lblFibonacci.Text = "Fibonacci";
            // 
            // grdFibonacci
            // 
            this.grdFibonacci.Location = new System.Drawing.Point(112, 78);
            // 
            // grdFibonacci
            // 
            this.grdFibonacci.MasterTemplate.AllowAddNewRow = false;
            this.grdFibonacci.MasterTemplate.AllowCellContextMenu = false;
            this.grdFibonacci.MasterTemplate.AllowColumnChooser = false;
            this.grdFibonacci.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.grdFibonacci.MasterTemplate.AllowColumnReorder = false;
            this.grdFibonacci.MasterTemplate.AllowColumnResize = false;
            this.grdFibonacci.MasterTemplate.AllowDeleteRow = false;
            this.grdFibonacci.MasterTemplate.AllowDragToGroup = false;
            this.grdFibonacci.MasterTemplate.AllowRowResize = false;
            this.grdFibonacci.MasterTemplate.AutoGenerateColumns = false;
            this.grdFibonacci.MasterTemplate.EnableGrouping = false;
            this.grdFibonacci.MasterTemplate.EnableSorting = false;
            this.grdFibonacci.MasterTemplate.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
            this.grdFibonacci.MasterTemplate.ShowRowHeaderColumn = false;
            this.grdFibonacci.Name = "grdFibonacci";
            this.grdFibonacci.ShowCellErrors = false;
            this.grdFibonacci.ShowRowErrors = false;
            this.grdFibonacci.Size = new System.Drawing.Size(250, 208);
            this.grdFibonacci.TabIndex = 0;
            this.grdFibonacci.Text = "grdFibonacci";
            this.grdFibonacci.ThemeName = "ControlDefault";
            this.grdFibonacci.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.grdFibonacci_CellFormatting);
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(15, 52);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(33, 18);
            this.lblColor.TabIndex = 1;
            this.lblColor.Text = "Color";
            // 
            // rseLineThickness
            // 
            this.rseLineThickness.Location = new System.Drawing.Point(112, 26);
            this.rseLineThickness.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.rseLineThickness.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rseLineThickness.Name = "rseLineThickness";
            // 
            // 
            // 
            this.rseLineThickness.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.rseLineThickness.Size = new System.Drawing.Size(100, 20);
            this.rseLineThickness.TabIndex = 7;
            this.rseLineThickness.TabStop = false;
            this.rseLineThickness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});

            ((System.ComponentModel.ISupportInitialize)(this.lblLineThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFibonacci)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFibonacci)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rseLineThickness)).EndInit();
        }

        protected void NewLoadStudies()
        {
            this.pgrdStudies = new Telerik.WinControls.UI.RadPropertyGrid();
            // 
            // tabStudies
            // 
            this.tabStudies.Controls.Add(this.pgrdStudies);
            // 
            // pgrdPrice
            // 
            this.pgrdStudies.EnableGrouping = false;
            this.pgrdStudies.EnableKineticScrolling = true;
            this.pgrdStudies.EnableSorting = false;
            this.pgrdStudies.Location = new System.Drawing.Point(16, 26);
            this.pgrdStudies.Name = "pgrdStudies";
            this.pgrdStudies.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdStudies.Size = new System.Drawing.Size(346, 260);
            this.pgrdStudies.TabIndex = 20;
            this.pgrdStudies.Text = "radPropertyGrid1";
        }

        protected void LoadChart()
        {
            this.pgrdChart = new Telerik.WinControls.UI.RadPropertyGrid();
            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.pgrdChart);
            // 
            // pgrdPrice
            // 
            this.pgrdChart.EnableGrouping = false;
            this.pgrdChart.EnableKineticScrolling = true;
            this.pgrdChart.EnableSorting = false;
            this.pgrdChart.Location = new System.Drawing.Point(16, 26);
            this.pgrdChart.Name = "pgrdChart";
            this.pgrdChart.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdChart.Size = new System.Drawing.Size(346, 260);
            this.pgrdChart.TabIndex = 20;
            this.pgrdChart.Text = "radPropertyGrid1";
        }

        protected void LoadPrice()
        {
            this.pgrdPrice = new Telerik.WinControls.UI.RadPropertyGrid();

            // 
            // grpPrice
            // 
            this.tabPrice.Controls.Add(this.pgrdPrice);
            // 
            // pgrdPrice
            // 
            this.pgrdPrice.EnableGrouping = false;
            this.pgrdPrice.EnableKineticScrolling = true;
            this.pgrdPrice.EnableSorting = false;
            this.pgrdPrice.Location = new System.Drawing.Point(16, 26);
            this.pgrdPrice.Name = "pgrdPrice";
            this.pgrdPrice.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdPrice.Size = new System.Drawing.Size(346, 260);
            this.pgrdPrice.TabIndex = 20;
            this.pgrdPrice.Text = "radPropertyGrid1";
        }

        protected void LoadUser()
        {
            this.pgrdUser = new Telerik.WinControls.UI.RadPropertyGrid();

            // 
            // tabUser
            // 
            this.tabUser.Controls.Add(this.pgrdUser);
            // 
            // pgrdUser
            // 
            this.pgrdUser.EnableGrouping = false;
            this.pgrdUser.EnableKineticScrolling = true;
            this.pgrdUser.EnableSorting = false;
            this.pgrdUser.Location = new System.Drawing.Point(16, 26);
            this.pgrdUser.Name = "pgrdUser";
            this.pgrdUser.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdUser.Size = new System.Drawing.Size(346, 260);
            this.pgrdUser.TabIndex = 20;
            this.pgrdUser.Text = "radPropertyGrid1";
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel panel;
        private Telerik.WinControls.UI.RadPageView pgvSettings;
        private Telerik.WinControls.UI.RadPageViewPage tabProxy;
        private Telerik.WinControls.UI.RadPageViewPage tabStudies;
        private Telerik.WinControls.UI.RadPageViewPage tabPrice;
        private Telerik.WinControls.UI.RadPageViewPage tabChart;
        private Telerik.WinControls.UI.RadPageViewPage tabUser;

        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadTextBox txtPasswordAuthenticationProxy;
        private Telerik.WinControls.UI.RadTextBox txtPortProxySocks;
        private Telerik.WinControls.UI.RadLabel lblPasswordAuthenticationProxy;
        private Telerik.WinControls.UI.RadTextBox txtUserAuthenticationProxy;
        private Telerik.WinControls.UI.RadLabel lblPortProxySocks;
        private Telerik.WinControls.UI.RadLabel lblUserAuthenticationProxy;
        private Telerik.WinControls.UI.RadTextBox txtPortProxy;
        private Telerik.WinControls.UI.RadTextBox txtServerURL;
        private Telerik.WinControls.UI.RadTextBox txtSuperscriptionProxySocks;
        private Telerik.WinControls.UI.RadLabel lblPortProxy;
        private Telerik.WinControls.UI.RadLabel lblServerURL;
        private Telerik.WinControls.UI.RadLabel lblSuperscriptionProxySocks;
        private Telerik.WinControls.UI.RadTextBox txtSuperscriptionProxy;
        private Telerik.WinControls.UI.RadLabel lblSuperscriptionProxy;
        private Telerik.WinControls.UI.RadCheckBox cbxAuthenticationProxy;
        private Telerik.WinControls.UI.RadRadioButton optProxySocks;
        private Telerik.WinControls.UI.RadRadioButton optProxyServer;
        private Telerik.WinControls.UI.RadRadioButton optConfigProxyNavigator;
        private Telerik.WinControls.UI.RadRadioButton optNotProxy;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadLabel lblLineThickness;
        private Telerik.WinControls.UI.RadLabel lblColor;
        private Telerik.WinControls.UI.RadColorBox boxColor;
        private Telerik.WinControls.UI.RadSpinEditor rseLineThickness;
        private Telerik.WinControls.UI.RadGridView grdFibonacci;
        private Telerik.WinControls.UI.RadLabel lblFibonacci;
        private System.Windows.Forms.GroupBox grpSettingsServer;
        private Telerik.WinControls.UI.RadTextBox txtServerPort3;
        private Telerik.WinControls.UI.RadTextBox txtServerPort2;
        private Telerik.WinControls.UI.RadLabel lblPort3;
        private Telerik.WinControls.UI.RadLabel lblPort2;
        private Telerik.WinControls.UI.RadTextBox txtServer3;
        private Telerik.WinControls.UI.RadLabel lblServer3;
        private Telerik.WinControls.UI.RadTextBox txtServerPort1;
        private Telerik.WinControls.UI.RadTextBox txtServer2;
        private Telerik.WinControls.UI.RadLabel lblPort1;
        private Telerik.WinControls.UI.RadLabel lblServer2;
        private Telerik.WinControls.UI.RadTextBox txtServer1;
        private Telerik.WinControls.UI.RadLabel lblServer1;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdPrice;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdChart;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdStudies;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdUser;

    }
}
