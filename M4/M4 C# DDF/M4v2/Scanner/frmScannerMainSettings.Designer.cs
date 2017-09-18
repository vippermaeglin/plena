namespace M4
{
  partial class frmScannerMainSettings
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
        this.Label7 = new System.Windows.Forms.Label();
        this.cboPeriodicity = new System.Windows.Forms.ComboBox();
        this.lblSymbolFile = new System.Windows.Forms.Label();
        this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.Cancel_button = new System.Windows.Forms.Button();
        this.OK_Button = new System.Windows.Forms.Button();
        this.cmdBrowse = new System.Windows.Forms.Button();
        this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        this.NGrouper1 = new System.Windows.Forms.GroupBox();
        this.txtScript = new System.Windows.Forms.TextBox();
        this.Label6 = new System.Windows.Forms.Label();
        this.grpData = new System.Windows.Forms.GroupBox();
        this.txtInterval = new System.Windows.Forms.TextBox();
        this.Label8 = new System.Windows.Forms.Label();
        this.txtBars = new System.Windows.Forms.TextBox();
        this.Label9 = new System.Windows.Forms.Label();
        this.cmdDocumentation = new System.Windows.Forms.Button();
        this.TableLayoutPanel1.SuspendLayout();
        this.NGrouper1.SuspendLayout();
        this.grpData.SuspendLayout();
        this.SuspendLayout();
        // 
        // Label7
        // 
        this.Label7.AutoSize = true;
        this.Label7.BackColor = System.Drawing.Color.Transparent;
        this.Label7.Location = new System.Drawing.Point(185, 43);
        this.Label7.Name = "Label7";
        this.Label7.Size = new System.Drawing.Size(55, 13);
        this.Label7.TabIndex = 158;
        this.Label7.Text = "Periodicity";
        // 
        // cboPeriodicity
        // 
        this.cboPeriodicity.Items.AddRange(new object[] {"Minute", "Hour", "Day", "Week"});
        this.cboPeriodicity.Location = new System.Drawing.Point(247, 39);
        this.cboPeriodicity.Name = "cboPeriodicity";
        this.cboPeriodicity.Size = new System.Drawing.Size(79, 20);
        this.cboPeriodicity.TabIndex = 4;
        // 
        // lblSymbolFile
        // 
        this.lblSymbolFile.AutoSize = true;
        this.lblSymbolFile.BackColor = System.Drawing.Color.Transparent;
        this.lblSymbolFile.Location = new System.Drawing.Point(373, 46);
        this.lblSymbolFile.Name = "lblSymbolFile";
        this.lblSymbolFile.Size = new System.Drawing.Size(0, 13);
        this.lblSymbolFile.TabIndex = 156;
        // 
        // TableLayoutPanel1
        // 
        this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.TableLayoutPanel1.ColumnCount = 2;
        this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.TableLayoutPanel1.Controls.Add(this.Cancel_button, 1, 0);
        this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
        this.TableLayoutPanel1.Location = new System.Drawing.Point(420, 352);
        this.TableLayoutPanel1.Name = "TableLayoutPanel1";
        this.TableLayoutPanel1.RowCount = 1;
        this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.TableLayoutPanel1.Size = new System.Drawing.Size(146, 29);
        this.TableLayoutPanel1.TabIndex = 154;
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
        // cmdBrowse
        // 
        this.cmdBrowse.Location = new System.Drawing.Point(247, 73);
        this.cmdBrowse.Name = "cmdBrowse";
        this.cmdBrowse.Size = new System.Drawing.Size(79, 20);
        this.cmdBrowse.TabIndex = 5;
        this.cmdBrowse.Text = "&Browse...";
        this.cmdBrowse.UseVisualStyleBackColor = false;
        this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
        // 
        // OpenFileDialog1
        // 
        this.OpenFileDialog1.FileName = "OpenFileDialog1";
        this.OpenFileDialog1.Filter = "CSV Files|*.csv|Text Files|*.txt";
        // 
        // NGrouper1
        // 
        this.NGrouper1.Controls.Add(this.txtScript);
        this.NGrouper1.Location = new System.Drawing.Point(10, 14);
        this.NGrouper1.Name = "NGrouper1";
        this.NGrouper1.Size = new System.Drawing.Size(560, 137);
        this.NGrouper1.TabIndex = 157;
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
        this.txtScript.TextChanged += new System.EventHandler(this.txtScript_TextChanged);
        // 
        // Label6
        // 
        this.Label6.AutoSize = true;
        this.Label6.BackColor = System.Drawing.Color.Transparent;
        this.Label6.Location = new System.Drawing.Point(16, 43);
        this.Label6.Name = "Label6";
        this.Label6.Size = new System.Drawing.Size(61, 13);
        this.Label6.TabIndex = 109;
        this.Label6.Text = "Bar Interval";
        // 
        // grpData
        // 
        this.grpData.Controls.Add(this.Label7);
        this.grpData.Controls.Add(this.cboPeriodicity);
        this.grpData.Controls.Add(this.lblSymbolFile);
        this.grpData.Controls.Add(this.cmdBrowse);
        this.grpData.Controls.Add(this.Label6);
        this.grpData.Controls.Add(this.txtInterval);
        this.grpData.Controls.Add(this.Label8);
        this.grpData.Controls.Add(this.txtBars);
        this.grpData.Controls.Add(this.Label9);
        this.grpData.Location = new System.Drawing.Point(10, 197);
        this.grpData.Name = "grpData";
        this.grpData.Size = new System.Drawing.Size(559, 119);
        this.grpData.TabIndex = 156;
        this.grpData.Text = "Data Source";
        // 
        // txtInterval
        // 
        this.txtInterval.Location = new System.Drawing.Point(84, 41);
        this.txtInterval.Name = "txtInterval";
        this.txtInterval.Size = new System.Drawing.Size(76, 18);
        this.txtInterval.TabIndex = 2;
        // 
        // Label8
        // 
        this.Label8.AutoSize = true;
        this.Label8.BackColor = System.Drawing.Color.Transparent;
        this.Label8.Location = new System.Drawing.Point(19, 76);
        this.Label8.Name = "Label8";
        this.Label8.Size = new System.Drawing.Size(58, 13);
        this.Label8.TabIndex = 107;
        this.Label8.Text = "Bar History";
        // 
        // txtBars
        // 
        this.txtBars.Location = new System.Drawing.Point(84, 73);
        this.txtBars.Name = "txtBars";
        this.txtBars.Size = new System.Drawing.Size(76, 18);
        this.txtBars.TabIndex = 3;
        // 
        // Label9
        // 
        this.Label9.AutoSize = true;
        this.Label9.BackColor = System.Drawing.Color.Transparent;
        this.Label9.Location = new System.Drawing.Point(193, 75);
        this.Label9.Name = "Label9";
        this.Label9.Size = new System.Drawing.Size(46, 13);
        this.Label9.TabIndex = 106;
        this.Label9.Text = "Symbols";
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
        // frmScannerMainSettings
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(580, 394);
        this.Controls.Add(this.TableLayoutPanel1);
        this.Controls.Add(this.NGrouper1);
        this.Controls.Add(this.grpData);
        this.Controls.Add(this.cmdDocumentation);
        this.Name = "frmScannerMainSettings";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Scanner Settings";
        this.Load += new System.EventHandler(this.frmScannerMainSettings_Load);
        this.TableLayoutPanel1.ResumeLayout(false);
        this.NGrouper1.ResumeLayout(false);
        this.grpData.ResumeLayout(false);
        this.grpData.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Label Label7;
    internal System.Windows.Forms.ComboBox cboPeriodicity;
    internal System.Windows.Forms.Label lblSymbolFile;
    internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
    internal System.Windows.Forms.Button Cancel_button;
    internal System.Windows.Forms.Button OK_Button;
    internal System.Windows.Forms.Button cmdBrowse;
    internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
    internal System.Windows.Forms.GroupBox NGrouper1;
    internal System.Windows.Forms.TextBox txtScript;
    internal System.Windows.Forms.Label Label6;
    internal System.Windows.Forms.GroupBox grpData;
    internal System.Windows.Forms.TextBox txtInterval;
    internal System.Windows.Forms.Label Label8;
    internal System.Windows.Forms.TextBox txtBars;
    internal System.Windows.Forms.Label Label9;
    internal System.Windows.Forms.Button cmdDocumentation;

  }
}