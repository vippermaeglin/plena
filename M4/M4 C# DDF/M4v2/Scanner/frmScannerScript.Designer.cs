
namespace M4.M4v2.Scanner
{
  partial class frmScannerScript
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
      this.NGrouper1 = new System.Windows.Forms.GroupBox();
      this.txtScript = new System.Windows.Forms.TextBox();
      this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.cmdDocumentation = new System.Windows.Forms.Button();
      this.Cancel_button = new System.Windows.Forms.Button();
      this.OK_Button = new System.Windows.Forms.Button();
      this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.NGrouper1.SuspendLayout();
      this.TableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // NGrouper1
      // 
      this.NGrouper1.Controls.Add(this.txtScript);
      this.NGrouper1.Location = new System.Drawing.Point(10, 14);
      this.NGrouper1.Name = "NGrouper1";
      this.NGrouper1.Size = new System.Drawing.Size(560, 137);
      this.NGrouper1.TabIndex = 156;
      this.NGrouper1.Text = "Script";
      // 
      // txtScript
      // 
      this.txtScript.Location = new System.Drawing.Point(4, 25);
      this.txtScript.Multiline = true;
      this.txtScript.Name = "txtScript";
      this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtScript.Size = new System.Drawing.Size(546, 103);
      this.txtScript.TabIndex = 0;
      // 
      // OpenFileDialog1
      // 
      this.OpenFileDialog1.FileName = "OpenFileDialog1";
      this.OpenFileDialog1.Filter = "CSV Files|*.csv|Text Files|*.txt";
      // 
      // cmdDocumentation
      // 
      this.cmdDocumentation.Location = new System.Drawing.Point(15, 154);
      this.cmdDocumentation.Name = "cmdDocumentation";
      this.cmdDocumentation.Size = new System.Drawing.Size(97, 23);
      this.cmdDocumentation.TabIndex = 155;
      this.cmdDocumentation.Text = "&Script Guide";
      this.cmdDocumentation.UseVisualStyleBackColor = false;
      this.cmdDocumentation.Click += new System.EventHandler(this.cmdDocumentation_Click);
      // 
      // Cancel_button
      // 
      this.Cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Cancel_button.Location = new System.Drawing.Point(76, 3);
      this.Cancel_button.Name = "Cancel_button";
      this.Cancel_button.Size = new System.Drawing.Size(67, 23);
      this.Cancel_button.TabIndex = 7;
      this.Cancel_button.Text = "&Cancel";
      this.Cancel_button.UseVisualStyleBackColor = false;
      this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
      // 
      // OK_Button
      // 
      this.OK_Button.Location = new System.Drawing.Point(3, 3);
      this.OK_Button.Name = "OK_Button";
      this.OK_Button.Size = new System.Drawing.Size(67, 23);
      this.OK_Button.TabIndex = 6;
      this.OK_Button.Text = "&OK";
      this.OK_Button.UseVisualStyleBackColor = false;
      this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
      // 
      // TableLayoutPanel1
      // 
      this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.TableLayoutPanel1.ColumnCount = 2;
      this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.TableLayoutPanel1.Controls.Add(this.Cancel_button, 1, 0);
      this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
      this.TableLayoutPanel1.Location = new System.Drawing.Point(420, 186);
      this.TableLayoutPanel1.Name = "TableLayoutPanel1";
      this.TableLayoutPanel1.RowCount = 1;
      this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.TableLayoutPanel1.Size = new System.Drawing.Size(146, 29);
      this.TableLayoutPanel1.TabIndex = 154;
      // 
      // frmScannerScript
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(580, 228);
      this.Controls.Add(this.NGrouper1);
      this.Controls.Add(this.cmdDocumentation);
      this.Controls.Add(this.TableLayoutPanel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = "frmScannerScript";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Scanner Script";
      this.Load += new System.EventHandler(this.frmScannerScript_Load);
      this.NGrouper1.ResumeLayout(false);
      this.TableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.GroupBox NGrouper1;
    internal System.Windows.Forms.TextBox txtScript;
    internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
    internal System.Windows.Forms.Button cmdDocumentation;
    internal System.Windows.Forms.Button Cancel_button;
    internal System.Windows.Forms.Button OK_Button;
    internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
  }
}