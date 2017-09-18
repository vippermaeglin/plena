namespace M4
{
    partial class frmHelp2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp2));
            this.Label2 = new System.Windows.Forms.Label();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.White;
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(-2, 9);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(435, 132);
            this.Label2.TabIndex = 9;
            this.Label2.Text = resources.GetString("Label2.Text");
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(344, 149);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(73, 23);
            this.radButton1.TabIndex = 10;
            this.radButton1.Text = "&OK";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.BackColor = System.Drawing.Color.White;
            this.LinkLabel1.Location = new System.Drawing.Point(-2, 51);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(40, 13);
            this.LinkLabel1.TabIndex = 11;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "http://";
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // frmHelp2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 184);
            this.Controls.Add(this.LinkLabel1);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.Label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHelp2";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About PLENA";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        private Telerik.WinControls.UI.RadButton radButton1;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
    }
}
