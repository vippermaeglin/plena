namespace M4.M4v2.Chart.Templates
{
    partial class FrmTemplates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTemplates));
            this.trvTemplates = new Telerik.WinControls.UI.RadTreeView();
            this.btnDismiss = new Telerik.WinControls.UI.RadButton();
            this.btnNew = new Telerik.WinControls.UI.RadButton();
            this.btnRemove = new Telerik.WinControls.UI.RadButton();
            this.btnApply = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.trvTemplates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // trvTemplates
            // 
            this.trvTemplates.AutoScroll = true;
            this.trvTemplates.Location = new System.Drawing.Point(12, 12);
            this.trvTemplates.Name = "trvTemplates";
            this.trvTemplates.ShowLines = true;
            this.trvTemplates.ShowNodeToolTips = true;
            this.trvTemplates.Size = new System.Drawing.Size(274, 284);
            this.trvTemplates.SpacingBetweenNodes = -1;
            this.trvTemplates.TabIndex = 20;
            this.trvTemplates.Text = "radTreeView1";
            // 
            // btnDismiss
            // 
            this.btnDismiss.Location = new System.Drawing.Point(194, 305);
            this.btnDismiss.Name = "btnDismiss";
            this.btnDismiss.Size = new System.Drawing.Size(92, 24);
            this.btnDismiss.TabIndex = 21;
            this.btnDismiss.Text = "Dismiss";
            this.btnDismiss.Click += new System.EventHandler(this.BtnDismissClick);
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNew.Location = new System.Drawing.Point(12, 305);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(24, 24);
            this.btnNew.TabIndex = 23;
            this.btnNew.Click += new System.EventHandler(this.BtnNewClick);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemove.Location = new System.Drawing.Point(72, 305);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 24;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // btnApply
            // 
            this.btnApply.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnApply.Location = new System.Drawing.Point(42, 305);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(24, 24);
            this.btnApply.TabIndex = 22;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // FrmTemplates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 334);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnDismiss);
            this.Controls.Add(this.trvTemplates);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTemplates";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmTemplates";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.trvTemplates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDismiss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView trvTemplates;
        private Telerik.WinControls.UI.RadButton btnDismiss;
        private Telerik.WinControls.UI.RadButton btnNew;
        private Telerik.WinControls.UI.RadButton btnRemove;
        private Telerik.WinControls.UI.RadButton btnApply;
    }
}
