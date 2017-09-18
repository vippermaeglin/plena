namespace M4.M4v2.Portfolio
{
    partial class DataManager
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.commandBarRowElement2 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.grdAtivos = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdAtivos)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarRowElement2
            // 
            this.commandBarRowElement2.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarRowElement2.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarRowElement2.DisplayName = null;
            this.commandBarRowElement2.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement2.Text = "";
            // 
            // grdAtivos
            // 
            this.grdAtivos.AutoScroll = true;
            this.grdAtivos.AutoSizeRows = true;
            this.grdAtivos.BackColor = System.Drawing.Color.Transparent;
            this.grdAtivos.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdAtivos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAtivos.Font = new System.Drawing.Font("Arial", 8.25F);
            this.grdAtivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(143)))), ((int)(((byte)(160)))));
            this.grdAtivos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.grdAtivos.Location = new System.Drawing.Point(0, 0);
            // 
            // grdAtivos
            // 
            this.grdAtivos.MasterTemplate.AllowAddNewRow = false;
            this.grdAtivos.MasterTemplate.AllowDeleteRow = false;
            this.grdAtivos.MasterTemplate.AllowEditRow = false;
            this.grdAtivos.MasterTemplate.AllowRowReorder = true;
            this.grdAtivos.MasterTemplate.AllowRowResize = false;
            this.grdAtivos.MasterTemplate.AutoGenerateColumns = false;
            this.grdAtivos.MasterTemplate.Caption = "Products";
            gridViewTextBoxColumn1.AllowHide = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "Symbol";
            gridViewTextBoxColumn1.HeaderText = "Symbol";
            gridViewTextBoxColumn1.Name = "Symbol";
            gridViewTextBoxColumn1.RowSpan = 5;
            gridViewTextBoxColumn1.Width = 5;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Last";
            gridViewTextBoxColumn2.HeaderText = "Last";
            gridViewTextBoxColumn2.Name = "Last";
            gridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn2.Width = 5;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Time";
            gridViewTextBoxColumn3.HeaderText = "Time";
            gridViewTextBoxColumn3.Name = "Time";
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn3.Width = 5;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Variation";
            gridViewTextBoxColumn4.HeaderText = "%";
            gridViewTextBoxColumn4.Name = "Variation";
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn4.Width = 5;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "High";
            gridViewTextBoxColumn5.HeaderText = "High";
            gridViewTextBoxColumn5.Name = "High";
            gridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn5.Width = 5;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "Low";
            gridViewTextBoxColumn6.HeaderText = "Low";
            gridViewTextBoxColumn6.Name = "Low";
            gridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn6.Width = 5;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "Close";
            gridViewTextBoxColumn7.HeaderText = "Close";
            gridViewTextBoxColumn7.Name = "Close";
            gridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn7.Width = 5;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "Open";
            gridViewTextBoxColumn8.HeaderText = "Open";
            gridViewTextBoxColumn8.Name = "Open";
            gridViewTextBoxColumn8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn8.Width = 5;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.FieldName = "Trades";
            gridViewTextBoxColumn9.HeaderText = "Trades";
            gridViewTextBoxColumn9.Name = "Trades";
            gridViewTextBoxColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn9.Width = 5;
            gridViewTextBoxColumn10.EnableExpressionEditor = false;
            gridViewTextBoxColumn10.FieldName = "Volume";
            gridViewTextBoxColumn10.HeaderText = "Volume";
            gridViewTextBoxColumn10.Name = "Volume";
            gridViewTextBoxColumn10.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn10.Width = 20;
            this.grdAtivos.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10});
            this.grdAtivos.MasterTemplate.EnableGrouping = false;
            this.grdAtivos.MasterTemplate.EnableSorting = false;
            this.grdAtivos.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.grdAtivos.MasterTemplate.ShowFilteringRow = false;
            this.grdAtivos.MasterTemplate.ShowRowHeaderColumn = false;
            this.grdAtivos.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.grdAtivos.Name = "grdAtivos";
            this.grdAtivos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.grdAtivos.RootElement.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(143)))), ((int)(((byte)(160)))));
            this.grdAtivos.ShowGroupPanel = false;
            this.grdAtivos.Size = new System.Drawing.Size(688, 455);
            this.grdAtivos.TabIndex = 2;
            this.grdAtivos.Text = "radGridView1";
            this.grdAtivos.ThemeName = "BusinessGrid";
            this.grdAtivos.RowFormatting += new Telerik.WinControls.UI.RowFormattingEventHandler(GrdAtivosRowFormatting);
            this.grdAtivos.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.grdAtivos_CellFormatting);
            this.grdAtivos.SizeChanged += new System.EventHandler(this.GrdAtivosSizeChanged);
            this.grdAtivos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MasterTemplateKeyDown);
            // 
            // DataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdAtivos);
            this.Name = "DataManager";
            this.Size = new System.Drawing.Size(688, 455);
            ((System.ComponentModel.ISupportInitialize)(this.grdAtivos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement2;
        private Telerik.WinControls.UI.RadGridView grdAtivos;

    }
}
