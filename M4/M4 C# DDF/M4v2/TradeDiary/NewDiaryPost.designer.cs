namespace M4.M4v2.Portfolio
{
    partial class NewDiaryPost
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
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.lblDescriptionPeriodicity = new Telerik.WinControls.UI.RadLabel();
            this.radSeparator1 = new Telerik.WinControls.UI.RadSeparator();
            this.lblStock = new Telerik.WinControls.UI.RadLabel();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.radSeparator2 = new Telerik.WinControls.UI.RadSeparator();
            this.ddlFilter = new Telerik.WinControls.UI.RadDropDownList();
            this.stockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.LblDate = new Telerik.WinControls.UI.RadLabel();
            this.TxtDate = new Telerik.WinControls.UI.RadTextBox();
            this.radCalendar1 = new Telerik.WinControls.UI.RadCalendar();
            this.LblTime = new Telerik.WinControls.UI.RadLabel();
            this.TxtTime = new Telerik.WinControls.UI.RadMaskedEditBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescriptionPeriodicity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LblTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(207, 289);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 24);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDescriptionPeriodicity
            // 
            this.lblDescriptionPeriodicity.Location = new System.Drawing.Point(109, 9);
            this.lblDescriptionPeriodicity.Name = "lblDescriptionPeriodicity";
            this.lblDescriptionPeriodicity.Size = new System.Drawing.Size(74, 18);
            this.lblDescriptionPeriodicity.TabIndex = 29;
            this.lblDescriptionPeriodicity.TabStop = true;
            this.lblDescriptionPeriodicity.Text = Program.LanguageDefault.DictionaryPlena["tradeDiary"].ToUpper();
            // 
            // radSeparator1
            // 
            this.radSeparator1.Location = new System.Drawing.Point(7, 11);
            this.radSeparator1.Name = "radSeparator1";
            this.radSeparator1.ShadowOffset = new System.Drawing.Point(0, 0);
            this.radSeparator1.Size = new System.Drawing.Size(268, 12);
            this.radSeparator1.TabIndex = 30;
            this.radSeparator1.Text = "radSeparator1";
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
            this.btnOk.Location = new System.Drawing.Point(122, 289);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 24);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // radSeparator2
            // 
            this.radSeparator2.Location = new System.Drawing.Point(8, 271);
            this.radSeparator2.Name = "radSeparator2";
            this.radSeparator2.ShadowOffset = new System.Drawing.Point(0, 0);
            this.radSeparator2.Size = new System.Drawing.Size(267, 12);
            this.radSeparator2.TabIndex = 31;
            this.radSeparator2.Text = "radSeparator2";
            // 
            // ddlFilter
            // 
            this.ddlFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlFilter.AutoSizeItems = true;
            this.ddlFilter.BackColor = System.Drawing.Color.Transparent;
            this.ddlFilter.DropDownAnimationEnabled = true;
            this.ddlFilter.FormatString = "{0}";
            this.ddlFilter.Location = new System.Drawing.Point(58, 33);
            this.ddlFilter.MaxDropDownItems = 1000;
            this.ddlFilter.Name = "ddlFilter";
            this.ddlFilter.ShowImageInEditorArea = true;
            this.ddlFilter.Size = new System.Drawing.Size(217, 20);
            this.ddlFilter.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Ascending;
            this.ddlFilter.TabIndex = 36;
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.ddlFilter.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
            // 
            // stockBindingSource
            // 
            this.stockBindingSource.DataSource = typeof(M4Core.Entities.Stock);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(6, 33);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(46, 18);
            this.radLabel1.TabIndex = 30;
            this.radLabel1.TabStop = true;
            this.radLabel1.Text = "Symbol:";
            // 
            // LblDate
            // 
            this.LblDate.Location = new System.Drawing.Point(19, 83);
            this.LblDate.Name = "LblDate";
            this.LblDate.Size = new System.Drawing.Size(32, 18);
            this.LblDate.TabIndex = 31;
            this.LblDate.TabStop = true;
            this.LblDate.Text = "Date:";
            // 
            // TxtDate
            // 
            this.TxtDate.Location = new System.Drawing.Point(58, 83);
            this.TxtDate.Name = "TxtDate";
            this.TxtDate.ReadOnly = true;
            this.TxtDate.Size = new System.Drawing.Size(61, 20);
            this.TxtDate.TabIndex = 37;
            this.TxtDate.TabStop = false;
            // 
            // radCalendar1
            // 
            this.radCalendar1.CellAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radCalendar1.CellMargin = new System.Windows.Forms.Padding(0);
            this.radCalendar1.CellPadding = new System.Windows.Forms.Padding(0);
            this.radCalendar1.HeaderHeight = 17;
            this.radCalendar1.HeaderWidth = 17;
            this.radCalendar1.Location = new System.Drawing.Point(8, 109);
            this.radCalendar1.Name = "radCalendar1";
            this.radCalendar1.RangeMaxDate = new System.DateTime(2099, 12, 30, 0, 0, 0, 0);
            this.radCalendar1.Size = new System.Drawing.Size(267, 156);
            this.radCalendar1.TabIndex = 38;
            this.radCalendar1.Text = "radCalendar1";
            this.radCalendar1.SelectionChanged += new System.EventHandler(this.radCalendar1_SelectionChanged);
            this.radCalendar1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radCalendar1_KeyPress);
            // 
            // LblTime
            // 
            this.LblTime.Location = new System.Drawing.Point(19, 57);
            this.LblTime.Name = "LblTime";
            this.LblTime.Size = new System.Drawing.Size(33, 18);
            this.LblTime.TabIndex = 38;
            this.LblTime.TabStop = true;
            this.LblTime.Text = "Time:";
            // 
            // TxtTime
            // 
            this.TxtTime.AllowPromptAsInput = false;
            this.TxtTime.AutoSize = true;
            this.TxtTime.Location = new System.Drawing.Point(57, 57);
            this.TxtTime.Mask = "90:00";
            this.TxtTime.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.TxtTime.Name = "TxtTime";
            this.TxtTime.Size = new System.Drawing.Size(32, 20);
            this.TxtTime.TabIndex = 39;
            this.TxtTime.TabStop = false;
            this.TxtTime.Text = "__:__";
            this.TxtTime.Value = "  :";
            // 
            // NewDiaryPost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 316);
            this.ControlBox = false;
            this.Controls.Add(this.lblDescriptionPeriodicity);
            this.Controls.Add(this.TxtTime);
            this.Controls.Add(this.LblTime);
            this.Controls.Add(this.radCalendar1);
            this.Controls.Add(this.TxtDate);
            this.Controls.Add(this.LblDate);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.ddlFilter);
            this.Controls.Add(this.radSeparator2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.radSeparator1);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.btnOk);
            this.Name = "NewDiaryPost";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = Program.LanguageDefault.DictionaryPlena["tradeDiary"];
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescriptionPeriodicity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LblTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadLabel lblDescriptionPeriodicity;
        private Telerik.WinControls.UI.RadSeparator radSeparator1;
        private Telerik.WinControls.UI.RadLabel lblStock;
        private System.Windows.Forms.BindingSource stockBindingSource;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadSeparator radSeparator2;
        private Telerik.WinControls.UI.RadDropDownList ddlFilter;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel LblDate;
        private Telerik.WinControls.UI.RadTextBox TxtDate;
        private Telerik.WinControls.UI.RadCalendar radCalendar1;
        private Telerik.WinControls.UI.RadLabel LblTime;
        private Telerik.WinControls.UI.RadMaskedEditBox TxtTime;
    }
}
