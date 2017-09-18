namespace M4
{
  partial class frmFindSymbol
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
      this.dgvSymbols = new System.Windows.Forms.DataGridView();
      this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Company = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cmdOK = new Nevron.UI.WinForm.Controls.NButton();
      this.Label1 = new System.Windows.Forms.Label();
      this.txtSearch = new Nevron.UI.WinForm.Controls.NTextBox();
      ((System.ComponentModel.ISupportInitialize)(this.dgvSymbols)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvSymbols
      // 
      this.dgvSymbols.AllowUserToAddRows = false;
      this.dgvSymbols.AllowUserToDeleteRows = false;
      this.dgvSymbols.AllowUserToOrderColumns = true;
      this.dgvSymbols.AllowUserToResizeRows = false;
      this.dgvSymbols.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
      this.dgvSymbols.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.dgvSymbols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSymbols.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Symbol,
            this.Company});
      this.dgvSymbols.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dgvSymbols.Location = new System.Drawing.Point(12, 81);
      this.dgvSymbols.Name = "dgvSymbols";
      this.dgvSymbols.ReadOnly = true;
      this.dgvSymbols.RowHeadersVisible = false;
      this.dgvSymbols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvSymbols.Size = new System.Drawing.Size(274, 123);
      this.dgvSymbols.TabIndex = 10;
      // 
      // Symbol
      // 
      this.Symbol.HeaderText = "Symbol";
      this.Symbol.Name = "Symbol";
      this.Symbol.ReadOnly = true;
      this.Symbol.Width = 75;
      // 
      // Company
      // 
      this.Company.HeaderText = "Company";
      this.Company.Name = "Company";
      this.Company.ReadOnly = true;
      this.Company.Width = 195;
      // 
      // cmdOK
      // 
      this.cmdOK.Location = new System.Drawing.Point(214, 43);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(63, 23);
      this.cmdOK.TabIndex = 9;
      this.cmdOK.Text = "&Find";
      this.cmdOK.UseVisualStyleBackColor = false;
      this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.Location = new System.Drawing.Point(15, 18);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(82, 13);
      this.Label1.TabIndex = 8;
      this.Label1.Text = "Company Name";
      // 
      // txtSearch
      // 
      this.txtSearch.Location = new System.Drawing.Point(105, 15);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new System.Drawing.Size(173, 18);
      this.txtSearch.TabIndex = 7;
      // 
      // frmFindSymbol
      // 
      this.AcceptButton = this.cmdOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(298, 79);
      this.Controls.Add(this.dgvSymbols);
      this.Controls.Add(this.cmdOK);
      this.Controls.Add(this.Label1);
      this.Controls.Add(this.txtSearch);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmFindSymbol";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Symbol Search";
      ((System.ComponentModel.ISupportInitialize)(this.dgvSymbols)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.DataGridView dgvSymbols;
    internal System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
    internal System.Windows.Forms.DataGridViewTextBoxColumn Company;
    internal Nevron.UI.WinForm.Controls.NButton cmdOK;
    internal System.Windows.Forms.Label Label1;
    internal Nevron.UI.WinForm.Controls.NTextBox txtSearch;
  }
}