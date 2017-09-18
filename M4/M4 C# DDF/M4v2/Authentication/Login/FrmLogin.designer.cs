using Telerik.WinControls.UI;

namespace M4.M4v2.Authentication.Login
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.panorama = new Telerik.WinControls.UI.RadPanorama();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.cbxOffline = new Telerik.WinControls.UI.RadCheckBox();
            this.lblStatus = new Telerik.WinControls.UI.RadLabel();
            this.lblVersion = new Telerik.WinControls.UI.RadLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbxLembrar = new Telerik.WinControls.UI.RadCheckBox();
            this.lblRegister = new Telerik.WinControls.UI.RadLabel();
            this.btnGuest = new Telerik.WinControls.UI.RadButton();
            this.lblForgotPassword = new Telerik.WinControls.UI.RadLabel();
            this.btnOk = new Telerik.WinControls.UI.RadButton();
            this.lblPassword = new Telerik.WinControls.UI.RadLabel();
            this.lblLogin = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.panorama)).BeginInit();
            this.panorama.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxOffline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLembrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGuest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblForgotPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // panorama
            // 
            this.panorama.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(153)))));
            this.panorama.Controls.Add(this.txtPassword);
            this.panorama.Controls.Add(this.txtLogin);
            this.panorama.Controls.Add(this.cbxOffline);
            this.panorama.Controls.Add(this.lblStatus);
            this.panorama.Controls.Add(this.lblVersion);
            this.panorama.Controls.Add(this.pictureBox1);
            this.panorama.Controls.Add(this.cbxLembrar);
            this.panorama.Controls.Add(this.lblRegister);
            this.panorama.Controls.Add(this.btnGuest);
            this.panorama.Controls.Add(this.lblForgotPassword);
            this.panorama.Controls.Add(this.btnOk);
            this.panorama.Controls.Add(this.lblPassword);
            this.panorama.Controls.Add(this.lblLogin);
            this.panorama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panorama.Location = new System.Drawing.Point(0, 0);
            this.panorama.Name = "panorama";
            this.panorama.PanelImageSize = new System.Drawing.Size(1024, 768);
            this.panorama.RowsCount = 2;
            this.panorama.ShowGroups = true;
            this.panorama.Size = new System.Drawing.Size(390, 280);
            this.panorama.TabIndex = 0;
            this.panorama.Text = "radTilePanel1";
            ((Telerik.WinControls.UI.RadPanoramaElement)(this.panorama.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(153)))));
            ((Telerik.WinControls.UI.RadScrollBarElement)(this.panorama.GetChildAt(0).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.panorama.GetChildAt(0).GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(10)))), ((int)(((byte)(21)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.panorama.GetChildAt(0).GetChildAt(0).GetChildAt(5))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(153)))));
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(91, 130);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(227, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(91, 103);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(227, 20);
            this.txtLogin.TabIndex = 8;
            this.txtLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLogin_KeyPress);
            // 
            // cbxOffline
            // 
            this.cbxOffline.BackColor = System.Drawing.Color.Transparent;
            this.cbxOffline.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxOffline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.cbxOffline.Location = new System.Drawing.Point(91, 186);
            this.cbxOffline.Name = "cbxOffline";
            this.cbxOffline.Size = new System.Drawing.Size(61, 15);
            this.cbxOffline.TabIndex = 3;
            this.cbxOffline.Text = "Lembrar";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(221)))));
            this.lblStatus.Location = new System.Drawing.Point(12, 256);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(34, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "status";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(53)))), ((int)(((byte)(73)))));
            this.lblVersion.Location = new System.Drawing.Point(340, 256);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(37, 15);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "versão";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(114, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(166, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // cbxLembrar
            // 
            this.cbxLembrar.BackColor = System.Drawing.Color.Transparent;
            this.cbxLembrar.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxLembrar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.cbxLembrar.Location = new System.Drawing.Point(91, 165);
            this.cbxLembrar.Name = "cbxLembrar";
            this.cbxLembrar.Size = new System.Drawing.Size(61, 15);
            this.cbxLembrar.TabIndex = 2;
            this.cbxLembrar.Text = "Lembrar";
            // 
            // lblRegister
            // 
            this.lblRegister.BackColor = System.Drawing.Color.Transparent;
            this.lblRegister.EnableTheming = false;
            this.lblRegister.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegister.ForeColor = System.Drawing.Color.Blue;
            this.lblRegister.Location = new System.Drawing.Point(269, 186);
            this.lblRegister.Name = "lblRegister";
            this.lblRegister.Size = new System.Drawing.Size(49, 15);
            this.lblRegister.TabIndex = 5;
            this.lblRegister.Text = "Registrar";
            this.lblRegister.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);
            // 
            // btnGuest
            // 
            this.btnGuest.BackColor = System.Drawing.Color.Transparent;
            this.btnGuest.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btnGuest.Location = new System.Drawing.Point(127, 220);
            this.btnGuest.Name = "btnGuest";
            this.btnGuest.Size = new System.Drawing.Size(130, 24);
            this.btnGuest.TabIndex = 6;
            this.btnGuest.Text = "Entrar como convidado";
            this.btnGuest.Click += new System.EventHandler(this.BtnGuestClick);
            // 
            // lblForgotPassword
            // 
            this.lblForgotPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblForgotPassword.EnableTheming = false;
            this.lblForgotPassword.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForgotPassword.ForeColor = System.Drawing.Color.Blue;
            this.lblForgotPassword.Location = new System.Drawing.Point(197, 165);
            this.lblForgotPassword.Name = "lblForgotPassword";
            this.lblForgotPassword.Size = new System.Drawing.Size(121, 15);
            this.lblForgotPassword.TabIndex = 4;
            this.lblForgotPassword.Text = "Login/Senha esquecidos";
            this.lblForgotPassword.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblForgotPassword.Click += new System.EventHandler(this.lblForgotPassword_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(263, 220);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(55, 24);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Entrar";
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lblPassword.Location = new System.Drawing.Point(41, 135);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(35, 15);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Senha";
            // 
            // lblLogin
            // 
            this.lblLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblLogin.Font = new System.Drawing.Font("Trebuchet MS", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lblLogin.Location = new System.Drawing.Point(41, 103);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(32, 15);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Login";
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 280);
            this.Controls.Add(this.panorama);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plena Login";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.FrmLoginLoad);
            ((System.ComponentModel.ISupportInitialize)(this.panorama)).EndInit();
            this.panorama.ResumeLayout(false);
            this.panorama.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxOffline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLembrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGuest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblForgotPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RadPanorama panorama;
        private RadLabel lblPassword;
        private RadLabel lblLogin;
        private RadLabel lblForgotPassword;
        private RadButton btnOk;
        private RadButton btnGuest;
        private RadCheckBox cbxLembrar;
        private RadLabel lblRegister;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RadLabel lblVersion;
        private RadLabel lblStatus;
        private RadCheckBox cbxOffline;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
    }
}