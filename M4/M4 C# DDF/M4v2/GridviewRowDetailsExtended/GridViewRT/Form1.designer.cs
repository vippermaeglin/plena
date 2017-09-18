namespace M4.M4v2.GridviewRowDetailsExtended.GridViewRT
{
    partial class GridDataAssetsRT
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
            Telerik.WinControls.UI.GridViewRelation gridViewRelation1 = new Telerik.WinControls.UI.GridViewRelation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridDataAssetsRT));
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.employeesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.timerTick = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.AllowDrop = true;
            this.radGridView1.BackColor = System.Drawing.SystemColors.Control;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.EnableHotTracking = false;
            this.radGridView1.ForeColor = System.Drawing.Color.Black;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            this.radGridView1.AllowDrop = true;
            // 
            // radGridView1
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnChooser = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AllowRowReorder = true;
            this.radGridView1.MasterTemplate.AllowRowResize = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.EnableAlternatingRowColor = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.Name = "radGridView1";
            gridViewRelation1.ChildColumnNames = ((System.Collections.Specialized.StringCollection)(resources.GetObject("gridViewRelation1.ChildColumnNames")));
            gridViewRelation1.ParentColumnNames = ((System.Collections.Specialized.StringCollection)(resources.GetObject("gridViewRelation1.ParentColumnNames")));
            gridViewRelation1.ParentTemplate = this.radGridView1.MasterTemplate;
            gridViewRelation1.RelationName = null;
            this.radGridView1.Relations.AddRange(new Telerik.WinControls.UI.GridViewRelation[] {
            gridViewRelation1});
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.Size = new System.Drawing.Size(1200, 1000);
            this.radGridView1.TabIndex = 1;
            this.radGridView1.CreateCell += new Telerik.WinControls.UI.GridViewCreateCellEventHandler(this.radGridView1_CreateCell);
            this.radGridView1.RowFormatting += new Telerik.WinControls.UI.RowFormattingEventHandler(this.radGridView1_RowFormatting);
            this.radGridView1.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridView1_CellFormatting);
            this.radGridView1.ChildViewExpanded += new Telerik.WinControls.UI.ChildViewExpandedEventHandler(this.radGridView1_ChildViewExpanded);
            this.radGridView1.ChildViewExpanding += new Telerik.WinControls.UI.ChildViewExpandingEventHandler(this.radGridView1_ChildViewExpanding);
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellDoubleClick);
            this.radGridView1.ColumnIndexChanged += new Telerik.WinControls.UI.ColumnIndexChangedEventHandler(this.radGridView1_ColumnIndexChanged);
            this.radGridView1.ColumnIndexChanging += new Telerik.WinControls.UI.ColumnIndexChangingEventHandler(this.radGridView1_ColumnIndexChanging);
            this.radGridView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.radGridView1_ColumnWidthChanged);
            this.radGridView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.radGridView1_ColumnWidthChanging);
            this.radGridView1.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.radGridView1_ContextMenuOpening);
            this.radGridView1.CurrentViewChanged += new Telerik.WinControls.UI.GridViewCurrentViewChangedEventHandler(this.radGridView1_CurrentViewChanged);
            this.radGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radGridView1_MouseDown);
            this.radGridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.radGridView1_MouseMove);
            this.radGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.radGridView1_MouseUp);
            this.radGridView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.RadGridView1PreviewKeyDown);
            // 
            // timerTick
            // 
            this.timerTick.Interval = 200;
            this.timerTick.Tick += new System.EventHandler(this.timerTick_Tick);
            // 
            // GridDataAssetsRT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.radGridView1);
            this.Name = "GridDataAssetsRT";
            this.Size = new System.Drawing.Size(1200, 1000);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.BindingSource employeesBindingSource;
        private System.Windows.Forms.Timer timerTick;
    }
}
