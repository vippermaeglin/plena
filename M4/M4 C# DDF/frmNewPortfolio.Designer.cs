namespace M4
{
  partial class frmNewPortfolio
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
      this.cmdCancel = new Nevron.UI.WinForm.Controls.NButton();
      this.Label2 = new System.Windows.Forms.Label();
      this.txtBalance = new Nevron.UI.WinForm.Controls.NTextBox();
      this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
      this.Label1 = new System.Windows.Forms.Label();
      this.txtName = new Nevron.UI.WinForm.Controls.NTextBox();
      this.SuspendLayout();
      // 
      // cmdCancel
      // 
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(201, 84);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(63, 23);
      this.cmdCancel.TabIndex = 11;
      this.cmdCancel.Text = "&Cancel";
      this.cmdCancel.UseVisualStyleBackColor = false;
      this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
      // 
      // Label2
      // 
      this.Label2.AutoSize = true;
      this.Label2.Location = new System.Drawing.Point(16, 47);
      this.Label2.Name = "Label2";
      this.Label2.Size = new System.Drawing.Size(85, 13);
      this.Label2.TabIndex = 13;
      this.Label2.Text = "Starting Balance";
      // 
      // txtBalance
      // 
      this.txtBalance.Location = new System.Drawing.Point(107, 44);
      this.txtBalance.Name = "txtBalance";
      this.txtBalance.ShortcutsEnabled = false;
      this.txtBalance.Size = new System.Drawing.Size(153, 18);
      this.txtBalance.TabIndex = 9;
      this.txtBalance.TextChanged += new System.EventHandler(this.txtBalance_TextChanged);
      this.txtBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBalance_KeyPress);
      // 
      // cmdOK
      // 
      this.cmdOK.Location = new System.Drawing.Point(129, 84);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(63, 23);
      this.cmdOK.TabIndex = 10;
      this.cmdOK.Text = "&OK";
      this.cmdOK.UseVisualStyleBackColor = false;
      this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.Location = new System.Drawing.Point(16, 21);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(76, 13);
      this.Label1.TabIndex = 12;
      this.Label1.Text = "Portfolio Name";
      // 
      // txtName
      // 
      this.txtName.Location = new System.Drawing.Point(107, 18);
      this.txtName.MaxLength = 49;
      this.txtName.Name = "txtName";
      this.txtName.Size = new System.Drawing.Size(153, 18);
      this.txtName.TabIndex = 8;
      // 
      // frmNewPortfolio
      // 
      this.AcceptButton = this.cmdOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cmdCancel;
      this.ClientSize = new System.Drawing.Size(280, 124);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this.Label2);
      this.Controls.Add(this.txtBalance);
      this.Controls.Add(this.cmdOK);
      this.Controls.Add(this.Label1);
      this.Controls.Add(this.txtName);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "frmNewPortfolio";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Portfolio";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewPortfolio_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NButton cmdCancel;
    internal System.Windows.Forms.Label Label2;
    internal Nevron.UI.WinForm.Controls.NTextBox txtBalance;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal System.Windows.Forms.Label Label1;
    internal Nevron.UI.WinForm.Controls.NTextBox txtName;
  }
}