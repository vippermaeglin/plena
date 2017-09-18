using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.LineStudy
{
    partial class FrmSelectStudy
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
            this.pgrdStudy = new Telerik.WinControls.UI.RadPropertyGrid();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            ((System.ComponentModel.ISupportInitialize)(this.pgrdStudy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pgrdStudy
            // 
            this.pgrdStudy.AutoScroll = true;
            this.pgrdStudy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgrdStudy.EnableGrouping = false;
            this.pgrdStudy.EnableKineticScrolling = true;
            this.pgrdStudy.EnableSorting = false;
            this.pgrdStudy.HelpBarHeight = 40F;
            this.pgrdStudy.Location = new System.Drawing.Point(0, 0);
            this.pgrdStudy.Name = "pgrdStudy";
            this.pgrdStudy.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgrdStudy.Size = new System.Drawing.Size(228, 194);
            this.pgrdStudy.TabIndex = 19;
            this.pgrdStudy.Text = "radPropertyGrid1";
            this.pgrdStudy.ItemFormatting += new Telerik.WinControls.UI.PropertyGridItemFormattingEventHandler(this.pgrdStudy_ItemFormatting);
            this.pgrdStudy.PropertyValueChanged += new Telerik.WinControls.UI.PropertyGridItemValueChangedEventHandler(this.pgrdStudy_PropertyValueChanged);
            // 
            // FrmSelectStudy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(228, 194);
            this.Controls.Add(this.pgrdStudy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectStudy";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lines";
            this.ThemeName = "ControlDefault";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSelectStudy_FormClosing);
            this.Shown += new System.EventHandler(this.FrmSelectStudy_Shown);
            this.Leave += new System.EventHandler(this.FrmSelectStudy_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.pgrdStudy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Surface surface1;
        private Telerik.WinControls.UI.RadPropertyGrid pgrdStudy;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
    }
}
