namespace M4
{
  partial class SplashScreen
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
        this.tmrUnload = new System.Windows.Forms.Timer(this.components);
        this.Version = new System.Windows.Forms.Label();
        this.PictureBox1 = new System.Windows.Forms.PictureBox();
        this.lblStatus = new System.Windows.Forms.Label();
        this.Logo = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
        this.SuspendLayout();
        // 
        // tmrUnload
        // 
        this.tmrUnload.Enabled = true;
        this.tmrUnload.Interval = 1000;
        this.tmrUnload.Tick += new System.EventHandler(this.tmrUnload_Tick);
        // 
        // Version
        // 
        this.Version.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.Version.BackColor = System.Drawing.Color.SteelBlue;
        this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Version.ForeColor = System.Drawing.SystemColors.Window;
        this.Version.Location = new System.Drawing.Point(207, 139);
        this.Version.Name = "Version";
        this.Version.Size = new System.Drawing.Size(125, 17);
        this.Version.TabIndex = 6;
        this.Version.Text = "Version {0}.{1:00}";
        // 
        // PictureBox1
        // 
        this.PictureBox1.BackColor = System.Drawing.Color.SteelBlue;
        this.PictureBox1.ErrorImage = null;
        this.PictureBox1.InitialImage = null;
        this.PictureBox1.Location = new System.Drawing.Point(-4, -15);
        this.PictureBox1.Name = "PictureBox1";
        this.PictureBox1.Size = new System.Drawing.Size(352, 228);
        this.PictureBox1.TabIndex = 7;
        this.PictureBox1.TabStop = false;
        // 
        // lblStatus
        // 
        this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.lblStatus.BackColor = System.Drawing.Color.SteelBlue;
        this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStatus.ForeColor = System.Drawing.SystemColors.Window;
        this.lblStatus.Location = new System.Drawing.Point(12, 172);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(320, 17);
        this.lblStatus.TabIndex = 8;
        // 
        // Logo
        // 
        this.Logo.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.Logo.BackColor = System.Drawing.Color.SteelBlue;
        this.Logo.Font = new System.Drawing.Font("Century Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Logo.ForeColor = System.Drawing.SystemColors.Window;
        this.Logo.Location = new System.Drawing.Point(26, 18);
        this.Logo.Name = "Logo";
        this.Logo.Size = new System.Drawing.Size(306, 57);
        this.Logo.TabIndex = 9;
        this.Logo.Text = "PLENA";
        // 
        // SplashScreen
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(344, 198);
        this.Controls.Add(this.Logo);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.Version);
        this.Controls.Add(this.PictureBox1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "SplashScreen";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Load += new System.EventHandler(this.SplashScreen_Load);
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Timer tmrUnload;
    internal System.Windows.Forms.Label Version;
    internal System.Windows.Forms.PictureBox PictureBox1;
    internal System.Windows.Forms.Label lblStatus;
    internal System.Windows.Forms.Label Logo;
  }
}