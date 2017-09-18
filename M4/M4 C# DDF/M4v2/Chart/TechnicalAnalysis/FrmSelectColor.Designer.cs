namespace M4.M4v2.Chart.TechnicalAnalysis
{
    partial class FrmSelectColor
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
            this.cslColor = new Telerik.WinControls.UI.RadColorSelector();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cslColor
            // 
            this.cslColor.AddNewColorButtonText = "Add Custom Color";
            this.cslColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cslColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(219)))), ((int)(((byte)(247)))));
            this.cslColor.BasicTabHeading = "Basic";
            this.cslColor.Location = new System.Drawing.Point(1, 1);
            this.cslColor.MinimumSize = new System.Drawing.Size(508, 395);
            this.cslColor.Name = "cslColor";
            this.cslColor.OldColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cslColor.OldColorLabelHeading = "Current";
            this.cslColor.ProfessionalTabHeading = "Professional";
            this.cslColor.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cslColor.SelectedColorLabelHeading = "New";
            this.cslColor.SelectedHslColor = Telerik.WinControls.HslColor.FromAhsl(0D, 1D, 1D);
            this.cslColor.SelectedRgbColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cslColor.ShowSystemColors = false;
            this.cslColor.ShowWebColors = false;
            this.cslColor.Size = new System.Drawing.Size(508, 395);
            this.cslColor.SystemTabHeading = "System";
            this.cslColor.TabIndex = 0;
            this.cslColor.WebTabHeading = "Web";
            this.cslColor.OkButtonClicked += new Telerik.WinControls.ColorChangedEventHandler(this.RadColorSelector1OkButtonClicked);
            this.cslColor.CancelButtonClicked += new Telerik.WinControls.ColorChangedEventHandler(this.RadColorSelector1CancelButtonClicked1);
            // 
            // FrmSelectColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 396);
            this.Controls.Add(this.cslColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectColor";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSelectColor";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadColorSelector cslColor;

    }
}
