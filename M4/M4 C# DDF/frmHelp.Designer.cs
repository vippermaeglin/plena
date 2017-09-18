namespace M4
{
  partial class frmHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp));
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.PrintDocument1 = new System.Drawing.Printing.PrintDocument();
            this.Label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(334, 151);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.BackColor = System.Drawing.Color.White;
            this.LinkLabel1.Location = new System.Drawing.Point(12, 52);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(38, 13);
            this.LinkLabel1.TabIndex = 9;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "http://";
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.White;
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(5, 9);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(421, 132);
            this.Label2.TabIndex = 8;
            this.Label2.Text = resources.GetString("Label2.Text");
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(431, 186);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.LinkLabel1);
            this.Controls.Add(this.Label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHelp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About PLENA";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.ToolTip ToolTip1;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal System.Windows.Forms.LinkLabel LinkLabel1;
    internal System.Drawing.Printing.PrintDocument PrintDocument1;
    internal System.Windows.Forms.Label Label2;
  }
}