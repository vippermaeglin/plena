namespace M4.M4v2.Workspace
{
    partial class FrmDescription
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
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.txtTextWorkspace = new Telerik.WinControls.UI.RadTextBox();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.cbxDefaultWorkspace = new Telerik.WinControls.UI.RadCheckBox();
            this.lblWorkspace = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTextWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxDefaultWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(91, 93);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(92, 24);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // txtTextWorkspace
            // 
            this.txtTextWorkspace.Location = new System.Drawing.Point(12, 32);
            this.txtTextWorkspace.Name = "txtTextWorkspace";
            this.txtTextWorkspace.Size = new System.Drawing.Size(269, 20);
            this.txtTextWorkspace.TabIndex = 26;
            this.txtTextWorkspace.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(189, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 24);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // cbxDefaultWorkspace
            // 
            this.cbxDefaultWorkspace.Location = new System.Drawing.Point(12, 60);
            this.cbxDefaultWorkspace.Name = "cbxDefaultWorkspace";
            this.cbxDefaultWorkspace.Size = new System.Drawing.Size(56, 18);
            this.cbxDefaultWorkspace.TabIndex = 27;
            this.cbxDefaultWorkspace.Text = "Default";
            this.cbxDefaultWorkspace.Visible = false;
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.Location = new System.Drawing.Point(13, 12);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(121, 18);
            this.lblWorkspace.TabIndex = 28;
            this.lblWorkspace.Text = "Workspace Description";
            // 
            // FrmDescription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 131);
            this.Controls.Add(this.lblWorkspace);
            this.Controls.Add(this.cbxDefaultWorkspace);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTextWorkspace);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDescription";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmDescription";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTextWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxDefaultWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadTextBox txtTextWorkspace;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadCheckBox cbxDefaultWorkspace;
        private Telerik.WinControls.UI.RadLabel lblWorkspace;
    }
}
