namespace M4
{
  partial class frmPriceStyle
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
      components = new System.ComponentModel.Container();
      txtInputB = new Nevron.UI.WinForm.Controls.NTextBox();
      cmdOK = new Nevron.UI.WinForm.Controls.NButton();
      txtInputA = new Nevron.UI.WinForm.Controls.NTextBox();
      _Line1_1 = new System.Windows.Forms.Label();
      Picture1 = new System.Windows.Forms.PictureBox();
      txtInputC = new Nevron.UI.WinForm.Controls.NTextBox();
      ToolTip1 = new System.Windows.Forms.ToolTip(components);
      lblInputC = new System.Windows.Forms.Label();
      lblInputB = new System.Windows.Forms.Label();
      lblInputA = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(Picture1)).BeginInit();
      SuspendLayout();
      // 
      // txtInputB
      // 
      txtInputB.Location = new System.Drawing.Point(114, 44);
      txtInputB.Name = "txtInputB";
      txtInputB.Size = new System.Drawing.Size(72, 18);
      txtInputB.TabIndex = 31;
      // 
      // cmdOK
      // 
      cmdOK.Location = new System.Drawing.Point(132, 114);
      cmdOK.Name = "cmdOK";
      cmdOK.Size = new System.Drawing.Size(63, 23);
      cmdOK.TabIndex = 30;
      cmdOK.Text = "&OK";
      cmdOK.UseVisualStyleBackColor = false;
      cmdOK.Click += new System.EventHandler(cmdOK_Click);
      // 
      // txtInputA
      // 
      txtInputA.Location = new System.Drawing.Point(114, 20);
      txtInputA.Name = "txtInputA";
      txtInputA.Size = new System.Drawing.Size(72, 18);
      txtInputA.TabIndex = 29;
      // 
      // _Line1_1
      // 
      _Line1_1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
      _Line1_1.Location = new System.Drawing.Point(-2, 103);
      _Line1_1.Name = "_Line1_1";
      _Line1_1.Size = new System.Drawing.Size(216, 1);
      _Line1_1.TabIndex = 28;
      // 
      // Picture1
      // 
      Picture1.BackColor = System.Drawing.SystemColors.Control;
      Picture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      Picture1.Cursor = System.Windows.Forms.Cursors.Default;
      Picture1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      Picture1.ForeColor = System.Drawing.SystemColors.ControlText;
      Picture1.Location = new System.Drawing.Point(-1, 7);
      Picture1.Name = "Picture1";
      Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No;
      Picture1.Size = new System.Drawing.Size(0, 0);
      Picture1.TabIndex = 27;
      Picture1.TabStop = false;
      // 
      // txtInputC
      // 
      txtInputC.Location = new System.Drawing.Point(114, 68);
      txtInputC.Name = "txtInputC";
      txtInputC.Size = new System.Drawing.Size(72, 18);
      txtInputC.TabIndex = 32;
      // 
      // lblInputC
      // 
      lblInputC.BackColor = System.Drawing.Color.Transparent;
      lblInputC.Cursor = System.Windows.Forms.Cursors.Default;
      lblInputC.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      lblInputC.ForeColor = System.Drawing.Color.Black;
      lblInputC.Location = new System.Drawing.Point(20, 68);
      lblInputC.Name = "lblInputC";
      lblInputC.RightToLeft = System.Windows.Forms.RightToLeft.No;
      lblInputC.Size = new System.Drawing.Size(84, 20);
      lblInputC.TabIndex = 26;
      lblInputC.Text = "###";
      lblInputC.Visible = false;
      // 
      // lblInputB
      // 
      lblInputB.BackColor = System.Drawing.Color.Transparent;
      lblInputB.Cursor = System.Windows.Forms.Cursors.Default;
      lblInputB.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      lblInputB.ForeColor = System.Drawing.Color.Black;
      lblInputB.Location = new System.Drawing.Point(20, 44);
      lblInputB.Name = "lblInputB";
      lblInputB.RightToLeft = System.Windows.Forms.RightToLeft.No;
      lblInputB.Size = new System.Drawing.Size(84, 20);
      lblInputB.TabIndex = 25;
      lblInputB.Text = "###";
      lblInputB.Visible = false;
      // 
      // lblInputA
      // 
      lblInputA.BackColor = System.Drawing.Color.Transparent;
      lblInputA.Cursor = System.Windows.Forms.Cursors.Default;
      lblInputA.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      lblInputA.ForeColor = System.Drawing.Color.Black;
      lblInputA.Location = new System.Drawing.Point(20, 23);
      lblInputA.Name = "lblInputA";
      lblInputA.RightToLeft = System.Windows.Forms.RightToLeft.No;
      lblInputA.Size = new System.Drawing.Size(84, 17);
      lblInputA.TabIndex = 24;
      lblInputA.Text = "###";
      lblInputA.Visible = false;
      // 
      // frmPriceStyle
      // 
      AcceptButton = cmdOK;
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(212, 145);
      Controls.Add(txtInputB);
      Controls.Add(cmdOK);
      Controls.Add(txtInputA);
      Controls.Add(_Line1_1);
      Controls.Add(Picture1);
      Controls.Add(txtInputC);
      Controls.Add(lblInputC);
      Controls.Add(lblInputB);
      Controls.Add(lblInputA);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "frmPriceStyle";
      StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      Text = "Price Style Properties";
      ((System.ComponentModel.ISupportInitialize)(Picture1)).EndInit();
      ResumeLayout(false);

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NTextBox txtInputB;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal Nevron.UI.WinForm.Controls.NTextBox txtInputA;
    public System.Windows.Forms.Label _Line1_1;
    public System.Windows.Forms.PictureBox Picture1;
    internal Nevron.UI.WinForm.Controls.NTextBox txtInputC;
    public System.Windows.Forms.ToolTip ToolTip1;
    public System.Windows.Forms.Label lblInputC;
    public System.Windows.Forms.Label lblInputB;
    public System.Windows.Forms.Label lblInputA;

  }
}