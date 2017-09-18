
namespace M4.M4v2.Chart.PriceSettings
{
    partial class FrmPriceSettings
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
            this.tabPrice = new Telerik.WinControls.UI.RadPageViewPage();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.cbApplyAll = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pgvSettings)).BeginInit();
            this.pgvSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbApplyAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pgvSettings
            // 
            this.pgvSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgvSettings.Controls.Add(this.tabPrice);
            this.pgvSettings.Location = new System.Drawing.Point(-3, 0);
            this.pgvSettings.Name = "pgvSettings";
            this.pgvSettings.SelectedPage = this.tabPrice;
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
            // tabPrice
            // 
            this.tabPrice.Location = new System.Drawing.Point(10, 37);
            this.tabPrice.Name = "tabPrice";
            this.tabPrice.Size = new System.Drawing.Size(371, 279);
            this.tabPrice.Text = "Price";
            this.tabPrice.Paint += new System.Windows.Forms.PaintEventHandler(this.TabPricePaint);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(218, 333);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 24);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.BtnConfirmClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(293, 333);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelarClick);
            // 
            // cbApplyAll
            // 
            this.cbApplyAll.Location = new System.Drawing.Point(12, 339);
            this.cbApplyAll.Name = "cbApplyAll";
            this.cbApplyAll.Size = new System.Drawing.Size(91, 18);
            this.cbApplyAll.TabIndex = 22;
            this.cbApplyAll.Text = "radCheckBox1";
            // 
            // FrmPriceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 362);
            this.Controls.Add(this.cbApplyAll);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pgvSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPriceSettings";
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
            ((System.ComponentModel.ISupportInitialize)(this.cbApplyAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        #endregion

        private Telerik.WinControls.UI.RadPanel panel;
        private Telerik.WinControls.UI.RadPageView pgvSettings;
        private Telerik.WinControls.UI.RadPageViewPage tabPrice;

        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadLabel lblLineThickness;
        private Telerik.WinControls.UI.RadLabel lblColor;
        private Telerik.WinControls.UI.RadColorBox boxColor;
        private Telerik.WinControls.UI.RadSpinEditor rseLineThickness;
        private Telerik.WinControls.UI.RadLabel lblFibonacci;
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
        private Telerik.WinControls.UI.RadCheckBox cbApplyAll;

    }
}
