namespace M4.M4v2.TradeDiary
{
    partial class EditDiarySymbols
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
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radListControl2 = new Telerik.WinControls.UI.RadListControl();
            this.radListControl1 = new Telerik.WinControls.UI.RadListControl();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.lblStock = new Telerik.WinControls.UI.RadLabel();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.ddlFilter = new Telerik.WinControls.UI.RadDropDownList();
            this.stockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(160, 136);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(51, 24);
            this.radButton2.TabIndex = 35;
            this.radButton2.Text = "<<";
            this.radButton2.Click += new System.EventHandler(this.RadButton2Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(160, 106);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(51, 24);
            this.radButton1.TabIndex = 34;
            this.radButton1.Text = ">>";
            this.radButton1.Click += new System.EventHandler(this.RadButton1Click);
            // 
            // radListControl2
            // 
            this.radListControl2.CaseSensitiveSort = true;
            this.radListControl2.ItemHeight = 18;
            this.radListControl2.Location = new System.Drawing.Point(217, 43);
            this.radListControl2.Name = "radListControl2";
            this.radListControl2.Size = new System.Drawing.Size(146, 185);
            this.radListControl2.TabIndex = 33;
            this.radListControl2.Text = "radListControl2";
            // 
            // radListControl1
            // 
            this.radListControl1.CaseSensitiveSort = true;
            this.radListControl1.ItemHeight = 18;
            this.radListControl1.Location = new System.Drawing.Point(8, 43);
            this.radListControl1.Name = "radListControl1";
            this.radListControl1.Size = new System.Drawing.Size(146, 185);
            this.radListControl1.TabIndex = 32;
            this.radListControl1.Text = "radListControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 24);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // lblStock
            // 
            this.lblStock.Location = new System.Drawing.Point(62, -16);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(2, 2);
            this.lblStock.TabIndex = 28;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(221, 242);
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
            this.ddlFilter.DropDownAnimationEnabled = true;
            this.ddlFilter.FormatString = "{0}";
            this.ddlFilter.Location = new System.Drawing.Point(8, 12);
            this.ddlFilter.MaxDropDownItems = 1000;
            this.ddlFilter.Name = "ddlFilter";
            this.ddlFilter.ShowImageInEditorArea = true;
            this.ddlFilter.Size = new System.Drawing.Size(355, 20);
            this.ddlFilter.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Ascending;
            this.ddlFilter.TabIndex = 36;
            this.ddlFilter.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DdlFilterSelectedIndexChanged);
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.ddlFilter.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
            // 
            // stockBindingSource
            // 
            this.stockBindingSource.DataSource = typeof(M4Core.Entities.Stock);
            // 
            // EditDiarySymbols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 274);
            this.ControlBox = false;
            this.Controls.Add(this.ddlFilter);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radListControl2);
            this.Controls.Add(this.radListControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.btnOk);
            this.Name = "EditDiarySymbols";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Trade Diary";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadListControl radListControl2;
        private Telerik.WinControls.UI.RadListControl radListControl1;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadLabel lblStock;
        private System.Windows.Forms.BindingSource stockBindingSource;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadDropDownList ddlFilter;
    }
}
