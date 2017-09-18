namespace M4
{
  partial class ctlForex
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.tmrSimulate = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // tmrSimulate
      // 
      this.tmrSimulate.Enabled = true;
      this.tmrSimulate.Interval = 500;
      this.tmrSimulate.Tick += new System.EventHandler(this.tmrSimulate_Tick);
      // 
      // ctlForex
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.Name = "ctlForex";
      this.Size = new System.Drawing.Size(616, 325);
      this.Resize += new System.EventHandler(this.ctlForex_Resize);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Timer tmrSimulate;
  }
}
