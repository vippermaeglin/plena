namespace M4.M4v2.Scripts
{
    partial class ctlScripts
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
            this.grpData = new System.Windows.Forms.GroupBox();
            this.cboScript = new System.Windows.Forms.ComboBox();
            this.lblScript = new System.Windows.Forms.Label();
            this.cboPeriodicity = new System.Windows.Forms.ComboBox();
            this.txtBars = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.lblPeriodicity = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.lblBars = new System.Windows.Forms.Label();
            this.lblInterval = new System.Windows.Forms.Label();
            this.tabScripts = new System.Windows.Forms.TabControl();
            this.tabBuyScript = new System.Windows.Forms.TabPage();
            this.tabSellScript = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btTest = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.grpData.SuspendLayout();
            this.tabScripts.SuspendLayout();
            this.SuspendLayout();
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
            this.grpData.TabIndex = 1;
            this.grpData.TabStop = false;
            this.grpData.Text = "Fonte";
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
            // lblScript
            // 
            this.lblScript.AutoSize = true;
            this.lblScript.Location = new System.Drawing.Point(23, 32);
            this.lblScript.Name = "lblScript";
            this.lblScript.Size = new System.Drawing.Size(54, 13);
            this.lblScript.TabIndex = 8;
            this.lblScript.Text = "Estratégia";
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
            // txtBars
            // 
            this.txtBars.Location = new System.Drawing.Point(258, 82);
            this.txtBars.Name = "txtBars";
            this.txtBars.Size = new System.Drawing.Size(100, 20);
            this.txtBars.TabIndex = 6;
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(258, 56);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(100, 20);
            this.txtInterval.TabIndex = 5;
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(84, 56);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(100, 20);
            this.txtSymbol.TabIndex = 4;
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
            // lblSymbol
            // 
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new System.Drawing.Point(47, 59);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(31, 13);
            this.lblSymbol.TabIndex = 2;
            this.lblSymbol.Text = "Ativo";
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
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(204, 59);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(48, 13);
            this.lblInterval.TabIndex = 0;
            this.lblInterval.Text = "Intervalo";
            // 
            // tabScripts
            // 
            this.tabScripts.Controls.Add(this.tabBuyScript);
            this.tabScripts.Controls.Add(this.tabSellScript);
            this.tabScripts.Location = new System.Drawing.Point(3, 129);
            this.tabScripts.Name = "tabScripts";
            this.tabScripts.SelectedIndex = 0;
            this.tabScripts.Size = new System.Drawing.Size(963, 468);
            this.tabScripts.TabIndex = 2;
            // 
            // tabBuyScript
            // 
            this.tabBuyScript.Location = new System.Drawing.Point(4, 22);
            this.tabBuyScript.Name = "tabBuyScript";
            this.tabBuyScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabBuyScript.Size = new System.Drawing.Size(955, 442);
            this.tabBuyScript.TabIndex = 0;
            this.tabBuyScript.Text = "Compra";
            this.tabBuyScript.UseVisualStyleBackColor = true;
            // 
            // tabSellScript
            // 
            this.tabSellScript.Location = new System.Drawing.Point(4, 22);
            this.tabSellScript.Name = "tabSellScript";
            this.tabSellScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabSellScript.Size = new System.Drawing.Size(955, 442);
            this.tabSellScript.TabIndex = 1;
            this.tabSellScript.Text = "Venda";
            this.tabSellScript.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(390, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(576, 84);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(810, 100);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 4;
            this.btTest.Text = "Teste";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(891, 100);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 5;
            this.btSave.Text = "Salvar";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // ctlScripts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.tabScripts);
            this.Controls.Add(this.grpData);
            this.Name = "ctlScripts";
            this.Size = new System.Drawing.Size(985, 600);
            this.Load += new System.EventHandler(this.ctlScripts_Load);
            this.grpData.ResumeLayout(false);
            this.grpData.PerformLayout();
            this.tabScripts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.ComboBox cboScript;
        private System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.ComboBox cboPeriodicity;
        private System.Windows.Forms.TextBox txtBars;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.Label lblPeriodicity;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblBars;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.TabControl tabScripts;
        private System.Windows.Forms.TabPage tabBuyScript;
        private System.Windows.Forms.TabPage tabSellScript;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Button btSave;

    }
}
