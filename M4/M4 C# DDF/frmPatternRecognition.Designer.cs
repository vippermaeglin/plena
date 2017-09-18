namespace M4
{
  partial class frmPatternRecognition
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
      cmdCancel = new Nevron.UI.WinForm.Controls.NButton();
      cmdOK = new Nevron.UI.WinForm.Controls.NButton();
      Label1 = new System.Windows.Forms.Label();
      cboPattern = new Nevron.UI.WinForm.Controls.NComboBox();
      SuspendLayout();
      // 
      // cmdCancel
      // 
      cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      cmdCancel.Location = new System.Drawing.Point(316, 64);
      cmdCancel.Name = "cmdCancel";
      cmdCancel.Size = new System.Drawing.Size(63, 23);
      cmdCancel.TabIndex = 66;
      cmdCancel.Text = "&Cancel";
      cmdCancel.UseVisualStyleBackColor = false;
      cmdCancel.Click += new System.EventHandler(cmdCancel_Click);
      // 
      // cmdOK
      // 
      cmdOK.Location = new System.Drawing.Point(241, 64);
      cmdOK.Name = "cmdOK";
      cmdOK.Size = new System.Drawing.Size(63, 23);
      cmdOK.TabIndex = 65;
      cmdOK.Text = "&OK";
      cmdOK.UseVisualStyleBackColor = false;
      cmdOK.Click += new System.EventHandler(cmdOK_Click);
      // 
      // Label1
      // 
      Label1.AutoSize = true;
      Label1.BackColor = System.Drawing.Color.Transparent;
      Label1.Location = new System.Drawing.Point(21, 23);
      Label1.Name = "Label1";
      Label1.Size = new System.Drawing.Size(88, 13);
      Label1.TabIndex = 67;
      Label1.Text = "Pattern Definition\r\n";
      // 
      // cboPattern
      // 
      cboPattern.ListProperties.ColumnOnLeft = false;
      cboPattern.Location = new System.Drawing.Point(119, 19);
      cboPattern.Name = "cboPattern";
      cboPattern.Size = new System.Drawing.Size(260, 22);
      cboPattern.TabIndex = 64;
      cboPattern.SelectedIndexChanged += new System.EventHandler(cboPattern_SelectedIndexChanged);
      // 
      // frmPatternRecognition
      // 
      AcceptButton = cmdOK;
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      CancelButton = cmdCancel;
      ClientSize = new System.Drawing.Size(401, 107);
      Controls.Add(cmdCancel);
      Controls.Add(cmdOK);
      Controls.Add(Label1);
      Controls.Add(cboPattern);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "frmPatternRecognition";
      ShowInTaskbar = false;
      StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      Text = "Advanced Pattern Recognition";
      Load += new System.EventHandler(frmPatternRecognition_Load);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NButton cmdCancel;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal System.Windows.Forms.Label Label1;
    internal Nevron.UI.WinForm.Controls.NComboBox cboPattern;
  }
}