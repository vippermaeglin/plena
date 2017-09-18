namespace M4.M4v2.Portfolio
{
    partial class EditPortfolio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPortfolio));
            this.btnRemovePortList = new Telerik.WinControls.UI.RadButton();
            this.btnAddPortList = new Telerik.WinControls.UI.RadButton();
            this.radListControl2 = new Telerik.WinControls.UI.RadListControl();
            this.radListControl1 = new Telerik.WinControls.UI.RadListControl();
            this.ddlPortfolios = new Telerik.WinControls.UI.RadDropDownList();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.lblStock = new Telerik.WinControls.UI.RadLabel();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.ddlFilter = new Telerik.WinControls.UI.RadDropDownList();
            this.btnApply = new Telerik.WinControls.UI.RadButton();
            this.btnDeletePort = new Telerik.WinControls.UI.RadButton();
            this.radSeparator1 = new Telerik.WinControls.UI.RadSeparator();
            this.btnAddPort = new Telerik.WinControls.UI.RadButton();
            this.lblSearchPort = new Telerik.WinControls.UI.RadLabel();
            this.stockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnRemovePortList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPortList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPortfolios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeletePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRemovePortList
            // 
            this.btnRemovePortList.Location = new System.Drawing.Point(160, 181);
            this.btnRemovePortList.Name = "btnRemovePortList";
            this.btnRemovePortList.Size = new System.Drawing.Size(51, 24);
            this.btnRemovePortList.TabIndex = 35;
            this.btnRemovePortList.Text = "<<";
            this.btnRemovePortList.Click += new System.EventHandler(this.RadButton2Click);
            // 
            // btnAddPortList
            // 
            this.btnAddPortList.Location = new System.Drawing.Point(160, 151);
            this.btnAddPortList.Name = "btnAddPortList";
            this.btnAddPortList.Size = new System.Drawing.Size(51, 24);
            this.btnAddPortList.TabIndex = 34;
            this.btnAddPortList.Text = ">>";
            this.btnAddPortList.Click += new System.EventHandler(this.RadButton1Click);
            // 
            // radListControl2
            // 
            this.radListControl2.Location = new System.Drawing.Point(217, 85);
            this.radListControl2.Name = "radListControl2";
            this.radListControl2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.radListControl2.Size = new System.Drawing.Size(146, 185);
            this.radListControl2.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Ascending;
            this.radListControl2.TabIndex = 33;
            this.radListControl2.SelectedIndexChanged += radListControl2_SelectedIndexChanged;
            // 
            // radListControl1
            // 
            this.radListControl1.Location = new System.Drawing.Point(8, 85);
            this.radListControl1.Name = "radListControl1";
            this.radListControl1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.radListControl1.Size = new System.Drawing.Size(146, 185);
            this.radListControl1.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Ascending;
            this.radListControl1.TabIndex = 32;
            this.radListControl1.SelectedIndexChanged += radListControl1_SelectedIndexChanged;
            // 
            // ddlPortfolios
            // 
            this.ddlPortfolios.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlPortfolios.AutoSizeItems = true;
            this.ddlPortfolios.BackColor = System.Drawing.Color.Transparent;
            this.ddlPortfolios.FormatString = "{0}";
            this.ddlPortfolios.Location = new System.Drawing.Point(8, 12);
            this.ddlPortfolios.MaxDropDownItems = 1000;
            this.ddlPortfolios.Name = "ddlPortfolios";
            this.ddlPortfolios.Size = new System.Drawing.Size(293, 22);
            this.ddlPortfolios.TabIndex = 31;
            this.ddlPortfolios.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DdlPortfoliosSelectedIndexChanged);
            this.ddlPortfolios.TextChanged += new System.EventHandler(this.ddlPortfolios_TextChanged);
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.ddlPortfolios.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 24);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // lblStock
            // 
            this.lblStock.Location = new System.Drawing.Point(61, 18);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(2, 2);
            this.lblStock.TabIndex = 28;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(147, 286);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 24);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // ddlFilter
            // 
            this.ddlFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlFilter.AutoSizeItems = true;
            this.ddlFilter.BackColor = System.Drawing.Color.Transparent;
            this.ddlFilter.FormatString = "{0}";
            this.ddlFilter.Location = new System.Drawing.Point(83, 54);
            this.ddlFilter.MaxDropDownItems = 1000;
            this.ddlFilter.Name = "ddlFilter";
            this.ddlFilter.Size = new System.Drawing.Size(212, 22);
            this.ddlFilter.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Ascending;
            this.ddlFilter.TabIndex = 36;
            this.ddlFilter.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DdlFilterSelectedIndexChanged);
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.ddlFilter.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(221, 286);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(68, 24);
            this.btnApply.TabIndex = 27;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // btnDeletePort
            // 
            this.btnDeletePort.Image = global::M4.Properties.Resources.remove;
            this.btnDeletePort.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDeletePort.Location = new System.Drawing.Point(342, 10);
            this.btnDeletePort.Name = "btnDeletePort";
            this.btnDeletePort.Size = new System.Drawing.Size(28, 28);
            this.btnDeletePort.TabIndex = 38;
            this.btnDeletePort.Click += new System.EventHandler(this.RadButton3Click);
            // 
            // radSeparator1
            // 
            this.radSeparator1.Location = new System.Drawing.Point(8, 38);
            this.radSeparator1.Name = "radSeparator1";
            this.radSeparator1.Size = new System.Drawing.Size(360, 10);
            this.radSeparator1.TabIndex = 40;
            this.radSeparator1.Text = "radSeparator1";
            // 
            // btnAddPort
            // 
            this.btnAddPort.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPort.Image")));
            this.btnAddPort.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddPort.Location = new System.Drawing.Point(312, 10);
            this.btnAddPort.Name = "btnAddPort";
            this.btnAddPort.Size = new System.Drawing.Size(28, 28);
            this.btnAddPort.TabIndex = 37;
            this.btnAddPort.Click += new System.EventHandler(this.BtnAddPortClick);
            // 
            // lblSearchPort
            // 
            this.lblSearchPort.Location = new System.Drawing.Point(24, 54);
            this.lblSearchPort.Name = "lblSearchPort";
            this.lblSearchPort.Size = new System.Drawing.Size(39, 18);
            this.lblSearchPort.TabIndex = 41;
            this.lblSearchPort.Text = "Search";
            // 
            // stockBindingSource
            // 
            this.stockBindingSource.DataSource = typeof(M4Core.Entities.Stock);
            // 
            // EditPortfolio
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(370, 321);
            this.Controls.Add(this.lblSearchPort);
            this.Controls.Add(this.radSeparator1);
            this.Controls.Add(this.btnDeletePort);
            this.Controls.Add(this.btnAddPort);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.ddlFilter);
            this.Controls.Add(this.btnRemovePortList);
            this.Controls.Add(this.btnAddPortList);
            this.Controls.Add(this.radListControl2);
            this.Controls.Add(this.radListControl1);
            this.Controls.Add(this.ddlPortfolios);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPortfolio";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Portfolio";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.EditPortfolio_Load);
            this.Shown +=EditPortfolio_Shown;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditPortfolioKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.btnRemovePortList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPortList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPortfolios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeletePort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnRemovePortList;
        private Telerik.WinControls.UI.RadButton btnAddPortList;
        private Telerik.WinControls.UI.RadListControl radListControl2;
        private Telerik.WinControls.UI.RadListControl radListControl1;
        private Telerik.WinControls.UI.RadDropDownList ddlPortfolios;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadLabel lblStock;
        private System.Windows.Forms.BindingSource stockBindingSource;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadDropDownList ddlFilter;
        private Telerik.WinControls.UI.RadButton btnApply;
        private Telerik.WinControls.UI.RadButton btnAddPort;
        private Telerik.WinControls.UI.RadButton btnDeletePort;
        private Telerik.WinControls.UI.RadSeparator radSeparator1;
        private Telerik.WinControls.UI.RadLabel lblSearchPort;
    }
}
