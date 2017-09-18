namespace M4.M4v2.Authentication.Login
{
    partial class FrmRecovery
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
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.radSeparator1 = new Telerik.WinControls.UI.RadSeparator();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.mtbCPF = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbEmail = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.lblCPF = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            this.radPanorama1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbCPF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanorama1
            // 
            this.radPanorama1.Controls.Add(this.radSeparator1);
            this.radPanorama1.Controls.Add(this.lblConfirm);
            this.radPanorama1.Controls.Add(this.mtbCPF);
            this.radPanorama1.Controls.Add(this.button1);
            this.radPanorama1.Controls.Add(this.tbEmail);
            this.radPanorama1.Controls.Add(this.lblCPF);
            this.radPanorama1.Controls.Add(this.lblEmail);
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.Size = new System.Drawing.Size(225, 132);
            this.radPanorama1.TabIndex = 8;
            this.radPanorama1.Text = "radPanorama1";
            // 
            // radSeparator1
            // 
            this.radSeparator1.Location = new System.Drawing.Point(0, 84);
            this.radSeparator1.Name = "radSeparator1";
            this.radSeparator1.Size = new System.Drawing.Size(225, 11);
            this.radSeparator1.TabIndex = 8;
            this.radSeparator1.Text = "radSeparator1";
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.BackColor = System.Drawing.Color.Transparent;
            this.lblConfirm.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirm.Location = new System.Drawing.Point(3, 9);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(167, 15);
            this.lblConfirm.TabIndex = 5;
            this.lblConfirm.Text = "Confirme seus dados cadastrados:";
            // 
            // mtbCPF
            // 
            this.mtbCPF.Location = new System.Drawing.Point(40, 32);
            this.mtbCPF.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.mtbCPF.Mask = "00000000000";
            this.mtbCPF.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.mtbCPF.Name = "mtbCPF";
            this.mtbCPF.PromptChar = ' ';
            this.mtbCPF.Size = new System.Drawing.Size(161, 20);
            this.mtbCPF.TabIndex = 6;
            this.mtbCPF.TabStop = false;
            this.mtbCPF.Text = "           ";
            this.mtbCPF.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtbCPF_KeyDown);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(77, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(40, 59);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbEmail.MaskType = Telerik.WinControls.UI.MaskType.EMail;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(161, 20);
            this.tbEmail.TabIndex = 7;
            this.tbEmail.TabStop = false;
            // 
            // lblCPF
            // 
            this.lblCPF.AutoSize = true;
            this.lblCPF.BackColor = System.Drawing.Color.Transparent;
            this.lblCPF.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPF.Location = new System.Drawing.Point(3, 39);
            this.lblCPF.Name = "lblCPF";
            this.lblCPF.Size = new System.Drawing.Size(25, 15);
            this.lblCPF.TabIndex = 1;
            this.lblCPF.Text = "CPF";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(3, 66);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(33, 15);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // FrmRecovery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 132);
            this.Controls.Add(this.radPanorama1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRecovery";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recovery";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            this.radPanorama1.ResumeLayout(false);
            this.radPanorama1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbCPF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblCPF;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblConfirm;
        private Telerik.WinControls.UI.RadMaskedEditBox mtbCPF;
        private Telerik.WinControls.UI.RadMaskedEditBox tbEmail;
        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadSeparator radSeparator1;
    }
}
