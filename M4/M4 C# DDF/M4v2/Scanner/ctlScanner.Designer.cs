namespace M4.M4v2.Scanner
{
  partial class ctlScanner
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlScanner));
      this.cmdDelete = new System.Windows.Forms.Button();
      this.pnlProgress = new System.Windows.Forms.Panel();
      this.lblInfo = new System.Windows.Forms.Label();
      this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
      this.lblSymbol = new System.Windows.Forms.Label();
      this.cboScanners = new System.Windows.Forms.ComboBox();
      this.lblAlert = new System.Windows.Forms.Label();
      this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
      this.cmdEditScript = new System.Windows.Forms.Button();
      this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
      this.cmdSave = new System.Windows.Forms.Button();
      this.grpSaveLoadScanner = new System.Windows.Forms.GroupBox();
      this.txtScannerName = new System.Windows.Forms.TextBox();
      this.Label1 = new System.Windows.Forms.Label();
      this.cmdScanner = new System.Windows.Forms.Button();
      this.grdResults = new M4DataGridView();
      this.pnlProgress.SuspendLayout();
      this.grpSaveLoadScanner.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
      this.SuspendLayout();
      // 
      // cmdDelete
      // 
      this.cmdDelete.Enabled = false;
      this.cmdDelete.Location = new System.Drawing.Point(272, 72);
      this.cmdDelete.Name = "cmdDelete";
      this.cmdDelete.Size = new System.Drawing.Size(61, 23);
      this.cmdDelete.TabIndex = 3;
      this.cmdDelete.Text = "&Delete";
      this.cmdDelete.UseVisualStyleBackColor = false;
      this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
      // 
      // pnlProgress
      // 
      this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlProgress.Controls.Add(this.lblInfo);
      this.pnlProgress.Controls.Add(this.ProgressBar1);
      this.pnlProgress.Controls.Add(this.lblSymbol);
      this.pnlProgress.Location = new System.Drawing.Point(513, 28);
      this.pnlProgress.Margin = new System.Windows.Forms.Padding(2);
      this.pnlProgress.Name = "pnlProgress";
      this.pnlProgress.Size = new System.Drawing.Size(349, 100);
      this.pnlProgress.TabIndex = 156;
      this.pnlProgress.Visible = false;
      // 
      // lblInfo
      // 
      this.lblInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.lblInfo.AutoSize = true;
      this.lblInfo.Location = new System.Drawing.Point(10, 7);
      this.lblInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new System.Drawing.Size(0, 13);
      this.lblInfo.TabIndex = 3;
      // 
      // ProgressBar1
      // 
      this.ProgressBar1.Location = new System.Drawing.Point(22, 34);
      this.ProgressBar1.Margin = new System.Windows.Forms.Padding(2);
      this.ProgressBar1.Name = "ProgressBar1";
      this.ProgressBar1.Size = new System.Drawing.Size(302, 20);
      this.ProgressBar1.TabIndex = 2;
      // 
      // lblSymbol
      // 
      this.lblSymbol.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.lblSymbol.AutoSize = true;
      this.lblSymbol.Location = new System.Drawing.Point(169, 56);
      this.lblSymbol.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblSymbol.Name = "lblSymbol";
      this.lblSymbol.Size = new System.Drawing.Size(0, 13);
      this.lblSymbol.TabIndex = 1;
      // 
      // cboScanners
      // 
      this.cboScanners.Location = new System.Drawing.Point(94, 72);
      this.cboScanners.Name = "cboScanners";
      this.cboScanners.Size = new System.Drawing.Size(166, 22);
      this.cboScanners.TabIndex = 2;
      this.cboScanners.SelectedIndexChanged += new System.EventHandler(this.cboScanners_SelectedIndexChanged);
      // 
      // lblAlert
      // 
      this.lblAlert.AutoSize = true;
      this.lblAlert.BackColor = System.Drawing.Color.Transparent;
      this.lblAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAlert.ForeColor = System.Drawing.SystemColors.WindowText;
      this.lblAlert.Location = new System.Drawing.Point(11, 76);
      this.lblAlert.Name = "lblAlert";
      this.lblAlert.Size = new System.Drawing.Size(74, 13);
      this.lblAlert.TabIndex = 134;
      this.lblAlert.Text = "Load Scanner";
      // 
      // m_ImgList
      // 
      this.m_ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList.ImageStream")));
      this.m_ImgList.TransparentColor = System.Drawing.Color.Fuchsia;
      this.m_ImgList.Images.SetKeyName(0, "");
      this.m_ImgList.Images.SetKeyName(1, "");
      this.m_ImgList.Images.SetKeyName(2, "");
      this.m_ImgList.Images.SetKeyName(3, "");
      // 
      // cmdEditScript
      // 
      this.cmdEditScript.Enabled = false;
      this.cmdEditScript.Location = new System.Drawing.Point(390, 76);
      this.cmdEditScript.Name = "cmdEditScript";
      this.cmdEditScript.Size = new System.Drawing.Size(97, 23);
      this.cmdEditScript.TabIndex = 157;
      this.cmdEditScript.Text = "&Edit Script";
      this.cmdEditScript.UseVisualStyleBackColor = false;
      this.cmdEditScript.Click += new System.EventHandler(this.cmdEditScript_Click);
      // 
      // tmrUpdate
      // 
      this.tmrUpdate.Interval = 500;
      // 
      // cmdSave
      // 
      this.cmdSave.Location = new System.Drawing.Point(272, 38);
      this.cmdSave.Name = "cmdSave";
      this.cmdSave.Size = new System.Drawing.Size(61, 23);
      this.cmdSave.TabIndex = 1;
      this.cmdSave.Text = "&Save";
      this.cmdSave.UseVisualStyleBackColor = false;
      this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
      // 
      // grpSaveLoadScanner
      // 
      this.grpSaveLoadScanner.Controls.Add(this.cmdDelete);
      this.grpSaveLoadScanner.Controls.Add(this.cboScanners);
      this.grpSaveLoadScanner.Controls.Add(this.lblAlert);
      this.grpSaveLoadScanner.Controls.Add(this.cmdSave);
      this.grpSaveLoadScanner.Controls.Add(this.txtScannerName);
      this.grpSaveLoadScanner.Controls.Add(this.Label1);
      this.grpSaveLoadScanner.Location = new System.Drawing.Point(9, 9);
      this.grpSaveLoadScanner.Name = "grpSaveLoadScanner";
      this.grpSaveLoadScanner.Size = new System.Drawing.Size(358, 119);
      this.grpSaveLoadScanner.TabIndex = 154;
      this.grpSaveLoadScanner.Text = "Scanner Settings";
      // 
      // txtScannerName
      // 
      this.txtScannerName.Location = new System.Drawing.Point(94, 40);
      this.txtScannerName.Name = "txtScannerName";
      this.txtScannerName.Size = new System.Drawing.Size(165, 18);
      this.txtScannerName.TabIndex = 0;
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.BackColor = System.Drawing.Color.Transparent;
      this.Label1.Location = new System.Drawing.Point(11, 42);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(78, 13);
      this.Label1.TabIndex = 129;
      this.Label1.Text = "Scanner Name";
      // 
      // cmdScanner
      // 
      this.cmdScanner.Location = new System.Drawing.Point(390, 47);
      this.cmdScanner.Name = "cmdScanner";
      this.cmdScanner.Size = new System.Drawing.Size(97, 23);
      this.cmdScanner.TabIndex = 153;
      this.cmdScanner.Text = "&Run Scanner";
      this.cmdScanner.UseVisualStyleBackColor = false;
      this.cmdScanner.Click += new System.EventHandler(this.cmdScanner_Click);
      // 
      // grdResults
      // 
      this.grdResults.AllowUserToAddRows = false;
      this.grdResults.AllowUserToDeleteRows = false;
      this.grdResults.AllowUserToResizeColumns = false;
      this.grdResults.AllowUserToResizeRows = false;
      this.grdResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.grdResults.BackgroundColor = System.Drawing.Color.White;
      this.grdResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.grdResults.GridColor = System.Drawing.SystemColors.Control;
      this.grdResults.Location = new System.Drawing.Point(3, 149);
      this.grdResults.MultiSelect = false;
      this.grdResults.Name = "grdResults";
      this.grdResults.ReadOnly = true;
      this.grdResults.RowHeadersVisible = false;
      this.grdResults.RowHeadersWidth = 4;
      this.grdResults.RowTemplate.Height = 24;
      this.grdResults.RowTemplate.ReadOnly = true;
      this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.grdResults.Size = new System.Drawing.Size(870, 450);
      this.grdResults.TabIndex = 155;
      this.grdResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdResults_CellClick);
      // 
      // ctlScanner
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grdResults);
      this.Controls.Add(this.pnlProgress);
      this.Controls.Add(this.cmdEditScript);
      this.Controls.Add(this.grpSaveLoadScanner);
      this.Controls.Add(this.cmdScanner);
      this.Name = "ctlScanner";
      this.Size = new System.Drawing.Size(876, 602);
      this.Load += new System.EventHandler(this.ctlScanner_Load);
      this.pnlProgress.ResumeLayout(false);
      this.pnlProgress.PerformLayout();
      this.grpSaveLoadScanner.ResumeLayout(false);
      this.grpSaveLoadScanner.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    internal M4DataGridView grdResults;
    internal System.Windows.Forms.Button cmdDelete;
    internal System.Windows.Forms.Panel pnlProgress;
    internal System.Windows.Forms.Label lblInfo;
    internal System.Windows.Forms.ProgressBar ProgressBar1;
    internal System.Windows.Forms.Label lblSymbol;
    internal System.Windows.Forms.ComboBox cboScanners;
    internal System.Windows.Forms.Label lblAlert;
    internal System.Windows.Forms.ImageList m_ImgList;
    internal System.Windows.Forms.Button cmdEditScript;
    internal System.Windows.Forms.Timer tmrUpdate;
    internal System.Windows.Forms.Button cmdSave;
    internal System.Windows.Forms.GroupBox grpSaveLoadScanner;
    internal System.Windows.Forms.TextBox txtScannerName;
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.Button cmdScanner;

  }
}