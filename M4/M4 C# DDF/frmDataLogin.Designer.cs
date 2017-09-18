namespace M4
{
  partial class frmDataLogin
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
            this.components = new System.ComponentModel.Container();
            this.Image1 = new System.Windows.Forms.PictureBox();
            this._lblCaption_0 = new System.Windows.Forms.Label();
            this._lblCaption_1 = new System.Windows.Forms.Label();
            this.txtPassword = new Nevron.UI.WinForm.Controls.NTextBox();
            this.txtUsername = new Nevron.UI.WinForm.Controls.NTextBox();
            this.cmdCancel = new Nevron.UI.WinForm.Controls.NButton();
            this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
            this._Line1_1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // Image1
            // 
            this.Image1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Image1.ErrorImage = null;
            this.Image1.Image = global::M4.Properties.Resources.Login;
            this.Image1.InitialImage = null;
            this.Image1.Location = new System.Drawing.Point(0, -2);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(350, 60);
            this.Image1.TabIndex = 26;
            this.Image1.TabStop = false;
            // 
            // _lblCaption_0
            // 
            this._lblCaption_0.BackColor = System.Drawing.Color.Transparent;
            this._lblCaption_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblCaption_0.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblCaption_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblCaption_0.Location = new System.Drawing.Point(63, 83);
            this._lblCaption_0.Name = "_lblCaption_0";
            this._lblCaption_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblCaption_0.Size = new System.Drawing.Size(73, 17);
            this._lblCaption_0.TabIndex = 30;
            this._lblCaption_0.Text = "&Username:";
            this._lblCaption_0.Click += new System.EventHandler(this._lblCaption_0_Click);
            // 
            // _lblCaption_1
            // 
            this._lblCaption_1.BackColor = System.Drawing.Color.Transparent;
            this._lblCaption_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblCaption_1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblCaption_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblCaption_1.Location = new System.Drawing.Point(64, 107);
            this._lblCaption_1.Name = "_lblCaption_1";
            this._lblCaption_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblCaption_1.Size = new System.Drawing.Size(73, 17);
            this._lblCaption_1.TabIndex = 29;
            this._lblCaption_1.Text = "&Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(141, 106);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(159, 18);
            this.txtPassword.TabIndex = 28;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(141, 82);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(159, 18);
            this.txtUsername.TabIndex = 27;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(255, 171);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 33;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(169, 171);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(77, 23);
            this.cmdOK.TabIndex = 32;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // _Line1_1
            // 
            this._Line1_1.BackColor = System.Drawing.SystemColors.Window;
            this._Line1_1.Location = new System.Drawing.Point(-23, 151);
            this._Line1_1.Name = "_Line1_1";
            this._Line1_1.Size = new System.Drawing.Size(392, 1);
            this._Line1_1.TabIndex = 31;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmDataLogin
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(346, 206);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this._Line1_1);
            this.Controls.Add(this._lblCaption_0);
            this.Controls.Add(this._lblCaption_1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.Image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDataLogin_FormClosing);
            this.Load += new System.EventHandler(this.frmDataLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.PictureBox Image1;
    public System.Windows.Forms.Label _lblCaption_0;
    public System.Windows.Forms.Label _lblCaption_1;
    internal Nevron.UI.WinForm.Controls.NTextBox txtPassword;
    internal Nevron.UI.WinForm.Controls.NTextBox txtUsername;
    internal Nevron.UI.WinForm.Controls.NButton cmdCancel;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    public System.Windows.Forms.Label _Line1_1;
    private System.Windows.Forms.Timer timer1;
  }
}