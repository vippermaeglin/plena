namespace M4
{
  partial class frmColor
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
            this.NColorPane1 = new Nevron.UI.WinForm.Controls.NColorPane();
            this.cmdCancel = new Nevron.UI.WinForm.Controls.NButton();
            this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
            this.SuspendLayout();
            // 
            // NColorPane1
            // 
            this.NColorPane1.BackgroundType = Nevron.UI.WinForm.Controls.BackgroundType.SolidColor;
            this.NColorPane1.Location = new System.Drawing.Point(-2, 10);
            this.NColorPane1.Name = "NColorPane1";
            this.NColorPane1.Size = new System.Drawing.Size(281, 260);
            this.NColorPane1.TabIndex = 3;
            this.NColorPane1.TabStop = false;
            this.NColorPane1.Text = "NColorPane1";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(186, 285);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(100, 285);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(77, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 320);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.NColorPane1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmColor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Color";
            this.ResumeLayout(false);

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NColorPane NColorPane1;
    internal Nevron.UI.WinForm.Controls.NButton cmdCancel;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
  }
}