namespace M4.M4v2.BackTest
{
    partial class ctlBackTest
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
            this.grpTrades = new System.Windows.Forms.GroupBox();
            this.m_ListTrades = new System.Windows.Forms.ListView();
            this.Trades = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblInterval = new System.Windows.Forms.Label();
            this.lblBars = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.lblPeriodicity = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.txtBars = new System.Windows.Forms.TextBox();
            this.cboPeriodicity = new System.Windows.Forms.ComboBox();
            this.lblScript = new System.Windows.Forms.Label();
            this.cboScript = new System.Windows.Forms.ComboBox();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.cmdBacktest = new System.Windows.Forms.Button();
            this.cmdDocumentation = new System.Windows.Forms.Button();
            this.grpTrades.SuspendLayout();
            this.grpData.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTrades
            // 
            this.grpTrades.Controls.Add(this.m_ListTrades);
            this.grpTrades.Location = new System.Drawing.Point(3, 158);
            this.grpTrades.Name = "grpTrades";
            this.grpTrades.Size = new System.Drawing.Size(381, 596);
            this.grpTrades.TabIndex = 1;
            this.grpTrades.TabStop = false;
            this.grpTrades.Text = "Resultados";
            // 
            // m_ListTrades
            // 
            this.m_ListTrades.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.m_ListTrades.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_ListTrades.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Trades});
            this.m_ListTrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ListTrades.ForeColor = System.Drawing.SystemColors.WindowText;
            this.m_ListTrades.FullRowSelect = true;
            this.m_ListTrades.GridLines = true;
            this.m_ListTrades.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.m_ListTrades.Location = new System.Drawing.Point(3, 16);
            this.m_ListTrades.MultiSelect = false;
            this.m_ListTrades.Name = "m_ListTrades";
            this.m_ListTrades.Size = new System.Drawing.Size(375, 577);
            this.m_ListTrades.TabIndex = 8;
            this.m_ListTrades.UseCompatibleStateImageBehavior = false;
            this.m_ListTrades.View = System.Windows.Forms.View.Details;
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(204, 59);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(48, 13);
            this.lblInterval.TabIndex = 0;
            this.lblInterval.Text = "Intervalo";
            // 
            // lblBars
            // 
            this.lblBars.AutoSize = true;
            this.lblBars.Location = new System.Drawing.Point(190, 85);
            this.lblBars.Name = "lblBars";
            this.lblBars.Size = new System.Drawing.Size(62, 13);
            this.lblBars.TabIndex = 1;
            this.lblBars.Text = "Num Barras";
            // 
            // lblSymbol
            // 
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new System.Drawing.Point(47, 59);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(31, 13);
            this.lblSymbol.TabIndex = 2;
            this.lblSymbol.Text = "Ativo";
            // 
            // lblPeriodicity
            // 
            this.lblPeriodicity.AutoSize = true;
            this.lblPeriodicity.Location = new System.Drawing.Point(6, 85);
            this.lblPeriodicity.Name = "lblPeriodicity";
            this.lblPeriodicity.Size = new System.Drawing.Size(71, 13);
            this.lblPeriodicity.TabIndex = 3;
            this.lblPeriodicity.Text = "Periodicidade";
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(84, 56);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(100, 20);
            this.txtSymbol.TabIndex = 4;
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(258, 56);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(100, 20);
            this.txtInterval.TabIndex = 5;
            // 
            // txtBars
            // 
            this.txtBars.Location = new System.Drawing.Point(258, 82);
            this.txtBars.Name = "txtBars";
            this.txtBars.Size = new System.Drawing.Size(100, 20);
            this.txtBars.TabIndex = 6;
            // 
            // cboPeriodicity
            // 
            this.cboPeriodicity.FormattingEnabled = true;
            this.cboPeriodicity.Items.AddRange(new object[] {
            "Minute",
            "Hour",
            "Day",
            "Week"});
            this.cboPeriodicity.Location = new System.Drawing.Point(83, 82);
            this.cboPeriodicity.Name = "cboPeriodicity";
            this.cboPeriodicity.Size = new System.Drawing.Size(100, 21);
            this.cboPeriodicity.TabIndex = 7;
            // 
            // lblScript
            // 
            this.lblScript.AutoSize = true;
            this.lblScript.Location = new System.Drawing.Point(23, 32);
            this.lblScript.Name = "lblScript";
            this.lblScript.Size = new System.Drawing.Size(54, 13);
            this.lblScript.TabIndex = 8;
            this.lblScript.Text = "Estratégia";
            // 
            // cboScript
            // 
            this.cboScript.FormattingEnabled = true;
            this.cboScript.Location = new System.Drawing.Point(84, 29);
            this.cboScript.Name = "cboScript";
            this.cboScript.Size = new System.Drawing.Size(100, 21);
            this.cboScript.TabIndex = 9;
            this.cboScript.SelectedIndexChanged += new System.EventHandler(this.cboScript_SelectedIndexChanged);
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.cboScript);
            this.grpData.Controls.Add(this.lblScript);
            this.grpData.Controls.Add(this.cboPeriodicity);
            this.grpData.Controls.Add(this.txtBars);
            this.grpData.Controls.Add(this.txtInterval);
            this.grpData.Controls.Add(this.txtSymbol);
            this.grpData.Controls.Add(this.lblPeriodicity);
            this.grpData.Controls.Add(this.lblSymbol);
            this.grpData.Controls.Add(this.lblBars);
            this.grpData.Controls.Add(this.lblInterval);
            this.grpData.Location = new System.Drawing.Point(3, 3);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(381, 120);
            this.grpData.TabIndex = 0;
            this.grpData.TabStop = false;
            this.grpData.Text = "Fonte";
            // 
            // cmdBacktest
            // 
            this.cmdBacktest.Location = new System.Drawing.Point(125, 129);
            this.cmdBacktest.Name = "cmdBacktest";
            this.cmdBacktest.Size = new System.Drawing.Size(76, 28);
            this.cmdBacktest.TabIndex = 2;
            this.cmdBacktest.Text = "Back Test";
            this.cmdBacktest.UseVisualStyleBackColor = true;
            this.cmdBacktest.Click += new System.EventHandler(this.cmdBacktest_Click);
            // 
            // cmdDocumentation
            // 
            this.cmdDocumentation.Location = new System.Drawing.Point(207, 129);
            this.cmdDocumentation.Name = "cmdDocumentation";
            this.cmdDocumentation.Size = new System.Drawing.Size(76, 28);
            this.cmdDocumentation.TabIndex = 3;
            this.cmdDocumentation.Text = "Referência";
            this.cmdDocumentation.UseVisualStyleBackColor = true;
            this.cmdDocumentation.Click += new System.EventHandler(this.cmdDocumentation_Click);
            // 
            // ctlBackTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdDocumentation);
            this.Controls.Add(this.cmdBacktest);
            this.Controls.Add(this.grpTrades);
            this.Controls.Add(this.grpData);
            this.Name = "ctlBackTest";
            this.Size = new System.Drawing.Size(997, 757);
            this.grpTrades.ResumeLayout(false);
            this.grpData.ResumeLayout(false);
            this.grpData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTrades;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblBars;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblPeriodicity;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.TextBox txtBars;
        private System.Windows.Forms.ComboBox cboPeriodicity;
        private System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.ComboBox cboScript;
        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.ListView m_ListTrades;
        private System.Windows.Forms.ColumnHeader Trades;
        private System.Windows.Forms.Button cmdBacktest;
        private System.Windows.Forms.Button cmdDocumentation;

    }
}
