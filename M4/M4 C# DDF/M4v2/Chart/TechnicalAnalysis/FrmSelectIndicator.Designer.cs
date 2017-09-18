using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.TechnicalAnalysis
{
    partial class FrmSelectIndicator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectIndicator));
            this.btnDismiss = new Telerik.WinControls.UI.RadButton();
            this.lblIndicators = new Telerik.WinControls.UI.RadLabel();
            this.trvIndicators = new Telerik.WinControls.UI.RadTreeView();
            this.pgrdIndicators = new Telerik.WinControls.UI.RadPropertyGrid();
            this.btnApply = new Telerik.WinControls.UI.RadButton();
            this.btnNew = new Telerik.WinControls.UI.RadButton();
            this.btnRemove = new Telerik.WinControls.UI.RadButton();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndicators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvIndicators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgrdIndicators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDismiss
            // 
            this.btnDismiss.Location = new System.Drawing.Point(366, 407);
            this.btnDismiss.Name = "btnDismiss";
            this.btnDismiss.Size = new System.Drawing.Size(83, 24);
            this.btnDismiss.TabIndex = 4;
            this.btnDismiss.Text = "Dismiss";
            this.btnDismiss.Click += new System.EventHandler(this.BtnDismissClick);
            // 
            // lblIndicators
            // 
            this.lblIndicators.Location = new System.Drawing.Point(13, 11);
            this.lblIndicators.Name = "lblIndicators";
            this.lblIndicators.Size = new System.Drawing.Size(55, 18);
            this.lblIndicators.TabIndex = 6;
            this.lblIndicators.Text = "Indicators";
            // 
            // trvIndicators
            // 
            this.trvIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trvIndicators.EnableKineticScrolling = true;
            this.trvIndicators.Location = new System.Drawing.Point(13, 31);
            this.trvIndicators.Name = "trvIndicators";
            this.trvIndicators.ShowLines = true;
            this.trvIndicators.ShowNodeToolTips = true;
            this.trvIndicators.Size = new System.Drawing.Size(235, 370);
            this.trvIndicators.SpacingBetweenNodes = -1;
            this.trvIndicators.TabIndex = 18;
            this.trvIndicators.Text = "radTreeView1";
            this.trvIndicators.SelectedNodeChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.trvIndicators_SelectedNodeChanging);
            this.trvIndicators.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.TrvIndicatorsSelectedNodeChanged);
            this.trvIndicators.NodeFormatting += new Telerik.WinControls.UI.TreeNodeFormattingEventHandler(this.RadTreeView1NodeFormatting);
            this.trvIndicators.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.trvIndicators_PreviewKeyDown);
            // 
            // pgrdIndicators
            // 
            this.pgrdIndicators.EnableGrouping = false;
            this.pgrdIndicators.EnableKineticScrolling = true;
            this.pgrdIndicators.EnableSorting = false;
            this.pgrdIndicators.Location = new System.Drawing.Point(246, 31);
            this.pgrdIndicators.Name = "pgrdIndicators";
            this.pgrdIndicators.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdIndicators.Size = new System.Drawing.Size(203, 370);
            this.pgrdIndicators.TabIndex = 19;
            this.pgrdIndicators.Text = "radPropertyGrid1";
            this.pgrdIndicators.ItemFormatting += new Telerik.WinControls.UI.PropertyGridItemFormattingEventHandler(this.pgrdIndicators_ItemFormatting);
            // 
            // btnApply
            // 
            this.btnApply.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply.Location = new System.Drawing.Point(43, 407);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(24, 24);
            this.btnApply.TabIndex = 5;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNew.Location = new System.Drawing.Point(13, 407);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(24, 24);
            this.btnNew.TabIndex = 6;
            this.btnNew.Click += new System.EventHandler(this.BtnNewClick);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemove.Location = new System.Drawing.Point(73, 407);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // FrmSelectIndicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 436);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.pgrdIndicators);
            this.Controls.Add(this.trvIndicators);
            this.Controls.Add(this.lblIndicators);
            this.Controls.Add(this.btnDismiss);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectIndicator";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indicators";
            this.ThemeName = "ControlDefault";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSelectIndicator_FormClosing);
            this.Shown += new System.EventHandler(this.FrmSelectIndicator_Shown);
            this.VisibleChanged += new System.EventHandler(this.FrmSelectIndicator_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndicators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvIndicators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgrdIndicators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Surface surface1;
        private Telerik.WinControls.UI.RadButton btnDismiss;
        private Telerik.WinControls.UI.RadLabel lblIndicators;
        private Telerik.WinControls.UI.RadTreeView trvIndicators;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdIndicators;
        private Telerik.WinControls.UI.RadButton btnApply;
        private Telerik.WinControls.UI.RadButton btnNew;
        private Telerik.WinControls.UI.RadButton btnRemove;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
    }
}
