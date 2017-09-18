namespace M4
{
  partial class frmSelectChart
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
      this._Line1_1 = new System.Windows.Forms.Label();
      this.Label4 = new System.Windows.Forms.Label();
      this.txtInterval = new Nevron.UI.WinForm.Controls.NTextBox();
      this.Label3 = new System.Windows.Forms.Label();
      this.cboPeriodicity = new Nevron.UI.WinForm.Controls.NComboBox();
      this.Label2 = new System.Windows.Forms.Label();
      this.txtBars = new Nevron.UI.WinForm.Controls.NTextBox();
      this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
      this.Label1 = new System.Windows.Forms.Label();
      this.txtSymbol = new Nevron.UI.WinForm.Controls.NTextBox();
      this.SuspendLayout();
      // 
      // cmdCancel
      // 
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(149, 159);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(71, 23);
      this.cmdCancel.TabIndex = 28;
      this.cmdCancel.Text = "&Cancel";
      this.cmdCancel.UseVisualStyleBackColor = false;
      this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
      // 
      // _Line1_1
      // 
      this._Line1_1.BackColor = System.Drawing.SystemColors.Window;
      this._Line1_1.Location = new System.Drawing.Point(-25, 144);
      this._Line1_1.Name = "_Line1_1";
      this._Line1_1.Size = new System.Drawing.Size(292, 1);
      this._Line1_1.TabIndex = 32;
      // 
      // Label4
      // 
      this.Label4.AutoSize = true;
      this.Label4.Location = new System.Drawing.Point(34, 84);
      this.Label4.Name = "Label4";
      this.Label4.Size = new System.Drawing.Size(61, 13);
      this.Label4.TabIndex = 31;
      this.Label4.Text = "Bar Interval";
      // 
      // txtInterval
      // 
      this.txtInterval.Location = new System.Drawing.Point(108, 81);
      this.txtInterval.Name = "txtInterval";
      this.txtInterval.Size = new System.Drawing.Size(91, 18);
      this.txtInterval.TabIndex = 24;
      // 
      // Label3
      // 
      this.Label3.AutoSize = true;
      this.Label3.Location = new System.Drawing.Point(34, 55);
      this.Label3.Name = "Label3";
      this.Label3.Size = new System.Drawing.Size(55, 13);
      this.Label3.TabIndex = 30;
      this.Label3.Text = "Periodicity";
      // 
      // cboPeriodicity
      // 
      this.cboPeriodicity.Items.AddRange(new object[] {
            new Nevron.UI.WinForm.Controls.NListBoxItem("Minute", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Hour", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Day", -1, false, 0, new System.Drawing.Size(0, 0)),
            new Nevron.UI.WinForm.Controls.NListBoxItem("Week", -1, false, 0, new System.Drawing.Size(0, 0))});
      this.cboPeriodicity.ListProperties.ColumnOnLeft = false;
      this.cboPeriodicity.Location = new System.Drawing.Point(108, 52);
      this.cboPeriodicity.Name = "cboPeriodicity";
      this.cboPeriodicity.Size = new System.Drawing.Size(91, 20);
      this.cboPeriodicity.TabIndex = 23;
      this.cboPeriodicity.Text = "NComboBox1";
      this.cboPeriodicity.SelectedIndexChanged += new System.EventHandler(this.cboPeriodicity_SelectedIndexChanged);
      // 
      // Label2
      // 
      this.Label2.AutoSize = true;
      this.Label2.Location = new System.Drawing.Point(34, 113);
      this.Label2.Name = "Label2";
      this.Label2.Size = new System.Drawing.Size(58, 13);
      this.Label2.TabIndex = 29;
      this.Label2.Text = "Bar History";
      // 
      // txtBars
      // 
      this.txtBars.Location = new System.Drawing.Point(108, 110);
      this.txtBars.Name = "txtBars";
      this.txtBars.Size = new System.Drawing.Size(91, 18);
      this.txtBars.TabIndex = 25;
      // 
      // cmdOK
      // 
      this.cmdOK.Location = new System.Drawing.Point(70, 159);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(71, 23);
      this.cmdOK.TabIndex = 26;
      this.cmdOK.Text = "&OK";
      this.cmdOK.UseVisualStyleBackColor = false;
      this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.Location = new System.Drawing.Point(34, 26);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(41, 13);
      this.Label1.TabIndex = 27;
      this.Label1.Text = "Symbol";
      // 
      // txtSymbol
      // 
      this.txtSymbol.Location = new System.Drawing.Point(108, 23);
      this.txtSymbol.Name = "txtSymbol";
      this.txtSymbol.Size = new System.Drawing.Size(91, 18);
      this.txtSymbol.TabIndex = 22;
      // 
      // frmSelectChart
      // 
      this.AcceptButton = this.cmdOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cmdCancel;
      this.ClientSize = new System.Drawing.Size(243, 205);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this._Line1_1);
      this.Controls.Add(this.Label4);
      this.Controls.Add(this.txtInterval);
      this.Controls.Add(this.Label3);
      this.Controls.Add(this.cboPeriodicity);
      this.Controls.Add(this.Label2);
      this.Controls.Add(this.txtBars);
      this.Controls.Add(this.cmdOK);
      this.Controls.Add(this.Label1);
      this.Controls.Add(this.txtSymbol);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmSelectChart";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectChart_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal Nevron.UI.WinForm.Controls.NButton cmdCancel;
    public System.Windows.Forms.Label _Line1_1;
    internal System.Windows.Forms.Label Label4;
    internal Nevron.UI.WinForm.Controls.NTextBox txtInterval;
    internal System.Windows.Forms.Label Label3;
    internal Nevron.UI.WinForm.Controls.NComboBox cboPeriodicity;
    internal System.Windows.Forms.Label Label2;
    internal Nevron.UI.WinForm.Controls.NTextBox txtBars;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal System.Windows.Forms.Label Label1;
    internal Nevron.UI.WinForm.Controls.NTextBox txtSymbol;
  }
}