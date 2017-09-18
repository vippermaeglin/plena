namespace M4
{
  partial class frmActivate
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
            this.txtActivate = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdActivate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtActivate
            // 
            this.txtActivate.Location = new System.Drawing.Point(16, 41);
            this.txtActivate.Name = "txtActivate";
            this.txtActivate.Size = new System.Drawing.Size(326, 18);
            this.txtActivate.TabIndex = 10;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(13, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(168, 13);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "Please enter your activation code:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(267, 80);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            // 
            // cmdActivate
            // 
            this.cmdActivate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdActivate.Location = new System.Drawing.Point(181, 80);
            this.cmdActivate.Name = "cmdActivate";
            this.cmdActivate.Size = new System.Drawing.Size(77, 23);
            this.cmdActivate.TabIndex = 7;
            this.cmdActivate.Text = "&Activate";
            this.cmdActivate.UseVisualStyleBackColor = false;
            this.cmdActivate.Click += new System.EventHandler(this.cmdActivate_Click);
            // 
            // frmActivate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 119);
            this.Controls.Add(this.txtActivate);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdActivate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmActivate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmActivate_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.TextBox txtActivate;
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.Button cmdCancel;
    internal System.Windows.Forms.Button cmdActivate;
  }
}