using Nevron.UI.WinForm.Controls;

namespace M4
{
  partial class frmNN : NForm
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
      this.nLineControl1 = new Nevron.UI.WinForm.Controls.NLineControl();
      this.cmdRun = new Nevron.UI.WinForm.Controls.NButton();
      this.lstAvailable = new Nevron.UI.WinForm.Controls.NListBox();
      this.lstSelected = new Nevron.UI.WinForm.Controls.NListBox();
      this.cmdAdd = new Nevron.UI.WinForm.Controls.NButton();
      this.cmdRemove = new Nevron.UI.WinForm.Controls.NButton();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // cmdCancel
      // 
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(455, 220);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(75, 23);
      this.cmdCancel.TabIndex = 5;
      this.cmdCancel.Text = "&Cancel";
      this.cmdCancel.UseVisualStyleBackColor = false;
      this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
      // 
      // nLineControl1
      // 
      this.nLineControl1.Location = new System.Drawing.Point(0, 200);
      this.nLineControl1.Name = "nLineControl1";
      this.nLineControl1.Size = new System.Drawing.Size(747, 2);
      this.nLineControl1.Text = "nLineControl1";
      // 
      // cmdRun
      // 
      this.cmdRun.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdRun.Location = new System.Drawing.Point(367, 220);
      this.cmdRun.Name = "cmdRun";
      this.cmdRun.Size = new System.Drawing.Size(75, 23);
      this.cmdRun.TabIndex = 4;
      this.cmdRun.Text = "&Run";
      this.cmdRun.UseVisualStyleBackColor = false;
      this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
      // 
      // lstAvailable
      // 
      this.lstAvailable.ColumnOnLeft = false;
      this.lstAvailable.Location = new System.Drawing.Point(12, 37);
      this.lstAvailable.Name = "lstAvailable";
      this.lstAvailable.Size = new System.Drawing.Size(218, 144);
      this.lstAvailable.TabIndex = 0;
      // 
      // lstSelected
      // 
      this.lstSelected.ColumnOnLeft = false;
      this.lstSelected.Location = new System.Drawing.Point(317, 37);
      this.lstSelected.Name = "lstSelected";
      this.lstSelected.Size = new System.Drawing.Size(218, 144);
      this.lstSelected.TabIndex = 3;
      // 
      // cmdAdd
      // 
      this.cmdAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdAdd.Location = new System.Drawing.Point(236, 81);
      this.cmdAdd.Name = "cmdAdd";
      this.cmdAdd.Size = new System.Drawing.Size(75, 23);
      this.cmdAdd.TabIndex = 1;
      this.cmdAdd.Text = "Add >";
      this.cmdAdd.UseVisualStyleBackColor = false;
      this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
      // 
      // cmdRemove
      // 
      this.cmdRemove.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdRemove.Enabled = false;
      this.cmdRemove.Location = new System.Drawing.Point(236, 110);
      this.cmdRemove.Name = "cmdRemove";
      this.cmdRemove.Size = new System.Drawing.Size(75, 23);
      this.cmdRemove.TabIndex = 2;
      this.cmdRemove.Text = "< Remove";
      this.cmdRemove.UseVisualStyleBackColor = false;
      this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Location = new System.Drawing.Point(12, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(159, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Available Neural Network Inputs";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.Location = new System.Drawing.Point(314, 18);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(158, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Selected Neural Network Inputs";
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::M4.Properties.Resources.NNSmall;
      this.pictureBox1.Location = new System.Drawing.Point(21, 206);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(105, 78);
      this.pictureBox1.TabIndex = 9;
      this.pictureBox1.TabStop = false;
      // 
      // frmNN
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(547, 268);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cmdRemove);
      this.Controls.Add(this.cmdAdd);
      this.Controls.Add(this.lstSelected);
      this.Controls.Add(this.cmdRun);
      this.Controls.Add(this.nLineControl1);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this.lstAvailable);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "frmNN";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Neural Network System";
      this.Load += new System.EventHandler(this.frmNN_Load);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private NButton cmdCancel;
    private NLineControl nLineControl1;
    private NButton cmdRun;
    private NListBox lstAvailable;
    private NListBox lstSelected;
    private NButton cmdAdd;
    private NButton cmdRemove;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox pictureBox1;
  }
}